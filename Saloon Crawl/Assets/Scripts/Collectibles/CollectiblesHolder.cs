using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesPool : MonoBehaviour
{

    private List<GameObject> pool = new List<GameObject>();
    private int maxPoolAmount = 0;
    private int minPoolAmount = 0;
    private int poolPointer = 0;
    public void setValues(GameObject collectiblesPrefab, int maxAmount, int minAmount)
    {
        maxPoolAmount = maxAmount;
        minPoolAmount = minAmount;
        for (int i = 0; i < maxPoolAmount; i++)
        {
            GameObject collectibleInstance = Instantiate(collectiblesPrefab, Vector3.zero, Quaternion.identity);
            collectibleInstance.SetActive(false);
            pool.Add(collectibleInstance);


        }




    }


    public bool hasAvailableObject()
    {

        if (poolPointer == maxPoolAmount)
        {
            poolPointer = 0;


        }


        return !pool[poolPointer].activeSelf;
    }

    public GameObject getAvaialbleObject()
    {

        return pool[poolPointer++];

    }


    public Vector3 getCollectibleScale()
    {
        return pool[0].transform.localScale;
    }

    

  



    public int MaxPoolAmount
    {
        get { return maxPoolAmount; }
    }


















}
