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
    private float lerpShiftSmoothing = 0.2f;
    private float lerpDragSmoothing = 0.85f;
    private float orthoSizeWhenViewing = 4.0f;
    private bool hasCentredOnTerrain = false;
    Vector2 screenBorderOffset = Vector2.zero; 
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
        hasReachedCentreOfTerrain();
        if (hasCentredOnTerrain)
        {
           
            Vector2 deltaPos = touchControls.getDragPos(); 

      

            Debug.Log("CAM SHIFTING  dragging " + transform.position);
            Vector2 screenOffset = new Vector2(playerCamMover.HalfRect.x , playerCamMover.HalfRect.y);
            Vector3 nextPos = playerCam.ScreenToWorldPoint(  playerCam.WorldToScreenPoint( (transform.position + (Vector3)deltaPos * Time.deltaTime ) ) + (Vector3) screenOffset  ) ;
          /*  Debug.Log("  CAM SHIFTING screen pixel rect offset" +screenOffset +"current pos "+nextPos);*/
            Vector3 overlap = Vector3.zero;
            if (!playerController.CurrentTerrain.NextTerrainType.SpriteBoundsSum.Contains(new Vector3(nextPos.x,nextPos.y,0.0f))) 
            {
                Debug.Log("CAMERA SHIFTING next position that was overlaping " + nextPos);
                Bounds resolve = playerController.CurrentTerrain.NextTerrainType.SpriteBoundsSum;
            
                overlap =   nextPos -  resolve.ClosestPoint(nextPos);
                Debug.Log("CAMERA SHIFTING resolving overlap "+overlap);
             
            }


            transform.position = Vector3.Lerp( transform.position, (transform.position + (Vector3)deltaPos * Time.deltaTime)+ -overlap , lerpDragSmoothing);

/*            transform.position += -overlap;
*/
         

        }


    }

    
    private void hasReachedCentreOfTerrain()
    {
       if(playerController.IsViewingNextTerrain && !hasCentredOnTerrain)
        {

            playerCamMover.shrinkOrthoSize(orthoSizeWhenViewing);
            hasCentredOnTerrain = playerCamMover.shiftToPosition(playerController.NextTerrainCamPos, lerpShiftSmoothing);
            
        }
    
        

       
    }


    

    

    



}
