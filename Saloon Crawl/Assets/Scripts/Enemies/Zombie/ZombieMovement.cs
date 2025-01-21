using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ZombieMovement : MonoBehaviour
{

    private float speed = -5;
    private Rigidbody2D zombieRigidBody;
    private Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        zombieRigidBody = GetComponent<Rigidbody2D>();
        playerCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if(isInNotCameraView())
        {
            gameObject.SetActive(false);
        }
    }

    private void Movement()
    {
        zombieRigidBody.velocity = new Vector2(speed, zombieRigidBody.velocity.y);
    }
    private bool isInNotCameraView()
    {

        Vector3 camViewPortPos = playerCam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        return camViewPortPos.x < 0.0f;
    }
}
