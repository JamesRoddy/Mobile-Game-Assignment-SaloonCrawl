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
    private int bSwipeLeft  = 0;
    private int bSwipeUp =   0;
    private bool bSwipeDown = false;
    
    private int swipeingDown = 0;
    private int swipingDown = 0;
    private int swipingRight = 0;
    private int accelRight  =0;
    private int accelLeft =  0;
    private int tap = 0;
    private float dragDistance = 0.0f;
    float t;
  
   


    private float swipeHorizontalPercent = 0.2f;
    private float swipeRightPercent = 0.05f; // used to make it so that the amount the player has to swiipe relates to their screen size 
    private float swipeVerticalPercent = 0.1f;
    private Camera playerCam;
    private float accelMoveX; // vairbale to store accelerometer X

    private float accelthresh = 0.5f; // define thresh hold that the accelerometer must reach for input 
    private float zoomSpeed = 0.1f; // define the zoom speed for multi touch zoom
    private int zooming = 0;
    private int dragging = 0;

   
   public  Dictionary<string, int> inputDictionary = new Dictionary<string, int>();

       
    void Start()
    {
        player = FindObjectOfType<playerController>();
        playerCam = Camera.main; 
       
        // define how much the player must swipe for input based on their screen size 
        swipeHorizontalPercent *= Screen.width;
        swipeVerticalPercent *= Screen.height;
        swipeRightPercent *= Screen.width;


        inputDictionary = new Dictionary<string,  int>
        {
            {"swipeRight", 0 },
            {"swipeDown",0 },
            {"swipeUp",0 },
            {"accelRight",0 },
            {"tap",0 },
            {"accelLeft",0 },
            {"dragged", 0 },
            {"zoomed",0 },
           

        };


    }

    
    void Update()
    {

        accelMoveX = Input.acceleration.x ; // get the current acceleration of the accelerometer of the device  in x 
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
                bSwipeRight = false;

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
            inputDictionary["tap"] = 1;
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            player.touchStore = touchPos;

      

        
        }
    }

    // main method for allowing the player to  zoom the camera when viewing the next terrain 
    public  void zoomPLayerCamera ()
    {

        if(Input.touchCount == 2) // if we have multiple touches 
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            inputDictionary["zoom"] = 1;
            Vector2 deltaDifference0 = touch0.position - touch0.deltaPosition; // get the difference between the delta position(difference between last update and current) and subtarct from current
            Vector2 deltaDifference1 = touch1.position - touch1.deltaPosition;
            float prevPositionDifference = (deltaDifference0 - deltaDifference1).magnitude; // get the maginutde/distance between the position on last 

            float currentPosition = (touch0.position - touch1.position).magnitude;// get current difference 

            float magDiff = prevPositionDifference - currentPosition; // get the difference between current and last

            if (playerCam.orthographic) // if the camera has an orthographic compoenent 
            {
                playerCam.orthographicSize += magDiff * zoomSpeed; // add on to the ortho size scaling and shrikning the camera by the difference between the last differencec between the two touches and the current 
                PlayerCameraShift cameraShift= playerCam.GetComponent<PlayerCameraShift>();
                playerCam.orthographicSize = Mathf.Clamp(playerCam.orthographicSize, cameraShift.CamShiftOrthoMin, cameraShift.CamShiftOrthoMax); // ensure to clamp the size so the zoom doesnt go to far out or in 
            }

        }


    }

    public Vector2 getDragPos()
    {
        if( Input.touchCount == 1 &&  Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            inputDictionary["dragging"] = 1;
            return Input.GetTouch(0).deltaPosition; // return the delta position of the current touch when dragging 


        }

        return Vector2.zero;


    }

    // check swipe directions 
    void checkSwipe(Touch touch)
    {




        direction = touch.position - touchInitialPos;

        if (touch.phase == TouchPhase.Ended && direction.y > swipeVerticalPercent && player.Grounded)
        {
            inputDictionary["swipeUp"] = 1;
            player.ShouldJump = true;
            direction = Vector2.zero;
            bSwiping = true ;

        }

        else if(touch.phase == TouchPhase.Ended && direction.x > swipeRightPercent)
        {
            player.CowboyAnim.SetBool("Kick", true);
            player.kickSound.Play();
            direction = Vector2.zero;
            inputDictionary["swipeRight"] = 1;
            Debug.Log("swiping right" + swipingRight);
            bSwiping = true;
            bSwipeRight = true;
           
        }

        else if(touch.phase == TouchPhase.Ended && direction.y < -swipeVerticalPercent)
        {
            player.shouldSlide = true;
            bSwipeDown = true;
            direction = Vector2.zero;
            inputDictionary["swipeDown"] = 1;

            bSwiping = true;
            swipingDown = 1;
        }
      

        else
        {
            bSwipeUp = 0;
            bSwipeLeft =0; 
            swipeingDown = 0;
           
            bSwiping = false ;
            bSwipeRight = false ;
        }




    }

    // getters to return when the accelerometer hits a particualr thesh hold 
    public bool accelerationHasHitNegative()
    {
        inputDictionary["accelLeft"] = Convert.ToInt32(accelMoveX < -accelthresh);
        return accelMoveX < -accelthresh; // if the accelX is smaller than the negated accelThresh 
    }
    public bool accelerationHasHitPositve()
    {
        inputDictionary["accelRight"] = Convert.ToInt32( accelMoveX < -accelthresh);
        return accelMoveX > accelthresh;// if the accelX is smaller than the  accelThresh 
    }
    public Vector2 getTouchPos()
    {
        return touchPos;
    }


    public float AccelMoveX
    {

        get { return accelMoveX; } 
    }
   

}
