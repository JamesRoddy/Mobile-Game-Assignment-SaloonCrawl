using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTerrainType : TerrainType
{

    private Vector3 enemySpawnPosition;


    public override void spawnInteractables(List<GameObject> interactables)
    {






    }

    public override void setSpawnPositions()
    {
        enemySpawnPosition = new Vector3(transform.position.x+transform.localScale.x/2.0f,transform.position.y,transform.position.z);   
    }



    public override void spawnEnemy(ref GameObject enemy)
    {
        
    }

    public override bool Validate()
    {
        Debug.Log("desert trigger condition hit " + true);


        return true;
    }
    public override void TerrainStart()
    {
       




    }

}
