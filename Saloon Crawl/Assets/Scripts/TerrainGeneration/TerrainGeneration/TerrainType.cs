using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainType : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 groundSize;
    private bool isConnected = false;
    [SerializeField] protected int adjacencyCount;
    protected Transform ground;
    [SerializeField] protected List<GameObject> interactables;
    [SerializeField] protected List<GameObject> enemies;
    [SerializeField] protected List<TerrainClassifications> validAdjacentTerrainTypes;
    [SerializeField] protected TerrainClassifications classification;
    [SerializeField] protected int minEnemies;
    [SerializeField] protected int maxEnemies; 
   
    protected  bool hasSpawnPositions = false;
    protected int currentEnemiesCount;
    protected int currentSpawnCount = 0;
    
    private TerrainClassifications nextTerrainOn;
    private GameObject nextTerrainTyle;
    private TerrainType nextTerrainType;
    public abstract void spawnInteractables(List<GameObject> interactables);
    public abstract void setSpawnPositions();
    public void activateObject(ref GameObject obj, Vector3 position) { 
    
       obj.SetActive(true); 
       obj.transform.position = position;
    
    }

    public abstract void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor);

    public void setValues()
    {
        currentEnemiesCount = minEnemies;
        ground = GetComponent<Transform>();
        groundSize.x = ground.localScale.x;
        groundSize.y = ground.localScale.y;

    }
    private void Start()
    {
        TerrainStart();
    } 

    public void genEnemyNum()
    {
        currentEnemiesCount = Random.Range(minEnemies, maxEnemies +1);
        Debug.Log(" SPAWNING ENEMY random enemy number  generated for  " + classification+" number " + currentEnemiesCount);
    }
    public abstract bool Validate();
    public abstract void TerrainStart();

    public abstract void ResetTerrain();
    public abstract void TerrainEnable();
    public Vector2 getGroundSize
    {
        get { return groundSize; }
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
