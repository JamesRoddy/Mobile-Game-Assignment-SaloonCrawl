using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBottle : EventObject
{
    Rigidbody2D bottle;
    float fBottleSpeed = 5.0f;
    float angle = 0.0f;
    float rotSpeed = 3.0f;
    int shotCount = 0;
   
    int shotCountMax = 3;
    int scoreIncrement = 40;
    bool shouldKill = true;
    Vector3 bottleVelocity;
    Vector3 rotationAxis = Vector3.forward;
    SpriteRenderer sprite;
    Camera playerCam;
    InstaniateScorePopUp scorePopUp;
    Collider2D col;
    
    public override void EventStart()
    {
     
        playerCam = FindFirstObjectByType<Camera>();
        sprite = GetComponent<SpriteRenderer>();
        bottle = GetComponent<Rigidbody2D>();
        bottleVelocity = -this.transform.right * fBottleSpeed;
        scorePopUp = GetComponent<InstaniateScorePopUp>(); 
        col = GetComponent<Collider2D>();
     
        
    }

    public override void EventObjEnable()
    {
        gameObject.SetActive(true);
        bottle.velocity = bottleVelocity;

    }
    public override void EventObjUpdate()
    {
       
        angle += rotSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle);
        deactivate();
    }



    public void deactivate()
    {

        if(shotCount >= shotCountMax || !isOnScreenRight())
        {
            if(shotCount >= shotCountMax)
            {
                scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
                playerController.conactToScore(Convert.ToString(scoreIncrement));

            }
            transform.rotation = Quaternion.identity; 
            angle = 0.0f;
            shotCount = 0;
     
            gameObject.SetActive(false);
        }
        


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag( "Player")  )
        {
           

            if (!collision.gameObject.GetComponent<playerController>().IsSliding )
            {
                collision.gameObject.GetComponent<DeathChecker>().isAlive = false;

                this.gameObject.SetActive(false);
            }
           
        }




        if (isOnScreen())
        {

            if (collision.gameObject.CompareTag("Bullet"))
            {
                shotCount++;

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
