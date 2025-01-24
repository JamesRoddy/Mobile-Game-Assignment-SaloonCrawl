using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.PackageManager;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{



    [SerializeField] int terrainMinSpawnLimit = 3;

    [SerializeField] List<GameObject> terrainTypeObjects = new List<GameObject>();
    private TerrainClassifications nextTerrain;
    private GameObject nextActiveTerrain;
    private Vector3 initialPos = Vector3.zero;
    private float localScaleFratcionDivider = 4.0f;
    Vector3 genericPadding = new Vector3(-0.01f, 0.0f, 0.0f); 
    /*    private bool terrainRequestFailed = false;
    */
    /*private int maxTerrainCycles = 4;*/ 
  
    private int currentTerrainCycles = 0;

    private playerController playerController;
    private List<GameObject> activeTerrain;
    private Dictionary<TerrainClassifications, PoolTerrainManager> terrainPools = new Dictionary<TerrainClassifications, PoolTerrainManager>();
    private Dictionary<TerrainClassifications, Enemypool> enemyPools = new Dictionary<TerrainClassifications, Enemypool>();
    private Dictionary<TerrainClassifications, InteractablePool> interactableObjectPools = new Dictionary<TerrainClassifications, InteractablePool>();
    private EnemySpawner enemySpawnHandler;
    private InteractableSpawnManager interactableSpawnManager;
    private CollectiblesManager collectiblesManager;

    private Transform terrainColliderHolder;
    private Camera cam;
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        terrainColliderHolder = transform.GetChild(0).GetComponent<Transform>();
        cam = Camera.main;
        initialzeStartingTerrain();
        setPlayerCurrentTerrain(); 
        enemySpawnHandler = gameObject.AddComponent<EnemySpawner>(); 
        interactableSpawnManager = gameObject.AddComponent<InteractableSpawnManager>();
        collectiblesManager = FindFirstObjectByType<CollectiblesManager>();
        Debug.Log((terrainColliderHolder == null) + "terrain collider null");
    } 
    private void updateActiveObjects()
    {

        
        for (int i = 0; i < activeTerrain.Count; i++)
        {

            Vector3 normalizeViewportPosition = cam.WorldToViewportPoint(activeTerrain[i].transform.position + activeTerrain[i].GetComponent<TerrainType>().getHalfScale); 
            
            if (!(normalizeViewportPosition.x > 0.0f) && !playerController.IsViewingNextTerrain)
            {

                activeTerrain[i].SetActive(false);
                activeTerrain[i].GetComponent<TerrainType>().ResetTerrain();
                activeTerrain.RemoveAt(i);
              

            }

        }
        setPlayerCurrentTerrain();



    }






    private void getNewTerrainChunck(bool shouldGenerate)
    {
        if (shouldGenerate)
        {
            Debug.Log("generating new chunk " + currentTerrainCycles);
         Vector3 positionToOffsetFrom = activeTerrain[activeTerrain.Count - 1].transform.position;
            GameObject currentTerrain = activeTerrain[activeTerrain.Count - 1];
            PoolTerrainManager currentPool = terrainPools[currentTerrain.GetComponent<TerrainType>().GetClassification];
            TerrainClassifications currentType = currentPool.PoolTerrain;
/*            Debug.Log("current pool going into generation " + currentType);
*/          TerrainGenerationPass(currentType, currentPool, 1);

             




        }








    }

    public void Update()
    {
        updateActiveObjects();
        getNewTerrainChunck(terrainCycleEnd());
        updateTerrainCollider();
        interactableSpawnManager.UpdateSpawns();
        collectiblesManager.UpdateSpawns();
        enemySpawnHandler.UpdateSpawns();
        
        
    }

    public Enemypool getEnemPool(TerrainClassifications type)
    {
        return enemyPools[type];
    }
     
    public InteractablePool getInteractablePool(TerrainClassifications type)
    {
        return interactableObjectPools[type];
    }

    private void TerrainGenerationPass(TerrainClassifications currentType, PoolTerrainManager currentPool, int spawnCount) {


        if (spawnCount == terrainMinSpawnLimit)
        {
        /*    Debug.Log("terrain finished generating due to count");*/
            return;
        }
        nextTerrain = currentType;
      /*  Debug.Log(
            "current terrain type in gen step " + currentType +
            "current pool " + currentPool.PoolTerrain +
            " current spawn count " + spawnCount);*/
        if (currentPool.hasHitSuccessionCount())
        {
/*            Debug.Log("succession count  was hit on step " + spawnCount + "current terrain type " + currentType);
*/
            valdiateNextTerrainOption(currentPool.getAdjacencyOptions());
            if (nextTerrain != currentType)
            {
               /* Debug.Log("terrain succession count reset " + currentType + " next terrain to gen " + nextTerrain);*/
                currentPool.resetSuccessionCount();
            }
            /*else
            {
                *//*Debug.Log("terrain succession count hit but not reset " + currentType);*//*
            }*/
           /* Debug.Log("new succession count next terrrain  " + nextTerrain);*/
            currentPool = terrainPools[nextTerrain];

        }

        if (currentPool.allTerrainActive())
        {
            /*Debug.Log("all terrain was hit on setp " + spawnCount + "current terrain type " + currentType);*/
            valdiateNextTerrainOption(currentPool.getAdjacencyOptions(), currentType);
            if (nextTerrain == currentType)
            {
             /*   Debug.Log("generation step interrutped no objects in pool  :" + currentType + ":  ");*/
                return;
            }

            currentPool = terrainPools[nextTerrain];
          /*  Debug.Log("new  terrrain due to all terrain being active   " + nextTerrain);*/


        }

        GameObject previous = activeTerrain[activeTerrain.Count - 1]; 
        TerrainType previousTerrain =  previous.GetComponent<TerrainType>();
        previousTerrain.NextTerrainOn = nextTerrain;
/*        Debug.Log(" ENEMY SPAWNING set previous terrain  " + previousTerrain.NextTerrainOn + "previous terrain was "+currentType);
*/
        requestTerrainFromPool(currentPool.PoolTerrain); 
        GameObject current = activeTerrain[activeTerrain.Count - 1];
        TerrainType currentTerrain = current.GetComponent<TerrainType>();  
        previousTerrain.NextAdjacentTerrainTile = current;
        previousTerrain.NextTerrainType = currentTerrain;
/*        Debug.Log(" ENEMY SPAWNING set previous terrain next terrain type  " + previousTerrain.NextTerrainType.GetClassification + "game object is null  = " + (previousTerrain.NextAdjacentTerrainTile == null));
*/
/*        Debug.Log(activeTerrain.Count + " new active terrain count ");
*/        spawnCount = spawnCount + 1;
        Vector3 newPosition = getNewPositionOnX(previous, current);
    /*    Debug.Log("next position for terrain " + newPosition);*/
        current.transform.position = newPosition;
        currentTerrain.TerrainEnable();
        currentTerrain.setGenericValues();
        currentTerrain.generatePositionsInteractables();
        TerrainGenerationPass(nextTerrain, currentPool, spawnCount);





    }



    
    private void valdiateNextTerrainOption(List<TerrainClassifications> adjacencyOptions)
    {
        List<TerrainClassifications> terrainOptions = new List<TerrainClassifications>();
        if (adjacencyOptions.Count == 1)
        {
            nextTerrain = adjacencyOptions[0];
            return;
        }


        foreach (TerrainClassifications terrainClassifications in adjacencyOptions) {

            if (terrainPools[terrainClassifications].validateTerrainType())
            {
                terrainOptions.Add(terrainClassifications);
            }


        }

        nextTerrain = terrainOptions[Random.Range(0, terrainOptions.Count)];
        /*Debug.Log("next terrain on " + nextTerrain);*/


    }



    private void setPlayerCurrentTerrain()
    {

        if( playerController.CurrentTerrain.NextTerrainType != null && !(playerController.isInCurrentTerrian()) )
        {
            Debug.Log("player was not on terrain " + playerController.CurrentTerrain.GetClassification + " but was still visisble ");
         playerController.CurrentTerrain = playerController.CurrentTerrain.NextTerrainType;
           
/*            Debug.Log(" new player terrain " + playerController.CurrentTerrain);
*/        }
       


       

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
                /*Debug.Log("terrain found when excluding " + terrainClassifications);*/
                terrainOptions.Add(terrainClassifications);
            }


        }

        if (terrainOptions.Count == 0)
        {
         /*   Debug.Log("next terrain exlusion failed terrain: " + exluding);*/
            nextTerrain = exluding;
            return;
        }
        nextTerrain = terrainOptions[Random.Range(0, terrainOptions.Count)];
      /*  Debug.Log("next terrain " + nextTerrain);*/


    }




    private void requestTerrainFromPool(TerrainClassifications type)
    {
        GameObject terrain = terrainPools[type].Pool[terrainPools[type].getAvailableObjectIndex()];
        if (activeTerrain.Count > 0)
        {
            TerrainClassifications previousTerrain = activeTerrain[activeTerrain.Count - 1].GetComponent<TerrainType>().GetClassification;
            bool wasEqualToPrevious = previousTerrain == type;
            terrain.GetComponent<TerrainType>().IsConnected = wasEqualToPrevious;
            if (wasEqualToPrevious || terrainPools[type].MaxPoolNum == 1)
            {
                terrainPools[type].RequestsInSuccession++;
           /*     Debug.Log("terrain requested in succession " + terrain.GetComponent<TerrainType>().GetClassification + " number " + terrainPools[type].RequestsInSuccession);*/
            }

        }
        activeTerrain.Add(terrain);





    }

    private void updateTerrainCollider()
    {


        
        if (playerController.isOnRightSideByCertainFractionOfScale(localScaleFratcionDivider))
        {
            terrainColliderHolder.transform.position = playerController.CurrentTerrain.SpawnRight;
            return;
        }
        if(terrainColliderHolder.transform.position != playerController.CurrentTerrain.transform.position)
        {
            terrainColliderHolder.transform.localScale = playerController.CurrentTerrain.transform.localScale;
            terrainColliderHolder.transform.position = playerController.CurrentTerrain.transform.position;
        }
   
        

    }

    private Vector3 getNewPositionOnX(GameObject gameObjectToOffsetFrom, GameObject gamObjectToPlace)
    {

        float widthOffset = (gameObjectToOffsetFrom.transform.localScale.x - gamObjectToPlace.transform.localScale.x) / 2.0f;
        return new Vector3(gameObjectToOffsetFrom.transform.position.x + ((gamObjectToPlace.transform.localScale.x + widthOffset) + genericPadding.x),
                                                              gameObjectToOffsetFrom.transform.position.y, gameObjectToOffsetFrom.transform.position.z);

    }


    private void initialzeStartingTerrain()
    {

        foreach (GameObject terrain in terrainTypeObjects)
        {

            TerrainType terrainType = terrain.GetComponent<TerrainType>();

            TerrainClassifications classification = terrainType.GetClassification;

            int poolNum = terrainType.getAdjacencyCount;
            int minEnemyNum = terrainType.MinEnemies;
            int maxEnemyNum = terrainType.MaxEnemies;
            List<GameObject> enemies = terrainType.Enemies;
            List<GameObject> interactables = terrainType.Interactables;
            terrainPools[classification] = gameObject.AddComponent<PoolTerrainManager>();

            terrainPools[classification].setValues(terrain, poolNum);
            enemyPools[classification] = gameObject.AddComponent<Enemypool>();
            enemyPools[classification].setValues(enemies, maxEnemyNum, minEnemyNum);

            interactableObjectPools[classification] = gameObject.AddComponent<InteractablePool>();
            interactableObjectPools[classification].setValues(interactables,poolNum);
        }
      


        activeTerrain = new List<GameObject>();
        requestTerrainFromPool(TerrainClassifications.SALOON);
        


        initialPos = new Vector3((playerController.transform.position.x-2.0f) + ((activeTerrain[0].transform.localScale.x / 2.0f))
                                     , (playerController.transform.position.y - (activeTerrain[0].transform.localScale.y+1.0f)) ,
                                      playerController.transform.position.z);
        Vector3 newPosition = initialPos;
        activeTerrain[0].transform.position = initialPos;

        activeTerrain[0].GetComponent<TerrainType>().TerrainEnable();

        playerController.CurrentTerrain = activeTerrain[0].GetComponent<TerrainType>();

        terrainColliderHolder.localScale =  playerController.CurrentTerrain.transform.localScale;
        terrainColliderHolder.transform.position = playerController.CurrentTerrain.transform.position;

    }



    


    private bool terrainCycleEnd()
    {
        if (playerController.CurrentTerrain.NextTerrainType == null)
        { 
           
            currentTerrainCycles++;
          Debug.Log("bool to generate terrain hit current number of cycles  " + currentTerrainCycles);
           playerController.getTerrainCycles = currentTerrainCycles;
            return true;
        }
        return false;

    }

    



}
