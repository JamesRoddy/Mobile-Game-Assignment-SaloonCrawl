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

    void Start()
    {


        player = FindObjectOfType<playerController>();
     
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
            Vector2 touchPos = touch.position;
            checkSwipe(touch);



        }

    }


    void checkSwipe(Touch touch)
    {


   
        direction = touch.position - touchInitialPos;




        if (touch.phase == TouchPhase.Ended && direction.y > 0.0f && player.CanJump)
        {
            
            player.ShouldJump = true;
            direction = Vector2.zero;
        }




    }
}
