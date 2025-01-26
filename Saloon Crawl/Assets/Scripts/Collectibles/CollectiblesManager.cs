using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class CollectiblesManager : MonoBehaviour
{
    // Start is called before the first frame update
    // main manager class for collectible spanwing and positioning  managing when an object should be requested from a paritcualr collectible pool 
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
    public  void CollectibleManagerStart()
    {
       // init collectible pools 
        playerController = FindObjectOfType<playerController>();
        playerCamera = Camera.main.GetComponent<CamMovement>();
        foreach( GameObject collectible in collectibles)
        {

           CollectiblesPool currentPool =  gameObject.AddComponent<CollectiblesPool>();
            int minAmount = collectible.GetComponent<Collectible>().MinAmount; 
            int maxAmount = collectible.GetComponent<Collectible>().MaxAmount;

           currentPool.setValues(collectible, maxAmount, minAmount);
           collectiblesPools.Add(currentPool);

        }



    }

    // Update is called once per frame
    public void UpdateSpawns()
    {

        if(playerController.CurrentTerrain.NextTerrainType != null && playerController.CurrentTerrain.NextTerrainType.HasCollectibles == false && playerCamera.playerCanSeeEnd())
        {
            playerController.CurrentTerrain.NextTerrainType.HasCollectibles = true;
            tempCollectibileStore.Clear();
            collectiblePoolPointer = 0;
            spawnCount = 0;
            totalObjects = 0;
            genCollectibles();





        }

        getPercentageOfObjectsFromPool();
       

    }


   
    void genCollectibles()
    {
        // pick random collecitble pools in range and then choose a certain amount from each pool 
        // depending on amount available in each   

        int poolNummber = Random.Range(1, collectiblesPools.Count+1 );
        int previousRandom = 0;
         for (int i = 0; i < poolNummber; i++)
        {
            int randomPool = Random.Range(0, collectiblesPools.Count);
            previousRandom = randomPool;

            if (previousRandom - randomPool == 0 && i!=0)
            {


                randomPool = previousRandom < collectiblesPools.Count / 2 ? Random.Range(randomPool, collectiblesPools.Count) : Random.Range(0, randomPool);

            }

            totalObjects += collectiblesPools[randomPool].MaxPoolAmount;
            
            tempCollectibileStore.Add(collectiblesPools[randomPool]);

        }
       
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
        finalPercent = Mathf.Clamp((float)tempCollectibileStore[  collectiblePoolPointer %tempCollectibileStore.Count].MaxPoolAmount / totalObjects + randomOffset, randomnessClampMin, randomnessClampMax);

    }
  
       

    private void getPercentageOfObjectsFromPool()
    {
        if(collectiblePoolPointer != tempCollectibileStore.Count)
        {
            currentPool = tempCollectibileStore[collectiblePoolPointer];

            int numberOfCollectibles = Mathf.RoundToInt(currentPool.MaxPoolAmount * finalPercent);
            BoxCollider2D playerCollider = playerController.GetComponent<BoxCollider2D>();
            TerrainType playerNextTerrain = playerController.CurrentTerrain.NextTerrainType;
          

            if (currentPool.hasAvailableObject() && spawnCount != numberOfCollectibles)
            {
                GameObject requested = currentPool.getAvaialbleObject();
                requested.SetActive(true);
                requested.transform.position = new Vector3
                   (
                    playerNextTerrain.generateRandomX(),
                    Random.Range(playerNextTerrain.transform.position.y + currentPool.getCollectibleScale().y, playerNextTerrain.transform.position.y + playerCollider.bounds.size.y * heightOffsetScalar),
                    playerController.transform.position.z
                    );
                requested.GetComponent<Collectible>().CollectibleEnable();

                spawnCount++;
                return;
            }

            spawnCount = 0;
            collectiblePoolPointer++;
            getNewRandomPercent();




        }






    }

}
