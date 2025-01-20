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






}
