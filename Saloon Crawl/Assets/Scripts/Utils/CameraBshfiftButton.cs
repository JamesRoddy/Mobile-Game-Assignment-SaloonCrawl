using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBshfiftButton : MonoBehaviour
{
    // Start is called before the first frame update

    playerController playerController;
    CamMovement playerCam;


    void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<CamMovement>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
