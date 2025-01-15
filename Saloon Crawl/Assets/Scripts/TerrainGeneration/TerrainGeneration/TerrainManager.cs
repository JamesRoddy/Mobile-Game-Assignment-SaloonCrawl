using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{



    [SerializeField] int terrainMinSpawnLimit = 3;
    
    [SerializeField] List<GameObject> terrainTypeObjects = new List<GameObject>();
    private TerrainClassifications nextTerrain;
    private Vector3 initialPos = Vector3.zero;
    Vector3 genericPadding = new Vector3(0.0005f,0.0f,0.0f);
/*    private bool terrainRequestFailed = false;
*/
    /*private int maxTerrainCycles = 4;*/
    private int currentTerrainCycles = 0;

    private playerController playerController;
    private List<GameObject> activeTerrain;
    private Dictionary<TerrainClassifications, PoolTerrainManager> terrainPools = new Dictionary<TerrainClassifications, PoolTerrainManager>();
    private Dictionary<TerrainClassifications, TerrainType> terrainData = new Dictionary<TerrainClassifications, TerrainType>();
    private List<GameObject> previousActiveTerrain;
    private PoolTerrainManager saloonPoolManager;
    private PoolTerrainManager desertPoolManager;
    private Camera cam;
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        cam = Camera.main;
        initialzeStartingTerrain();

    }
    private void updateActiveObjects()
    {

        for (int i = 0; i < activeTerrain.Count; i++)
        {

            Vector3 normalizeViewportPosition = cam.WorldToViewportPoint(activeTerrain[i].transform.position + activeTerrain[i].GetComponent<TerrainType>().getHalfScale);
            if (!(normalizeViewportPosition.x > 0.0f))
            {
                activeTerrain[i].SetActive(false);
                Debug.Log("object deactivated object bool set to " + terrainPools[activeTerrain[i].GetComponent<TerrainType>().GetClassification].Pool[0].activeSelf + " terrain type " + activeTerrain[i].GetComponent<TerrainType>().GetClassification);
                activeTerrain.RemoveAt(i);
                Debug.Log("new active terrain count " + activeTerrain.Count);

            }

        }


    }




    private void getNewTerrainChunck(bool shouldGenerate)
    {
        if (shouldGenerate)
        {
            Debug.Log("generating new chunk " + currentTerrainCycles);
            Vector3 positionToOffsetFrom = activeTerrain[activeTerrain.Count - 1].transform.position;
            GameObject currentTerrain = activeTerrain[activeTerrain.Count - 1];
            PoolTerrainManager currentPool = terrainPools[ currentTerrain.GetComponent<TerrainType>().GetClassification];
            TerrainClassifications currentType = currentPool.PoolTerrain;
            Debug.Log("current pool going into generation " + currentType);
            generationStep(currentType,currentPool, 1, positionToOffsetFrom);
               
        }








    }

    void generationStep(TerrainClassifications currentType,PoolTerrainManager currentPool, int spawnCount,Vector3 positionToOffsetFrom) {


        if(spawnCount == terrainMinSpawnLimit)
        {
            Debug.Log("terrain finished generating due to count");
            return;
        }
        nextTerrain = currentType;
        Debug.Log(
            "current terrain type in gen step " + currentType +
            "current pool " + currentPool.PoolTerrain +
            " current spawn count " + spawnCount +
            " current offset position " + positionToOffsetFrom);
        if (currentPool.hasHitSuccessionCount())
        {
            Debug.Log("succession count  was hit on step " + spawnCount + "current terrain type " + currentType);

            valdiateNextTerrainOption(currentPool.getAdjacencyOptions());
            if (nextTerrain != currentType)
            {
                Debug.Log("terrain succession count reset " + currentType + " next terrain to gen " + nextTerrain);
                currentPool.resetSuccessionCount();
            }
            else
            {
                Debug.Log("terrain succession count hit but not reset " + currentType);
            }
            Debug.Log("new succession count next terrrain  " + nextTerrain);
            currentPool = terrainPools[nextTerrain];

        }

        if (currentPool.allTerrainActive() )
        {
            Debug.Log("all terrain was hit on setp " + spawnCount + "current terrain type " + currentType);
            valdiateNextTerrainOption(currentPool.getAdjacencyOptions(),currentType);
            if(nextTerrain == currentType )
            {
                Debug.Log("generation step interrutped no objects in pool  :" + currentType + ":  ");
                return;
            }

            currentPool = terrainPools[nextTerrain];
            Debug.Log("new  terrrain due to all terrain being active   " + nextTerrain);


        }

        GameObject previous = activeTerrain[activeTerrain.Count - 1];
        requestTerrainFromPool(currentPool.PoolTerrain);
        Debug.Log(activeTerrain.Count + " new active terrain count ");
        spawnCount = spawnCount+1;
        Vector3 newPosition = getNewPositionOnX(previous, activeTerrain[activeTerrain.Count - 1]);
        Debug.Log("next position for terrain " + newPosition);
        activeTerrain[activeTerrain.Count - 1].transform.position = newPosition;
        generationStep(nextTerrain, currentPool, spawnCount, activeTerrain[activeTerrain.Count-1].transform.position);





    }


    private void valdiateNextTerrainOption(List<TerrainClassifications> adjacencyOptions)
    {
        List<TerrainClassifications> terrainOptions = new List<TerrainClassifications>();
        if(adjacencyOptions.Count == 1)
        {
            nextTerrain = adjacencyOptions[0];
            return;
        }


        foreach (TerrainClassifications terrainClassifications in adjacencyOptions) {

            if (terrainPools[terrainClassifications].validateTerrainType()  )
            {
                terrainOptions.Add(terrainClassifications);
            }
            
            
        } 

        nextTerrain = terrainOptions[Random.Range(0, terrainOptions.Count)];
        Debug.Log("next terrain on " + nextTerrain);


    }

    private void valdiateNextTerrainOption(List<TerrainClassifications> adjacencyOptions, TerrainClassifications exluding)
    {
        List<TerrainClassifications> terrainOptions = new List<TerrainClassifications>();
        if (adjacencyOptions.Count == 1)
        {
            nextTerrain = adjacencyOptions[0];
            return;
        }


        foreach (TerrainClassifications terrainClassifications in adjacencyOptions)
        {

            if (terrainPools[terrainClassifications].validateTerrainType() && terrainClassifications != exluding)
            {
                Debug.Log("terrain found when excluding " + terrainClassifications);
                terrainOptions.Add(terrainClassifications);
            }


        }

        if(terrainOptions.Count == 0)
        {
            Debug.Log("next terrain exlusion failed terrain: " + exluding);
            nextTerrain = exluding;
            return;
        }
        nextTerrain = terrainOptions[Random.Range(0, terrainOptions.Count)];
        Debug.Log("next terrain " + nextTerrain);


    }


    public void Update()
    {
        updateActiveObjects();
        getNewTerrainChunck(terrainCycleEnd());

    }
   
    private void requestTerrainFromPool(TerrainClassifications type)
    {
        GameObject terrain = terrainPools[type].Pool[terrainPools[type].getAvailableObjectIndex()];
        if(activeTerrain.Count > 0)
        {
            TerrainClassifications previousTerrain = activeTerrain[activeTerrain.Count - 1].GetComponent<TerrainType>().GetClassification;
            bool wasEqualToPrevious = previousTerrain == type;
            
            if (wasEqualToPrevious || terrainPools[type].MaxPoolNum == 1)
            {
                terrainPools[type].RequestsInSuccession++;
                Debug.Log("terrain requested in succession " + terrain.GetComponent<TerrainType>().GetClassification + " number " + terrainPools[type].RequestsInSuccession);
            }

        }
        activeTerrain.Add(terrain);
        
        
        
            

    }


    private Vector3 getNewPositionOnX(GameObject gameObjectToOffsetFrom,GameObject gamObjectToPlace)
    {

        float widthOffset = (gameObjectToOffsetFrom.GetComponent<BoxCollider2D>().bounds.size.x - gamObjectToPlace.GetComponent<BoxCollider2D>().bounds.size.x) / 2.0f;


        return new Vector3(gameObjectToOffsetFrom.transform.position.x + ((gamObjectToPlace.GetComponent<BoxCollider2D>().bounds.size.x + widthOffset)+genericPadding.x),
                                                              gameObjectToOffsetFrom.transform.position.y, gameObjectToOffsetFrom.transform.position.z);



    }


    private void initialzeStartingTerrain()
    {

        foreach (GameObject terrain in terrainTypeObjects)
        {

            TerrainClassifications classification = terrain.GetComponent<TerrainType>().GetClassification;

            int poolNum = terrain.GetComponent<TerrainType>().getAdjacencyCount;

            terrainPools[classification] = gameObject.AddComponent<PoolTerrainManager>();
            terrainPools[classification].setValues(terrain, poolNum);

        }
        saloonPoolManager = terrainPools[TerrainClassifications.SALOON];
        desertPoolManager = terrainPools[TerrainClassifications.DESERT];


        activeTerrain = new List<GameObject>() {};
        requestTerrainFromPool(TerrainClassifications.SALOON);
        requestTerrainFromPool(TerrainClassifications.DESERT);
        requestTerrainFromPool(TerrainClassifications.DESERT);


        initialPos = new Vector3(playerController.transform.position.x + activeTerrain[0].transform.localScale.x / 2.0f
                                     , (playerController.transform.position.y - activeTerrain[0].transform.localScale.y)-1.0f,
                                      playerController.transform.position.z);
        Vector3 newPosition = initialPos;
        activeTerrain[0].transform.position = initialPos;

        for (int i = 1; i < activeTerrain.Count; i++)
        {
            Debug.Log("active terrain scale " + activeTerrain[i-1].transform.localScale.x);

            float widthOffsetScaler = (activeTerrain[i-1].transform.localScale.x - activeTerrain[i].transform.localScale.x)/2.0f;
            Debug.Log("widht offset scalar " + widthOffsetScaler);
            activeTerrain[i].transform.position = getNewPositionOnX(activeTerrain[i-1],activeTerrain[i]);
            
            
            newPosition = activeTerrain[i].transform.position;
        }




    } 

    private bool terrainCycleEnd()
    {
        if(activeTerrain.Count == 1)
        {
            currentTerrainCycles++;
            return true;
        }
        return false;

    }




}
