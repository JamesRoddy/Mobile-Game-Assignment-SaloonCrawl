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
    // Start is called before the first frame update
    private playerController player;
    private Vector2 direction;
    private Vector2 touchInitialPos = Vector2.zero;
    bool bSwiping = false;

    void Start()
    {
        player = FindObjectOfType<playerController>();
        Debug.Log("start");
    }

    // Update is called once per frame
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
            Vector2 touchPos = touch.position;
            checkSwipe(touch);

            if(!bSwiping)
            {
                checkTap(touch, phase);
            }
            
        }
        
    }

    void checkTap(Touch touch, TouchPhase phase)
    {
        if(phase == TouchPhase.Ended)
        {
            Debug.Log("Tapping");
            player.shouldShoot = true;
        }
    }


    void checkSwipe(Touch touch)
    {


        Debug.Log("touch moving");
        direction = touch.position - touchInitialPos;
        Debug.Log(direction + " swipe dir : initial pos " + touchInitialPos + " touch position " + touch.position);



        if (touch.phase == TouchPhase.Ended && direction.y > 0.0f && player.CanJump)
        {
            Debug.Log("ended");
            player.ShouldJump = true;
            direction = Vector2.zero;
            bSwiping = true ;
        }

        else
        {
            bSwiping= false ;
        }




    }
}
