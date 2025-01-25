using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CoinCollectble : Collectible
{
    // Start is called before the first frame update
     
   
    public override void CollectibleStart()
    {
         
    }
    // Update is called once per frame

    public override void CollectibleUpdate()
    {
        
    }
    public override void interact()
    {


        playerController.CurrentCoinCount++; 
        Debug.Log("COIN COLLECTIBLE player coin count increased "+playerController.CurrentCoinCount);
        GetComponent<SpriteRenderer>().enabled = false;
        
       


        if (!sound.isPlaying)
        {
            startInteraction = false;
            gameObject.SetActive(false);
        }
       




    
    }




}
