using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottleEvent : TerrainEvent
{


    private GameObject beerBottle;
    private EventObject beerBottleEventScript;
    private playerController player;
    private float boundsScalarMax = 1.0f;
    private float boundPercentMin = 4.0f;
    public override void EventStart()
    {


        player = FindFirstObjectByType<playerController>();

        if (objectPool.hasAvailableObject())
        {
            beerBottle = objectPool.requestAvaialbeObject();
        }
       

        Debug.Log("EVENT beer bottle was null " + (beerBottle == null));

        beerBottle.SetActive(true);
        beerBottleEventScript = beerBottle.GetComponent<EventObject>();
        beerBottleEventScript.EventObjEnable();
        beerBottleEventScript.EventStart();
        Collider2D bottleCol = beerBottle.GetComponent<Collider2D>();
        float offsetPercent = Random.Range(0.0f, boundsScalarMax);
/*        bottleCol.bounds.size.y* offsetPercent
*/      float randomY = player.transform.position.y +( bottleCol.bounds.size.y/boundPercentMin +(bottleCol.bounds.size.y*offsetPercent));
        Vector3 beerBottelPos = new Vector3(player.CurrentTerrain.SpawnRight.x + player.CurrentTerrain.transform.localScale.x , randomY, player.transform.position.z);
        beerBottle.transform.position = beerBottelPos;
        Debug.Log(" EVENT  beer bottle event set up new position "+ beerBottle.transform.position +" percentage of bounds added " +offsetPercent );
        isFiring = true;


    }

    public override void Fire() {

        beerBottleEventScript.EventObjUpdate();


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
