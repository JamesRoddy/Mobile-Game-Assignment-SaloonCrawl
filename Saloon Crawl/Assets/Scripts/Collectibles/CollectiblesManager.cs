using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> collectibles;
    private List<CollectiblesPool> collectiblesPools;
    playerController playerController;
    int currentAmountToSpawn = 0;
    void Start()
    {

        playerController = FindObjectOfType<playerController>();
        for(int i = 0; i < collectibles.Count; i++)
        {

           CollectiblesPool currentPool =  gameObject.AddComponent<CollectiblesPool>();
           Collectible temp = collectibles[i].GetComponent<Collectible>();
           currentPool.setValues(collectibles[i], temp.MaxAmount, temp.MinAmount);

            collectiblesPools.Add(currentPool);
        }




    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // pick random collecitble pools in range and then choose a certain amount from each pool 
    // depending on amount avaialble in each   

    void selectPools()
    {

        int poolNummber = Random.Range(1, collectiblesPools.Count + 1);
        int totalObjects = 0;
        int previousRandom = 0;


        for (int i = 0; i < poolNummber; i++)
        {
            int randomPool = Random.Range(0, collectiblesPools.Count);
            previousRandom = randomPool;
            if (previousRandom - randomPool == 0)
            {

                randomPool = previousRandom < collectiblesPools.Count / 2 ? Random.Range(randomPool, collectiblesPools.Count) : Random.Range(0, randomPool);

            }

            totalObjects += collectiblesPools[randomPool].MaxPoolAmount;


        }



    }
         
       

}
