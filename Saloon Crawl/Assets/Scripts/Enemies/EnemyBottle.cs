using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBottle : EventObject
{
    Rigidbody2D bottle;
    float fBottleSpeed = 3.0f;
    float angle = 0.0f;
    float rotSpeed = 3.0f;
    int shotCount = 0;
    int shotCountCurrentLim = 0;
    int shotCountMin = 2; 
    int shotCountMax = 4;
    int scoreIncrement = 40;
    bool shouldKill = true;
    Vector3 bottleVelocity;
    Vector3 rotationAxis = Vector3.forward;
    SpriteRenderer sprite;
    Camera playerCam;
    InstaniateScorePopUp scorePopUp;
    Collider2D col;
    // Start is called before the first frame update
    
    public override void EventStart()
    {
     
        playerCam = FindFirstObjectByType<Camera>();
        sprite = GetComponent<SpriteRenderer>();
        bottle = GetComponent<Rigidbody2D>();
        bottleVelocity = -this.transform.right * fBottleSpeed;
        scorePopUp = GetComponent<InstaniateScorePopUp>(); 
        col = GetComponent<Collider2D>();
     
        Debug.Log("EVENT beer bottle start");
        
    }

    public override void EventObjEnable()
    {
        gameObject.SetActive(true);
        bottle.velocity = bottleVelocity;
        shotCountCurrentLim =  UnityEngine.Random.Range(shotCountMin, shotCountMax + 1);
        Debug.Log("EVENT beer bottle enable");

    }
    // Update is called once per frame
    public override void EventObjUpdate()
    {
       
        angle += rotSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle);
        deactivate();
    }



    public void deactivate()
    {

        if(shotCount >= shotCountCurrentLim || !isOnScreenRight())
        {
            if(shotCount >= shotCountCurrentLim)
            {
                scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
                playerController.conactToScore(Convert.ToString(scoreIncrement));

            }
            transform.rotation = Quaternion.identity; 
            angle = 0.0f;
            shotCount = 0;
            Debug.Log("EVENT beer bottle deactivate");
            gameObject.SetActive(false);
        }
        


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag( "Player")  )
        {
            if(collision.gameObject.transform.position.x > transform.position.x + col.bounds.size.x/4.0f )
            {
                shouldKill = false;
            }

            if (!collision.gameObject.GetComponent<playerController>().IsSliding && shouldKill)
            {
                collision.gameObject.GetComponent<DeathChecker>().isAlive = false;
                Debug.Log("EVENT beer bottle collided with player");

                this.gameObject.SetActive(false);
            }
           
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EVENT beer bottle collided with enemy");
            if(collision.gameObject.GetComponent<EnemyDescriptorInfo>().GetType() == typeof(Bandit))
            {
                Debug.Log("EVENT BEER BOTTLE collided with bandit getting bandit death checker  ");
                collision.gameObject.GetComponent<DeathChecker>().isAlive=false;
            }


        }


        if (isOnScreen())
        {

            if (collision.gameObject.CompareTag("Bullet"))
            {
                shotCount++;

            }
             
          /*  if (collision.gameObject.CompareTag("Kickable"))
            {
                collision.gameObject.GetComponent<Kick>().Kicked = true;
                Debug.Log("EVENT beer bottle collided with kickable");


            }*/


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
