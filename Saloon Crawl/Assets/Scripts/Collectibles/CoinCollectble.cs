using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CoinCollectble : Collectible
{
    // Start is called before the first frame update
   
    public override void CollectibleStart()
    {
 


    }
    // Update is called once per frames

    public override void CollectibleUpdate()
    {
       
    }

    public override void increaseStat()
    {
        
    }
    public override void interact()
    {
        playerController.CurrentCoinCount++;
       
        collectibleSound.Play();
        
      

        Debug.Log("COIN COLLECTIBLE player coin count increased "+playerController.CurrentCoinCount);
        if (!collectibleSound.isPlaying)
        {
            gameObject.SetActive(false);
        }

    }




}
