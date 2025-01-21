using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] protected GameObject backGroundContainer;

/*    protected List<Vector3> interactableSpawnPositions = new List<Vector3>();
*/  private float interactablesSpawnDivder = 2.0f;
    private float interactablesSpawnPadding = 2.0f;
    
    private int currentMaxInteractables = 0;
    private int interactablesCounter = 0;
    protected bool hasInteractables = false;
    Vector3 interactableSpawnRight;
    Vector3 interactableSpawnLeft;
    Vector3 tempPositionForInteractable = Vector3.zero;


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

    
    public abstract void setSpawnPositions();
    public void activateObject(ref GameObject obj, Vector3 position) {

        obj.SetActive(true);
        obj.transform.position = position;

    }

    public abstract void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor);


    public void spawnInteractableObjects(InteractablePool pool)
    {
        Debug.Log("SPAWNING INTERACTABLES  spawning at generated  positions  current max interactables " + currentMaxInteractables);

       
        if (interactablesCounter < currentMaxInteractables )
        {
            Debug.Log("SPAWNING INTERACTABLES requesting object for " + classification + "current interactables count " + interactablesCounter +" current max "+currentMaxInteractables);
            GameObject interactable = pool.getRandomAvailableObject();
            if (pool.HasDeffered)
            {
                if (interactableSpawnPositions[interactablesCounter].transform.position.x < transform.position.x && tempPositionForInteractable == Vector3.zero)
                {
                    Debug.Log(" SPAWNING INTERACTABLES had to shift interactable spawn position to the right due to it being defferred");
                    tempPositionForInteractable = new Vector3(Random.Range(transform.position.x + transform.localScale.x / 4, interactableSpawnRight.x + 1.0f), interactableSpawnPositions[interactablesCounter].transform.position.y, interactableSpawnPositions[interactablesCounter].transform.position.z);

                }
                Debug.Log(" SPAWNING INTERACTABLES pool has deffered object waiting ... ");

                return;
            }
          
            interactable.SetActive(true);
            interactable.transform.position = interactableSpawnPositions[interactablesCounter].transform.position;
            interactablesCounter++;
           
            if (tempPositionForInteractable != Vector3.zero)
            {
                Debug.Log("SPAWNING INTERACTABLES temp pos for deffered pool object " + tempPositionForInteractable);
                interactable.transform.position = tempPositionForInteractable;
                tempPositionForInteractable = Vector3.zero;
            }

      

            
        }

        if(interactablesCounter == currentMaxInteractables || interactablesCounter == interactableSpawnPositions.Count )
        {
            Debug.LogWarning("if max interactables is great than number of spawnPositions " + interactablesCounter == interactableSpawnPositions.Count +" max interactables will never be hit if 1 is displayed ");
            Debug.Log(" SPAWNING INTERACTABLES interactable spanw count max hit for terrain " + classification);
            hasInteractables = true;

        }



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
        Debug.Log("assiging new spanw count " + currentEnemiesCount);

    }
  


    public float generateRandomX()
    {


        return Random.Range(spawnLeft.x, spawnRight.x + 1.0f);
    }


    public void setEndSpawnPositions()
    {


   
            spawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder) - interactablesSpawnPadding, transform.position.y, transform.position.z);
            spawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder) + interactablesSpawnPadding, interactableSpawnRight.y, interactableSpawnRight.z);


     }
    public void generatePositionsInteractables()
    {
        hasInteractables = false;
        interactableSpawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder)  - interactablesSpawnPadding, transform.position.y, transform.position.z);
        interactableSpawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder) + interactablesSpawnPadding, interactableSpawnRight.y, interactableSpawnRight.z);
        interactablesCounter = 0;
        currentMaxInteractables = Random.Range(minInteractables, maxInteractables+1);
        
        Debug.Log("SPAWNING INTERACTABLES generating spawn positions  max spawn loc left "+interactableSpawnLeft+"max spawn loc right "+interactableSpawnRight+" has interactables "+hasInteractables + "current max " + currentMaxInteractables);

        for(int i = 0; i< interactableSpawnPositions.Count; i++)
        {
            int randomShuffle = Random.Range(0, interactableSpawnPositions.Count);
            Debug.Log("SPAWNING INTERACTABLES random shuffle base " + randomShuffle +"interactable spawn positions count "+interactableSpawnPositions.Count);
            randomShuffle =  randomShuffle == i && i < interactableSpawnPositions.Count / 2 ?   Random.Range(0, i): Random.Range(interactableSpawnPositions.Count / 2, interactableSpawnPositions.Count);
            Debug.Log("SPAWNING INTERACTABLES random after adjustement  " + randomShuffle +"was equal to current pos "+(randomShuffle==i));

            GameObject temp = interactableSpawnPositions[i];
            Debug.Log(" SPAWNING INTERACTABLES temp transform position  before swap" + temp.transform.position);
            Debug.Log(" SPAWNING INTERACTABLES random chose before swap " + interactableSpawnPositions[randomShuffle].transform.position);
            interactableSpawnPositions[i] = interactableSpawnPositions[randomShuffle]; 
            
            interactableSpawnPositions[randomShuffle] = temp;
            Debug.Log("SPAWNING INTERACTABLES temp transform position  after swap" + interactableSpawnPositions[i].transform.position);
            Debug.Log(" SPAWNING INTERACTABLES random chose after swap " + interactableSpawnPositions[randomShuffle].transform.position);



        }
/*        float previous = transform.position.x;*/
       /* for (int i = 0; i < currentMaxInteractables; i++)
        {

            float randomX = UnityEngine.Random.Range(interactableSpawnLeft.x, interactableSpawnRight.x + 1.0f);

            if ((previous < transform.position.x && randomX < transform.position.x) || (previous > transform.position.x && randomX > transform.position.x))
            {
                float side = previous < 0.0f ? interactableSpawnLeft.x : interactableSpawnRight.x;
                randomX = UnityEngine.Random.Range(transform.position.x, side);

            }
            previous = randomX;
            Vector3 spawnPositions = new Vector3(randomX, transform.position.y, transform.position.z);

            interactableSpawnPositions.Add(spawnPositions);


        }*/
    }
  
    
    private void Start()
    {
        TerrainStart();
        spawnRight = new Vector3((transform.position.x + transform.localScale.x / interactablesSpawnDivder) - interactablesSpawnPadding, transform.position.y, transform.position.z);
        spawnLeft = new Vector3((transform.position.x - transform.localScale.x / interactablesSpawnDivder) + interactablesSpawnPadding, interactableSpawnRight.y, interactableSpawnRight.z);
        float test = interactableSpawnPositions[interactableSpawnPositions.Count-1].transform.position.x;
        Debug.Log("interactable spanw pos test "+ test ) ;
        
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
