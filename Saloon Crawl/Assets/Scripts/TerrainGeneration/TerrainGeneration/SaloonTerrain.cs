using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SallonTerrainType : TerrainType
{

    Vector3 previousTransFormPosition;
    playerController player = null;
    float baseSaloonDist = 65.0f;
    public override bool Validate()
    {
        Debug.Log("saloon trigger condition hit " + (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist));
        return (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist);

    }
    public override void randomizeInteractablePositions()
    {

    }

    public override void TerrainStart()
    {
        previousTransFormPosition = transform.position;
    
       
        if(player == null)
        {
            player = FindObjectOfType<playerController>();
       
        }
    }


}
