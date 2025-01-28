using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottleEvent : TerrainEvent
{


    private GameObject beerBottle;
    private EventObject beerBottleEventScript;
    private Collider2D bottleCol; 
    private float boundsScalarMax = 1.0f;



    public override void EventStart()
    {
        


        if (objectPool.hasAvailableObject())
        {
            beerBottle = objectPool.requestAvaialbeObject();
          
        }
 

        beerBottleEventScript = beerBottle.GetComponent<EventObject>();
        bottleCol = beerBottle.GetComponent<Collider2D>();
    
        beerBottleEventScript.EventStart();

      



    }
    public override void EventEnable()
    {

        if (objectPool.hasAvailableObject())
        {
            beerBottle = objectPool.requestAvaialbeObject();
            
        }
        beerBottleEventScript.EventObjEnable();



        float offsetPercent = Random.Range(0.0f, boundsScalarMax);

        float randomY = playerController.transform.position.y + (playerCol.bounds.size.y /2.0f) + ((playerCol.bounds.size.y/2.0f)  * offsetPercent);
        Vector3 beerBottelPos = new Vector3(playerController.CurrentTerrain.SpawnRight.x + playerController.CurrentTerrain.transform.localScale.x, randomY, playerController.transform.position.z);
        beerBottle.transform.position = beerBottelPos;
        isFiring = true;
    }
    public override void Fire() {


        if (playerController.IsViewingNextTerrain && playerController.CurrentTerrain.containsPoint(beerBottleEventScript.transform.position)) // if the player has not started viewing the next terrain menaing that they cant see their current 
        {
            return;
        }

  


    }

    public override bool HasFinished() {

        if(beerBottle.activeSelf == false)
        {
            isFiring = false;
            return true;
        }


        return false;    
    }

  

}
