using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    private float offsetX = 5.0f;
    private Camera cam;
    private float offsetSmooth = 1.5f;
    private bool followPLayer = true;
    private playerController player;
    private float  shiftingBackSmooth = 0.85f;
    private PlayerCameraShift playerCamShift;
    private TerrainManager terrainManager;

    Vector3 offsetPos = Vector3.zero;
    private float orthoSizeDefault = 5.0f;
    private float camSizeDecrease = 0.01f;




    private DeathChecker deathChecker;
    private float deathZoom = 2;


    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = orthoSizeDefault;
        terrainManager = FindFirstObjectByType<TerrainManager>();
        playerCamShift = FindFirstObjectByType<PlayerCameraShift>();

        player = FindObjectOfType<playerController>();

        deathChecker = player.GetComponent<DeathChecker>();
        

    }






    public bool canSeePosition(Vector3 position)
    {
        float x = cam.WorldToViewportPoint(position).x;

        return x >= 0.0f && x <= 1.0f;



    }

    void Update()
    {
        followPLayerPosition();
        checkDeath();
    }




    public bool playerCanSeeEndWithoutNextTerrain()
    {


        float end = cam.WorldToViewportPoint(player.CurrentTerrain.SpawnRight).x;

        return player.CurrentTerrain.NextTerrainType == null && end > 0.0f && end <= 1.0f;


    }

    public void shrinkOrthoSize(float size)
    {

        if (cam.orthographicSize > size)
        {
            cam.orthographicSize -= camSizeDecrease;


        }


    }

    public void resetOrtho()
    {
        cam.orthographicSize = orthoSizeDefault;
    }
    private void deathCheck()
    {



    }


    public bool playerCanSeeEnd()
    {




        float end = cam.WorldToViewportPoint(player.CurrentTerrain.SpawnRight).x;

        return end > 0.0f && end < 1.0f;


    }


    private void followPLayerPosition()
    {


        if (followPLayer)
        {
            offsetPos = Vector3.Lerp(cam.transform.position, new Vector3(player.transform.position.x + offsetX, cam.transform.position.y, cam.transform.position.z), offsetSmooth * Time.deltaTime);

            cam.transform.position = offsetPos;




        }

      



    }



    private void checkDeath()
    {
        if (deathChecker.IsAlive == false)
        {

            if (cam.orthographicSize > deathZoom)
            {
                Debug.Log("cam size is  " + cam.orthographicSize);
                cam.orthographicSize -= 0.01f;
            }
        }



    }


    public bool shiftToPosition(Vector3 position, float smoothing)
    {

        if (transform.position != position)
        {

            transform.position = Vector3.Lerp(transform.position, position, smoothing);
            return false;

        }

        return true;
    }


    public bool shiftToPlayerPosition()
    {

        if (transform.position != offsetPos)
        {

            transform.position = Vector3.Lerp(transform.position, offsetPos, shiftingBackSmooth);
            return false;

        }

        return true;
    }


    public bool playerCamHasReachedPlayerPos()
    {
        Debug.Log("player cam reached position for offset " + (transform.position == new Vector3(player.transform.position.x + offsetX, cam.transform.position.y, cam.transform.position.z)));
        return transform.position == new Vector3(player.transform.position.x +offsetX, cam.transform.position.y , cam.transform.position.z);
    }

    public Vector2 HalfRect
    {
        get { return new Vector2(cam.pixelRect.width / 2.0f, cam.pixelRect.height / 2.0f); }
    
    }
    public Vector2 HalfPixelRes 
    {
        get { return new Vector2(cam.scaledPixelWidth / 2.0f, cam.scaledPixelHeight / 2.0f); }
    
    }
    public bool FollowPlayer
    {
        set { followPLayer = value; }
    }


}
