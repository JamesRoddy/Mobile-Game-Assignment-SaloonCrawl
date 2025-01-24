using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
   
    private playerController player;
    private Vector2 direction;
    private Vector2 touchInitialPos = Vector2.zero;
    bool bSwiping = false;
    public Vector2 touchPos;
    public bool bSwipeRight = false;
    private bool bSwipeLeft = false;
    private float dragDistance = 0.0f;
    float t;
    float directionYThreshHold = 100.0f;
    float directionXThreshHold = 150.0f;
    private float swipeHorizontalPercent = 0.4f;
    private float swipeVerticalPercent = 0.5f;
    private Camera playerCam;
    void Start()
    {
        player = FindObjectOfType<playerController>();
        Debug.Log("start");
        playerCam = Camera.main;

        swipeHorizontalPercent *= playerCam.scaledPixelWidth;
        swipeVerticalPercent *= playerCam.scaledPixelHeight;
    }

    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchPhase phase = touch.phase;
            if (phase == TouchPhase.Began)
            {

                touchInitialPos = touch.position;
            }
            touchPos = touch.position;
            checkSwipe(touch);
            if (!bSwiping)
            {
                checkTap(touch, phase);
            }
            
        }

        if (bSwipeRight)
        {
            
            t += Time.deltaTime;
            if (t > 0.5f)
            {
                Debug.Log("Stopping swipe");
                bSwipeRight = false;

                Debug.Log("Stopping Anim");
                player.CowboyAnim.SetBool("Kick", false);
                t = 0f;
            }

        }

    }

    void checkTap(Touch touch, TouchPhase phase)
    {
        if(phase == TouchPhase.Ended)
        {
            player.CowboyAnim.SetBool("Kick", false);
            player.shouldShoot = true;

            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            //Debug.Log("touchPos" + touchPos);

            Vector2 store = Camera.main.ScreenToWorldPoint(touch.position) - player.bulletSpawnPoint.position; 

            player.fBulletAngle = Mathf.Atan2( store.y, store.x) * Mathf.Rad2Deg;
           // Debug.Log("Bullet Angle:" + player.fBulletAngle);
        }
    }

    public Vector2 getDragPos()
    {
        if( Input.touchCount == 1 &&  Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            return Input.GetTouch(0).deltaPosition;


        }

        return Vector2.zero;


    }

    void checkSwipe(Touch touch)
    {



        //Debug.Log("touch moving");

        direction = touch.position - touchInitialPos;
        Debug.Log("direction difference x" + direction.x + "direction difference y" + direction.y +"swipe hori percent"+swipeHorizontalPercent+"swipe vertcial "+swipeVerticalPercent);



        if (touch.phase == TouchPhase.Ended && direction.y > swipeVerticalPercent && player.Grounded)
        {

            //Debug.Log("ended");
            Debug.Log("Swipe up");
            player.ShouldJump = true;
            direction = Vector2.zero;
            bSwiping = true ;
        }

        else if(touch.phase == TouchPhase.Ended && direction.x > swipeHorizontalPercent)
        {
            player.CowboyAnim.SetBool("Kick", true);
            player.kickSound.Play();
            direction = Vector2.zero;
            bSwiping = true;
            bSwipeRight = true;
            Debug.Log("Swiping Right" + bSwipeRight);
           
        }

        else if(touch.phase == TouchPhase.Ended && direction.y < -swipeVerticalPercent)
        {
            player.shouldSlide = true;

            direction = Vector2.zero;
            bSwiping = true;
            
        }
        else if (touch.phase == TouchPhase.Ended && direction.x < -swipeHorizontalPercent)
        {
            bSwipeLeft = true;
            direction = Vector2.zero;
            Debug.Log("Swiping Left" + bSwipeLeft);
            bSwiping = true;

        }

        else
        {
            bSwipeLeft =false;
            bSwiping = false ;
            bSwipeRight = false ;
        }




    }

    
    public  void assignDrag(Touch touch)
    {
        if(  Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
        {
             dragDistance  = Vector3.Distance( touch.position , touchInitialPos); 
           


        }
        dragDistance = 0;
      


    }


    public Vector2 getTouchPos()
    {
        return touchPos;
    }



    public bool SwipeLeft
    {
        get { return bSwipeLeft; } 

    }

}
