using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class CollectiblesManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> collectibles;
    private List<CollectiblesPool> collectiblesPools = new List<CollectiblesPool>();
    private playerController playerController;
    private CollectiblesPool currentPool;
    private CamMovement playerCamera;

    private int spawnCount = 0;
    private float collectibleOffsetPercentMin = -0.5f;
    private float collectibelOffsetPercentMax = 0.5f;
    private float randomnessClampMin = 0.2f;
    private float randomnessClampMax = 1.0f;
    private float heightOffsetScalar = 2.0f;

    private int collectiblePoolPointer = 0;
    private float randomOffset = 0.0f;
    private float finalPercent = 0.0f;
    private int totalObjects = 0;
    List<CollectiblesPool> tempCollectibileStore  = new List<CollectiblesPool>();    
    void Start()
    {
       
        playerController = FindObjectOfType<playerController>();
        Debug.Log("collectibles count "+collectibles.Count);
        playerCamera = Camera.main.GetComponent<CamMovement>();
        foreach( GameObject collectible in collectibles)
        {

           CollectiblesPool currentPool =  gameObject.AddComponent<CollectiblesPool>();
            Debug.Log("current pool is null "+(currentPool ==null));
            int minAmount = collectible.GetComponent<Collectible>().MinAmount; 
            int maxAmount = collectible.GetComponent<Collectible>().MaxAmount;
            Debug.Log("min collectibles amount " + minAmount + " max collectibles amount " + maxAmount);
            Debug.Log("collectibile was null " + (collectible == null));

           currentPool.setValues(collectible, maxAmount, minAmount);
           collectiblesPools.Add(currentPool);

        }



    }

    // Update is called once per frame
    public void UpdateSpawns()
    {

        if(playerController.CurrentTerrain.NextTerrainType != null && playerController.CurrentTerrain.NextTerrainType.HasCollectibles == false && playerCamera.playerCanSeeEnd())
        {
            Debug.Log("condition hit to generate new collectibles next terrain was not null " + (playerController.CurrentTerrain.NextTerrainType != null)+" has interactables was "+playerController.CurrentTerrain.NextTerrainType.HasCollectibles);
            playerController.CurrentTerrain.NextTerrainType.HasCollectibles = true;
            tempCollectibileStore.Clear();
            collectiblePoolPointer = 0;
            spawnCount = 0;
            totalObjects = 0;
            genCollectibles();





        }

        getPercentageOfObjectsFromPool();
       

    }


    // pick random collecitble pools in range and then choose a certain amount from each pool 
    // depending on amount avaialble in each   



    void genCollectibles()
    {

        
        int poolNummber = Random.Range(1, collectiblesPools.Count+1 );
        Debug.Log("SPAWNING COLLECTIBLES random amount to select pools " + poolNummber);
        int previousRandom = 0;
         for (int i = 0; i < poolNummber; i++)
        {
            int randomPool = Random.Range(0, collectiblesPools.Count);
            previousRandom = randomPool;
            Debug.Log("SPAWNING COLLECTIBLES ranomd number to select collectible pool  " + randomPool);

            if (previousRandom - randomPool == 0 && i!=0)
            {
                Debug.Log("SPAWNING COLLECTIBLES previous was equal to current when selecting pool previous was  " + previousRandom);


                randomPool = previousRandom < collectiblesPools.Count / 2 ? Random.Range(randomPool, collectiblesPools.Count) : Random.Range(0, randomPool);

            }

            totalObjects += collectiblesPools[randomPool].MaxPoolAmount;
            
            tempCollectibileStore.Add(collectiblesPools[randomPool]);

        }
       
        Debug.Log(" SPAWNING COLLECTIBLES total sum of objects between pools when genertaing was " + totalObjects +"temp collectible store count "+tempCollectibileStore.Count);
        getNewRandomPercent();
      



    }
         

    private void getNewRandomPercent()
    {

        if(tempCollectibileStore.Count == 1)
        {
            finalPercent = Random.Range(0.2f, 1.0f);
            return;
        }

        randomOffset = Random.Range(-collectibleOffsetPercentMin, collectibelOffsetPercentMax);
        Debug.Log(" SPAWNING COLLECTIBLES  current random offest to apply to final percent for current pool "+randomOffset);
        finalPercent = Mathf.Clamp((float)tempCollectibileStore[  collectiblePoolPointer %tempCollectibileStore.Count].MaxPoolAmount / totalObjects + randomOffset, randomnessClampMin, randomnessClampMax);

        Debug.Log(" SPAWNING COLLECTIBLES final percentage to use for current pool " + finalPercent);
    }
  
       

    private void getPercentageOfObjectsFromPool()
    {
        if(collectiblePoolPointer != tempCollectibileStore.Count)
        {
            currentPool = tempCollectibileStore[collectiblePoolPointer];
            int numberOfCollectibles = Mathf.RoundToInt(currentPool.MaxPoolAmount * finalPercent);
            Debug.Log("num of collectibles for pool " + numberOfCollectibles);
            BoxCollider2D playerCollider = playerController.GetComponent<BoxCollider2D>();
            TerrainType playerNextTerrain = playerController.CurrentTerrain.NextTerrainType;
          

            if (currentPool.hasAvailableObject() && spawnCount != numberOfCollectibles)
            {
                GameObject requested = currentPool.getAvaialbleObject();
                requested.SetActive(true);
                Debug.Log(" SPAWNING COLLECTIBLE getting current collectibel from current pool ");
                requested.transform.position = new Vector3
                   (
                    playerNextTerrain.generateRandomX(),
                    Random.Range(playerNextTerrain.transform.position.y + currentPool.getCollectibleScale().y, playerNextTerrain.transform.position.y + playerCollider.bounds.size.y * heightOffsetScalar),
                    playerController.transform.position.z
                    );
                requested.GetComponent<Collectible>().CollectibleEnable();
                Debug.Log(" SPAWNING COLLECTIBLE getting current posiition " + requested.transform.position);

                spawnCount++;
                return;
            }
            spawnCount = 0;
            collectiblePoolPointer++;
            getNewRandomPercent();
            Debug.Log("collectiblePool has no objects left  is " + currentPool.hasAvailableObject() + " or spawn count for pool was hit " + (spawnCount == numberOfCollectibles));




        }






    }

}
