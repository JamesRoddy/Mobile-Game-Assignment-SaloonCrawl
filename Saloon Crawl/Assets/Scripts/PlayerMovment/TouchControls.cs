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
    public float t;

    private Animator CowboyAnim;

    void Start()
    {
        player = FindObjectOfType<playerController>();

        CowboyAnim = GetComponent<Animator>();

        Debug.Log("start");

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
            CowboyAnim.SetBool("Kick", true);
            t += Time.deltaTime;
            Debug.Log(t);
            if (t > 0.5f)
            {
                Debug.Log("Stopping swipe");
                bSwipeRight = false;

                Debug.Log("Stopping Anim");
                CowboyAnim.SetBool("Kick", false);
                t = 0f;
            }

        }

    }

    void checkTap(Touch touch, TouchPhase phase)
    {
        if(phase == TouchPhase.Ended)
        {
            //Debug.Log("Tapping");
            player.shouldShoot = true;

            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            //Debug.Log("touchPos" + touchPos);

            Vector2 store = Camera.main.ScreenToWorldPoint(touch.position) - player.bulletSpawnPoint.position; 

            player.fBulletAngle = Mathf.Atan2( store.y, store.x) * Mathf.Rad2Deg;
           // Debug.Log("Bullet Angle:" + player.fBulletAngle);
        }
    }


    void checkSwipe(Touch touch)
    {



        //Debug.Log("touch moving");

        direction = touch.position - touchInitialPos;
        



        if (touch.phase == TouchPhase.Ended && direction.y > 100.0f && player.CanJump)
        {

            //Debug.Log("ended");
            Debug.Log("Swipe up");
            player.ShouldJump = true;
            direction = Vector2.zero;
            bSwiping = true ;
        }

        else if(touch.phase == TouchPhase.Ended && direction.x > 150.0f)
        {
            //Debug.Log("Swiping Right");
            direction = Vector2.zero;
            bSwiping = true;
            Debug.Log("Kick is" + CowboyAnim.GetBool("Kick"));
            bSwipeRight = true;
            Debug.Log("Swiping Right" + bSwipeRight);
           
        }

        else if(touch.phase == TouchPhase.Ended && direction.y < 0.0f)
        {
            player.shouldSlide = true;
            direction = Vector2.zero;
            bSwiping = true;
            
        }

        else
        {
            bSwiping = false ;
            bSwipeRight = false ;
        }




    }

    public Vector2 getTouchPos()
    {
        return touchPos;
    }
}
