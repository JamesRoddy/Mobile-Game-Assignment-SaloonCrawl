using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bullet;
    float fBulletSpeed = 60f;
    float bulletDestroyTime = 0.025f;
    TouchControls control;
    playerController player;
    private Camera playerCam;
    private BoxCollider2D boxCollider;
    private SpriteRenderer offScreenCheck;

    public LayerMask Ground;
    public LayerMask Enemy;
    [SerializeField] private TrailRenderer tr;
    public Vector2 Dir;


    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<TouchControls>();
        player = FindObjectOfType<playerController>();
        bullet = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        offScreenCheck = GetComponent<SpriteRenderer>();
        playerCam = FindFirstObjectByType<Camera>();
        Dir = player.Dir;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, player.fBulletAngle));
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate( Dir * fBulletSpeed *Time.deltaTime);
        tr.emitting = true;
        
        if(isNotInCameraView())
        {
            Destroy(this.gameObject, bulletDestroyTime);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       Debug.Log("Collision");
       if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("EventObjectShootable"))
       {
            Destroy(this.gameObject);
       }

      
        


    }

    private bool isNotInCameraView()
    {
        bool isNotInCameraView = false;

        Vector3 viewPortPos = playerCam.WorldToViewportPoint(transform.position - offScreenCheck.bounds.size/2.0f); 

        

        if (viewPortPos.x>1.0f || viewPortPos.y >1.0f )
        {
            
            isNotInCameraView = true;
        }


        return isNotInCameraView;
    }


}
