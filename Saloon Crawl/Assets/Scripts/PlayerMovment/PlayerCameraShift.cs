using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerCameraShift : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera playerCam;
    private CamMovement playerCamMover;
    private playerController playerController;
    private TouchControls touchControls;
    private float lerpShiftSmoothing = 0.85f;
    private float lerpDragSmoothing = 1.5f;
    private float orthoSizeWhenViewing = 4.0f;
    private float OrothoSizeWhenViewingMin = 1.0f;
    private bool hasCentredOnTerrain = false;
    Vector2 screenBorderOffset = Vector2.zero;
    private bool isShiftingBack = false;
    void Start()
    {
        
        playerCamMover = GetComponent<CamMovement>();
        playerCam = GetComponent<Camera>();
        touchControls  = FindFirstObjectByType<TouchControls>();
        playerController = FindFirstObjectByType  <playerController > ();
        screenBorderOffset.x = playerCam.scaledPixelWidth / 2.0f;
        screenBorderOffset.y = playerCam.scaledPixelHeight / 2.0f;

    }

    // Update is called once per frame
    void Update()
    {

        checkShouldShiftBack();
        hasReachedCentreOfTerrain();
        resolveOverlap();


    }


   
    private void checkShouldShiftBack()
    {
        
        if (hasCentredOnTerrain && touchControls.accelerationHasHitNegative())
        {
           
             isShiftingBack = true;
        } 

        if(isShiftingBack && playerCamMover.shiftToPlayerPosition())
        {
            
            playerController.IsViewingNextTerrain = false;
            playerCamMover.FollowPlayer = true;
            isShiftingBack = false;
            hasCentredOnTerrain = false;
            playerCamMover.resetOrtho();
            return;
        }
    

    }


    private void resolveOverlap()
    {

        // if the camera has  centred on the terrain 
        if (hasCentredOnTerrain && !isShiftingBack)
        {


            Vector2 deltaPos = touchControls.getDragPos(); // allows the player to inspect the next terrain by moving the camera with a single finger 

            
            // keep camera within the bounds of the terrain using the background sprites as a guide 
            Vector3 direction = Vector2.zero;
            direction.x = transform.position.x < playerController.CurrentTerrain.NextTerrainType.transform.position.x ? -1.0f : 1.0f;

            direction.y = transform.position.y < playerController.CurrentTerrain.NextTerrainType.transform.position.y ? -1.0f : 1.0f;
            Vector2 screenOffset = new Vector2(playerCamMover.HalfRect.x * direction.x, playerCamMover.HalfRect.y * direction.y);

            Vector3 nextPos = playerCam.ScreenToWorldPoint(playerCam.WorldToScreenPoint((transform.position + (Vector3)deltaPos * Time.deltaTime)) + (Vector3)screenOffset);
            Vector3 overlap = Vector3.zero;
            if (!playerController.CurrentTerrain.NextTerrainType.SpriteBoundsSum.Contains(new Vector3(nextPos.x, nextPos.y, 0.0f)))
            {
              
                Bounds resolve = playerController.CurrentTerrain.NextTerrainType.SpriteBoundsSum;
                Vector3 resolution = nextPos - resolve.ClosestPoint(nextPos);
                overlap = new Vector3(resolution.x, resolution.y, 0.0f);

            }


            transform.position = Vector3.Lerp(transform.position, (transform.position + (Vector3)deltaPos * Time.deltaTime) + -overlap, lerpDragSmoothing); // lerp the camera postion to make movement more smooth along with accounting for bounds checking 

            touchControls.zoomPLayerCamera();
        }

    }
    private void hasReachedCentreOfTerrain()
    {
       if(playerController.IsViewingNextTerrain && !hasCentredOnTerrain && !isShiftingBack)
        {

            playerCamMover.shrinkOrthoSize(orthoSizeWhenViewing);
            hasCentredOnTerrain = playerCamMover.shiftToPosition(playerController.NextTerrainCamPos, lerpShiftSmoothing);
            
        }
    
        

       
    }

    public float CamShiftOrthoMin
    {
        get { return OrothoSizeWhenViewingMin; }
    }
    public float CamShiftOrthoMax
    {
        get { return orthoSizeWhenViewing; }
    }

    public bool IsShiftingBack
    {

        get { return isShiftingBack; }




    }


}
