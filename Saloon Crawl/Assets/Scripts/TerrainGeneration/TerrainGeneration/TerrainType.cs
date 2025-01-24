using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TerrainType : MonoBehaviour
{
    // terrain transform properties
    private Vector2 groundSize;
    protected Transform ground;

    // interactables 
    [SerializeField] protected List<GameObject> interactables;
    [SerializeField] protected List<GameObject> interactableSpawnPositions;
    [SerializeField] protected int minInteractables;
    [SerializeField] protected int maxInteractables;
    [SerializeField] protected GameObject backGroundTopContainer;
    [SerializeField] protected GameObject backGroundBottomContainer;

    private SpriteRenderer backgroundTop;
    private SpriteRenderer backgroundBottom;
    private List<Vector3> currentSpawnPositions = new List<Vector3>();

    private float interactablesSpawnDivder = 2.0f;
    private float interactablesSpawnPadding = 2.0f;
    private Vector3 boundingMin; 
    private Vector3 boundingMax;
    private int currentMaxInteractables = 0;
    private int interactablesCounter = 0;
    protected bool hasInteractables = false;
    Vector3 interactableSpawnRight;
    Vector3 interactableSpawnLeft;
    Vector3 tempPositionForInteractable = Vector3.zero;
    Bounds spriteBoundsSum;
    private bool hasDefferedInteactableSpawn = false;
    Vector3 spawnRight;
    Vector3 spawnLeft;

    // enemies
    [SerializeField] protected List<GameObject> enemies;
    [SerializeField] protected int minEnemies;
    [SerializeField] protected int maxEnemies;

    protected bool hasSpawnPositions = false;
    protected int currentEnemiesCount = 0;
    protected int currentEnemySpawnCount = 0;


    // terrain 
    [SerializeField] protected List<TerrainClassifications> validAdjacentTerrainTypes;
    [SerializeField] protected TerrainClassifications classification;
    [SerializeField] protected int adjacencyCount;

    private bool isConnected = false;
    private TerrainClassifications nextTerrainOn;
    private GameObject nextTerrainTyle;
    private TerrainType nextTerrainType;

    // collectibles 
    private bool hasCollectibles = false;

    public abstract void setSpawnPositions();
    public void activateObject(ref GameObject obj, Vector3 position) {

        obj.SetActive(true);
        obj.transform.position = position;

        
    }

    public abstract void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor);


    public void spawnInteractableObjects(InteractablePool pool)
    {
      /*  Debug.Log("SPAWNING INTERACTABLES  spawning at generated  positions  current max interactables " + currentMaxInteractables);*/

       
        if (interactablesCounter < currentMaxInteractables )
        {/*
            Debug.Log("SPAWNING INTERACTABLES requesting object for " + classification + "current interactables count " + interactablesCounter +" current max "+currentMaxInteractables);*/
            GameObject interactable = pool.getRandomAvailableObject();
            if (pool.HasDeffered)
            {
                if (currentSpawnPositions
                    [interactablesCounter].x < transform.position.x && tempPositionForInteractable == Vector3.zero)
                { 
                    hasDefferedInteactableSpawn = true;
                    /*Debug.Log(" SPAWNING INTERACTABLES had to shift interactable spawn position to the right due to it being defferred");*/

                }
              /*  Debug.Log(" SPAWNING INTERACTABLES pool has deffered object waiting ... ");*/

                return;
            }


            hasDefferedInteactableSpawn = false;
            interactable.SetActive(true);
            interactable.transform.position = currentSpawnPositions[interactablesCounter];
            interactablesCounter++;
           
          
      

            
        }

        if(interactablesCounter == currentMaxInteractables || interactablesCounter == interactableSpawnPositions.Count )
        {
         /*   Debug.LogWarning("if max interactables is great than number of spawnPositions " + interactablesCounter == interactableSpawnPositions.Count +" max interactables will never be hit if 1 is displayed ");
            Debug.Log(" SPAWNING INTERACTABLES interactable spanw count max hit for terrain " + classification);*/
            hasInteractables = true;

        }



    }



    public bool containsPoint(Vector3 point)
    {


        return point.x <= spawnRight.x && point.x >= spawnLeft.x;

    }
    public bool hasMetInteractableRequest()
    {
        return interactablesCounter>=currentMaxInteractables;
    }
    public void setValues()
    {
        currentEnemiesCount = minEnemies;
        ground = GetComponent<Transform>();
        groundSize.x = ground.localScale.x;
        groundSize.y = ground.localScale.y;

    }
    public void assignSpawnVlaue()
    {

        currentEnemiesCount = Random.Range(MinEnemies, MaxEnemies + 1);
       /* Debug.Log("assiging new spanw count " + currentEnemiesCount);*/

    }
  


    public float generateRandomX()
    {


        return Random.Range(spawnLeft.x, spawnRight.x + 1.0f);
    }


    public void setGenericValues()
    {
        backgroundTop = backGroundTopContainer.GetComponent<SpriteRenderer>();
        backgroundBottom = backGroundBottomContainer.GetComponent<SpriteRenderer>();
        spawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder), transform.position.y , transform.position.z);
        spawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder), transform.position.y , transform.position.z);
        hasCollectibles = false;

        spriteBoundsSum.center = Vector3.Lerp(backgroundTop.bounds.center, backgroundBottom.bounds.center, 0.5f);
        spriteBoundsSum.extents = new Vector3((backgroundTop.bounds.max.x - backgroundBottom.bounds.min.x) / 2.0f , (backgroundTop.bounds.max.y - backgroundBottom.bounds.min.y)/2.0f , (backgroundTop.bounds.max.z - backgroundBottom.bounds.min.z) / 2.0f);
        spriteBoundsSum.min = backgroundBottom.bounds.min;
        spriteBoundsSum.max = backgroundTop.bounds.max;
         Debug.Log("background max and min "+ spriteBoundsSum.max  +" "+spriteBoundsSum.min +"center "+spriteBoundsSum.center +" extents "+spriteBoundsSum.extents);
       
    }
    public void generatePositionsInteractables()
    {
        hasInteractables = false;
        interactableSpawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder)  - interactablesSpawnPadding, transform.position.y, transform.position.z);
        interactableSpawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder) + interactablesSpawnPadding, interactableSpawnRight.y, interactableSpawnRight.z);
        interactablesCounter = 0;
        currentSpawnPositions.Clear();
        currentMaxInteractables = Random.Range(minInteractables, maxInteractables+1);
        currentSpawnPositions.Capacity = currentMaxInteractables;
         float areaToAllocate = transform.localScale.x/currentMaxInteractables;

       
        Debug.Log("count for interactable objects " + interactables.Count);
        int random = Random.Range(0, interactableSpawnPositions.Count);
        GameObject randomSpawnPositions = interactableSpawnPositions[random];
        Debug.Log("SPAWNING INTERACTABLES new random " + random +" for class " + classification);
        int count = randomSpawnPositions.transform.childCount;
        Debug.Log("number of spawn positions " + count);
        Transform[] childrenTransforms = randomSpawnPositions.GetComponentsInChildren<Transform>(); ;

        for(int i = 0; i<count;i++)
        {

            Debug.Log(" SPAWNING INTERACTABLES adding new postion " + randomSpawnPositions.transform.GetChild(i).position);
            currentSpawnPositions.Add(randomSpawnPositions.transform.GetChild(i).position);
           


        }
       
    }
  

    private void Start()
    {
        TerrainStart();
        spawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder) , transform.position.y, transform.position.z);
        spawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder) , interactableSpawnRight.y, interactableSpawnRight.z);


    }






    public Bounds SpriteBoundsSum
    {
        
        get { return spriteBoundsSum; }
    }
    public abstract bool Validate();
    public abstract void TerrainStart();

    public abstract void ResetTerrain();
    public abstract void TerrainEnable();
    public Vector2 getGroundSize
    {
        get { return groundSize; }
    }

    public bool HasInteractables
    {
        get { return hasInteractables; } 
        set { hasInteractables = value; }
    
    
    
    } 

    public bool HasCollectibles
    {
        get { return hasCollectibles; }
        set { hasCollectibles = value; }    


    }

    public Vector3 SpawnLeft
    {
        get { return spawnLeft; } 
    
    }
    public Vector3 SpawnRight
    {
        get { return spawnRight; }

    }

    public bool HasDefferedInteractableSpawn
    {

        get { return hasDefferedInteactableSpawn; }
    }

    public TerrainClassifications GetClassification
    {
        get { return classification; }
    }
    public Vector3 getHalfScale
    {
        get { return groundSize / 2.0f; }
    }
    public List<TerrainClassifications> AdjacencyTerrainTypes
    {
        get { return validAdjacentTerrainTypes; }
    }
    public List<GameObject> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }
    public List<GameObject> Interactables
    {
        get { return interactables; }
        set { interactables = value; }
    }


    public int MinEnemies {

        get { return minEnemies;}
        set { minEnemies = value; }
    
    }
    public int MaxEnemies
    {

        get { return maxEnemies; }
        set { maxEnemies = value; }

    }


    public int CurrentEnemySpawnCount
    {
        get { return currentEnemiesCount; }
    }


    public TerrainClassifications NextTerrainOn
    {
        set { nextTerrainOn = value; } 
        get { return nextTerrainOn; }
    }

    public GameObject NextAdjacentTerrainTile
    {
        set { nextTerrainTyle = value; }
        get { return nextTerrainTyle; }
    }
    public bool HasSpawnPositions
    {

        get { return hasSpawnPositions; }
        set { hasSpawnPositions = value; }
    } 
    public TerrainType NextTerrainType
    {
        get { return nextTerrainType; }
        set { nextTerrainType = value; }
    }

    public bool IsConnected{

        get {  return isConnected; }
        set {  isConnected = value; }
    }
    public int getAdjacencyCount
    {
        get { return adjacencyCount; }
    }

}
