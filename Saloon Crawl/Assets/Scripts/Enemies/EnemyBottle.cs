using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBottle : EventObject
{
    Rigidbody2D bottle;
    float fBottleSpeed = 5.0f;
    float angle = 0.0f;
    float rotSpeed = 7.0f;
    int shotCount = 0;
    int shotCountCurrentLim = 0;
    int shotCountMin = 2; 
    int shotCountMax = 4; 

    Vector3 bottleVelocity;
    Vector3 rotationAxis = Vector3.forward;
 
   

    // Start is called before the first frame update
    public override void EventStart()
    {
        bottle = GetComponent<Rigidbody2D>();
        bottleVelocity = -this.transform.right * fBottleSpeed;
        playerCam = FindFirstObjectByType<Camera>(); 
        
    }

    public override void EventObjEnable()
    {
        gameObject.SetActive(true);
        shotCount =  UnityEngine.Random.Range(shotCountMin, shotCountMax + 1);


    }
    // Update is called once per frame
    public override void EventObjUpdate()
    {
        bottle.velocity = bottleVelocity;
        angle += rotSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle);
        deactivate();
    }



    public void deactivate()
    {

        if(shotCount >= shotCountCurrentLim || !isOnScreenRight())
        {
            gameObject.SetActive(false);
        }
        


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag( "Player"))
        {
            collision.gameObject.GetComponent<DeathChecker>().isAlive = false;
            this.gameObject.SetActive(false);
        }



        if (isOnScreen())
        {

            if (collision.gameObject.CompareTag("Bullet"))
            {
                shotCount++;

            }
             
            if (collision.gameObject.CompareTag("Kickable"))
            {
                collision.gameObject.GetComponent<Kick>().Kicked = true;


            }


        }







    }



    private bool isOnScreenRight()
    {
        float screenXViewport = playerCam.WorldToViewportPoint(transform.position).x;

        return screenXViewport > 0.0f;
    }
    private bool isOnScreen()
    {

        float screenXViewport = playerCam.WorldToViewportPoint(transform.position).x;

        return screenXViewport > 0.0f && screenXViewport <= 1.0f;





    }
}
