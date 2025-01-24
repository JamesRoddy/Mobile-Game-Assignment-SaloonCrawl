using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

public class PoolTerrainManager : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    private GameObject baseObject;
    private int poolPointer = 0;
    private int maxPoolNum = 0;
    private int objectsAvailableOnTerrainEnd = 0;

    private int requestsSuccessionCount = 0;
    private int maxSuccessionRequests = 0;
    private TerrainClassifications poolTerrain;

    public void setValues(GameObject terrainType, int maxNum)
    {

        baseObject = terrainType;
        maxPoolNum = maxNum;
       
        objectsAvailableOnTerrainEnd = maxPoolNum - 1;
        for (int i = 0; i < maxPoolNum; i++)
        {
            GameObject newTerrain = Instantiate(terrainType, Vector3.zero, Quaternion.identity);
            newTerrain.GetComponent<TerrainType>().setValues();
           
            maxSuccessionRequests = newTerrain.GetComponent<TerrainType>().getAdjacencyCount;
           
            newTerrain.SetActive(false);
            pool.Add(newTerrain);

        }

        poolTerrain = pool[0].GetComponent<TerrainType>().GetClassification;
        



    }


   

   public  List<TerrainClassifications> getAdjacencyOptions()
    {
        return pool[0].GetComponent<TerrainType>().AdjacencyTerrainTypes;
    }
    public bool validateTerrainType()
    {

        return pool[0].GetComponent<TerrainType>().Validate(); 


    }


    public bool requestWouldSucceed()
    {
        return  hasAvailableObject();
    }

    public List<GameObject> requestAvailabeObjects()
    {
        List<GameObject> objects = new List<GameObject>();

        foreach (GameObject terrain in pool)
        {
            if (!terrain.activeSelf)
            {
              
                objects.Add(terrain);
            }
        }

        return objects;
    }
    public void resetSuccessionCount()
    {
        
            requestsSuccessionCount = 0;
         
        
    }

    public bool hasHitSuccessionCount()
    {

        return requestsSuccessionCount >= maxSuccessionRequests;

    }



    public bool hasAvailableObject()
    {
        foreach (GameObject terrain in pool)
        {
            if (!terrain.activeSelf)
            {
                return true;
            }
        }

        return false;
    }



    public int getAvailableObjectIndex()
    {

        if (poolPointer == maxPoolNum  )
        {
            poolPointer = 0;
            if (pool[0].activeSelf)
            {
               /* Debug.Log("could not get available object from pool 0 was active  " + pool[0].activeSelf);*/
                return -1;
            }
      
        }
        
        pool[poolPointer].SetActive(true);
        
        return poolPointer++;

    }

    public bool allTerrainActive()
    {
        if (poolPointer == maxPoolNum)
        {
            poolPointer = 0;
        }
        return pool[poolPointer].activeSelf;


    }
    public TerrainClassifications PoolTerrain
    {
        get { return poolTerrain; }
    }
    public int MaxPoolNum
    {

        get { return maxPoolNum; }
    }
    public int RequestsInSuccession
    {
        set { requestsSuccessionCount = value; }
        get { return requestsSuccessionCount; }
    }
    public List<GameObject> Pool
    {
        get { return pool; }



    }



}
