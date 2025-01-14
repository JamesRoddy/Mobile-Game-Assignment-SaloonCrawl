using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    private float offsetX = 3.0f;
    private float offsetY = 2.0f;
    private Camera cam;
    private playerController player;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCamPosition();
    }



    void updateCamPosition()
    {
        Vector3 finalCamPos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, cam.transform.position.z);

        cam.transform.position = finalCamPos;




    }






}
