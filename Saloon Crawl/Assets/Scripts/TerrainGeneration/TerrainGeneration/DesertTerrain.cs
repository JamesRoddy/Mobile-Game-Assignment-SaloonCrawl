using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTerrainType : TerrainType
{


    public override void randomizeInteractablePositions()
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
