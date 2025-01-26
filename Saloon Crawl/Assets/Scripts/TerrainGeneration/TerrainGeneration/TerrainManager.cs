using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TerrainManager : MonoBehaviour
{


    // main central class repsonsilbe for hanldin gterrain genertaion with all the pool objects and pool managers attached 
    [SerializeField] int terrainMinSpawnLimit = 3;

    [SerializeField] List<GameObject> terrainTypeObjects = new List<GameObject>();
    private TerrainClassifications nextTerrain;
    private GameObject nextActiveTerrain;
    private Vector3 initialPos = Vector3.zero;
    private float localScaleFratcionDivider = 4.0f;
    Vector3 genericPadding = new Vector3(-0.01f, 0.0f, 0.0f); 
    
  
    private int currentTerrainCycles = 0;

    private playerController playerController;
    private List<GameObject> activeTerrain;

    // diciotnaires used to store all of the initialised pools based on their associated terrain type 
    private Dictionary<TerrainClassifications, PoolTerrainManager> terrainPools = new Dictionary<TerrainClassifications, PoolTerrainManager>();
    private Dictionary<TerrainClassifications, Enemypool> enemyPools = new Dictionary<TerrainClassifications, Enemypool>();
    private Dictionary<TerrainClassifications, InteractablePool> interactableObjectPools = new Dictionary<TerrainClassifications, InteractablePool>();

    // all of the managers for the pools
    private EnemySpawner enemySpawnHandler;
    private InteractableSpawnManager interactableSpawnManager;
    private CollectiblesManager collectiblesManager;

    private PlayerCameraShift playerCamShift;
    private Transform terrainColliderHolder; // generic terrain collider that moves with the player 
    private Camera cam;
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        terrainColliderHolder = transform.GetChild(0).GetComponent<Transform>();
        playerCamShift = FindFirstObjectByType<PlayerCameraShift>();
        cam = Camera.main;

        initialzeStartingTerrain(); // intit all terrain pools and objects 
        setPlayerCurrentTerrain();  // set the players current terrain used to determine when pool managers should dynamically swicth the current pool they are managing based on current and next terrian type for the player 


        enemySpawnHandler = gameObject.AddComponent<EnemySpawner>(); 
        interactableSpawnManager = gameObject.AddComponent<InteractableSpawnManager>();
        collectiblesManager = FindFirstObjectByType<CollectiblesManager>();
        interactableSpawnManager.CurrentPool = GetComponent<InteractablePool>(); // set the current interatcable object pool
        // intialise all pool manager components with their associated terrain objects 
        interactableSpawnManager.interactbleSpawnManagerStart();
        enemySpawnHandler.EnemySpanwnManagerStart();
        collectiblesManager.CollectibleManagerStart();
    } 
    private void updateActiveObjects()
    {

        // update all active event objects and manage when they need to be deactivated
        for (int i = 0; i < activeTerrain.Count; i++)
        {

            Vector3 normalizeViewportPosition = cam.WorldToViewportPoint(activeTerrain[i].transform.position + activeTerrain[i].GetComponent<TerrainType>().getHalfScale); 
            
            if (!(normalizeViewportPosition.x > 0.0f) && !playerController.IsViewingNextTerrain && !playerCamShift.IsShiftingBack) // if the terrain object is fully off screen and the player is not intentially viewing the next terrain using the camera controls(accelerometer,multi touch zoom, dragging with touch)
            {

                activeTerrain[i].SetActive(false); // the object no longer needs to be updated to deactivate it to stop any calls to its components(reducing the load of having multiple objects active but off screen)
                activeTerrain[i].GetComponent<TerrainType>().ResetTerrain(); // reset all values 
                activeTerrain.RemoveAt(i); // remove from the active pool objects list
              

            }

        }
        setPlayerCurrentTerrain(); // set current terrain of player after update 



    }






    private void getNewTerrainChunck(bool shouldGenerate) //  main generation algorithm using the pool manager associated with the current terrain type 
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


        if (spawnCount == terrainMinSpawnLimit) // if we have not hit our step limit 
        {
            return;
        }
        nextTerrain = currentType;// set the next terrain to the current chosen terrain type 
    
        if (currentPool.hasHitSuccessionCount())
        {

            // if the current terrain pool has been requested a certain amount 
            // meaning that there is a certain amount of the same terrain type object in one row  
            // this is define by the terrain type object the current pool is pooling 
            valdiateNextTerrainOption(currentPool.getAdjacencyOptions()); // validate the potential options that may be different to the current terrain type  based on the terrain objects defined adjacency rules 
            if (nextTerrain != currentType) // if we didnt hit the same terrain type
            {
                currentPool.resetSuccessionCount(); // reset the current succession count of the chosen  poolTerrainManager(from the terrain pools dictionary)
            }
          
            currentPool = terrainPools[nextTerrain];// dynamically swap the current terrain pool during the algorithm to use the current terrain chosen 

        }

        if (currentPool.allTerrainActive())
        {
            // if the current pool we are using in the algorithm has all its objects activateds 
            valdiateNextTerrainOption(currentPool.getAdjacencyOptions(), currentType); // validate terrain adajcency options and choose the next terrain
            if (nextTerrain == currentType) // if we hit the same terrain we break and wait for more objects to become inactive 
            {
             
                return;
            }

            currentPool = terrainPools[nextTerrain]; // adjust current terrain pool being used 
          


        }


        // set up the current terrain object based on the previous terrain chosen in the previous step 
        GameObject previous = activeTerrain[activeTerrain.Count - 1]; 
        TerrainType previousTerrain =  previous.GetComponent<TerrainType>();
        previousTerrain.NextTerrainOn = nextTerrain;

        requestTerrainFromPool(currentPool.PoolTerrain);  // request the final terrain object from the chosen pool above 
        // set up current chosen terrain object 
        GameObject current = activeTerrain[activeTerrain.Count - 1]; 
        TerrainType currentTerrain = current.GetComponent<TerrainType>();  
        previousTerrain.NextAdjacentTerrainTile = current; // 
        previousTerrain.NextTerrainType = currentTerrain;

        spawnCount = spawnCount + 1; /// incrment the terrain object spawn count for the next step of the algorithm

        Vector3 newPosition = getNewPositionOnX(previous, current);

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



    }



    private void setPlayerCurrentTerrain()
    {

        if( playerController.CurrentTerrain.NextTerrainType != null && !(playerController.isInCurrentTerrian()) )
        {
            Debug.Log("player was not on terrain " + playerController.CurrentTerrain.GetClassification + " but was still visisble ");
         playerController.CurrentTerrain = playerController.CurrentTerrain.NextTerrainType;
           

     }
       


       

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
                terrainOptions.Add(terrainClassifications);
            }


        }

        if (terrainOptions.Count == 0)
        {
            nextTerrain = exluding;
            return;
        }
        nextTerrain = terrainOptions[Random.Range(0, terrainOptions.Count)];


    }




    private void requestTerrainFromPool(TerrainClassifications type) 
    {
        GameObject terrain = terrainPools[type].Pool[terrainPools[type].getAvailableObjectIndex()]; // get the current available index of the terrain pool and request the object
        if (activeTerrain.Count > 0) // if we have active terrain objects already 
        {
            // check for adjacenecy 
            TerrainClassifications previousTerrain = activeTerrain[activeTerrain.Count - 1].GetComponent<TerrainType>().GetClassification;
            bool wasEqualToPrevious = previousTerrain == type;
            terrain.GetComponent<TerrainType>().IsConnected = wasEqualToPrevious;
            if (wasEqualToPrevious || terrainPools[type].MaxPoolNum == 1)
            {
                terrainPools[type].RequestsInSuccession++;
            }

        }
        activeTerrain.Add(terrain); // add to current active terrain list





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


        // init all terrain type objects and pool managers on start 
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
      
        // set up inital terrain 

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
           playerController.getTerrainCycles = currentTerrainCycles;
            return true;
        }
        return false;

    }

    



}
