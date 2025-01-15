using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainType : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 groundSize;

    [SerializeField] protected int adjacencyCount;
    protected Transform ground;
    [SerializeField] protected List<GameObject> interactables;
    [SerializeField] protected List<TerrainClassifications> validAdjacentTerrainTypes; 
    [SerializeField] protected TerrainClassifications classification;
    public abstract void randomizeInteractablePositions();

    
    public void setValues()
    {

        ground = GetComponent<Transform>();
        groundSize.x = ground.localScale.x;
        groundSize.y = ground.localScale.y;

    }
    private void Start()
    {
        TerrainStart();   
    }
    public abstract bool Validate();
    public abstract void TerrainStart();

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


    public int getAdjacencyCount
    {
        get { return adjacencyCount; }
    }
}
