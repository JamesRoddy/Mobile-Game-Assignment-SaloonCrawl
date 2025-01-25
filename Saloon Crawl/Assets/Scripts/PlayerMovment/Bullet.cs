using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bullet;
    float fBulletSpeed = 30f;
    
    TouchControls control;
    playerController player;
    private Camera playerCam;
    private BoxCollider2D boxCollider;
    private Renderer offScreenCheck;

    public LayerMask Ground;
    public LayerMask Enemy;
    [SerializeField] private TrailRenderer tr;


    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<TouchControls>();
        player = FindObjectOfType<playerController>();
        bullet = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        offScreenCheck = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Bullet Active: " + isActiveAndEnabled);

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, player.fBulletAngle));
        bullet.velocity = player.Dir * fBulletSpeed;
        tr.emitting = true;
        
        if(isNotInCameraView())
        {
            Destroy(this.gameObject, 0.2f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       Debug.Log("Collision");
       if (collision.gameObject.CompareTag("Enemy"))
       {
            Destroy(this.gameObject);
       }

       if (collision.gameObject.CompareTag("Ground"))
       {
            Destroy(this.gameObject);
       }

    }

    private bool isNotInCameraView()
    {
        bool isNotInCameraView = false;

        if (!offScreenCheck)
        {
            isNotInCameraView = true;
        }


        return isNotInCameraView;
    }


}
