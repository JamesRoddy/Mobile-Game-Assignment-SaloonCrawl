using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    private float offsetX = 3.0f;
    private Camera cam;
    private playerController player;
    private DeathChecker deathChecker;
    private float deathZoom = 2;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<playerController>();
        deathChecker = player.GetComponent<DeathChecker>();

    }

    // Update is called once per frame
    void Update()
    {
        updateCamPosition();
    }



    void updateCamPosition()
    {
        Vector3 finalCamPos = new Vector3(player.transform.position.x + offsetX, cam.transform.position.y, cam.transform.position.z);
        if(deathChecker.IsAlive == false)
        {
/*         finalCamPos = new Vector2(player.transform.position.x, player.transform.position.y);*/

            if(cam.orthographicSize > 2)
            {
                Debug.Log("cam size is  " + cam.orthographicSize);
                cam.orthographicSize -= 0.01f;
            }
        }
        cam.transform.position = finalCamPos;




    }






}
