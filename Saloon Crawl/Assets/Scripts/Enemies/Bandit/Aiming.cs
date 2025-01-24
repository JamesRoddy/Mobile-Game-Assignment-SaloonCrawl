using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Aiming : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 shootPosition = Vector3 .zero;
    private LayerMask contactLayers;
    private float aimingTime;
    private float fireDelay;
    private float distance;
    public float range;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer parentSprite;
    public Sprite aimingSprite;
    public Sprite shootSprite;
    private Camera playerCam;
    private BoxCollider2D boxCollider;
    private Transform banditTransform;
    private LineRenderer parentLineRenderer;
    private Transform gunTransform;
    private TrailRenderer bulletTrail;
    private bool isFiring = false;
    private Vector3 startPosition;
    private playerController playerController; // use this 
    private float flipX;
    private bool canFlip = true;
    private DeathChecker deathChecker;


    // Start is called before the first frame update
    void Start()
    {
        parentSprite = GetComponentInParent<SpriteRenderer>(); 
        playerController = FindObjectOfType<playerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = Camera.main;

        spriteRenderer = transform.Find("Arm").GetComponent<SpriteRenderer>(); // get the sprite renderer of the arm child  ]
        gunTransform = transform.Find("Arm").transform.Find("GunPos");
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        banditTransform = transform.parent.GetComponent<Transform>();
        contactLayers = LayerMask.GetMask("Player", "Ground");
        bulletTrail = transform.parent.Find("TrailPos").GetComponent<TrailRenderer>();
        bulletTrail.time = 0.15f;
        parentLineRenderer  = transform.parent.GetComponent<LineRenderer>();

        parentLineRenderer.startColor = Color.red; 
        parentLineRenderer.endColor = Color.red;
        parentLineRenderer.startWidth = 0.01f;
        parentLineRenderer.endWidth = 0.01f;
        deathChecker = player.GetComponent<DeathChecker>();
        bulletTrail.enabled = false;
       
        Debug.Log( "arm not null "+(transform.Find("Arm") != null));
/*        boxCollider = GetComponent<BoxCollider2D>();*/
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position; // get player position into a vec 3 
        playerCam.WorldToViewportPoint(transform.position);
        distance = Vector2.Distance(transform.position, playerPosition); // gets the distance between the player and the enemy
     /*  Debug.Log("distance is  " + distance);*/
        if(canFlip == true)
        {
            shouldFlipTowardsPlayer();
        }
        
        if (distance < range) //&&!playerController.isCloseToEndOfCurrentterrain( )
        {
/*            LookAtPlayer(playerPosition); // turns the arm to look at the player position*/
            Shooting();
        }
        else if(isInNotCameraView()) 
        {
            Debug.Log("set active false " + isInNotCameraView());
            banditTransform.gameObject.SetActive(false);
        }
        
        //Debug.Log("Bandit death/ isAlive: " + banditDeath.IsAlive);
    }

    //function taking care of rotating the arm towards the player
    private void LookAtPlayer(Vector3 playerPosition)
    {
        Vector3 rotateDirection = transform.position - playerPosition; // find out the distance between player and enemy 
        rotateDirection.Normalize(); // normalise that distance
        float angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg; // use atan2 to figure out how much to rotate and then change it into degrees 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // rotate the Z axis only
        
    }


    private void setLinePosition(Vector3 start, Vector3 end) {

        if (parentLineRenderer.enabled)
        {
           /* Debug.Log(" set line positio start " + start + "line end " + end);*/
            Vector3[] newPositions = new Vector3[2] { start, end };

            parentLineRenderer.SetPositions(newPositions);
        }
      


    
    }
    private void shouldFlipTowardsPlayer()
    {

        float currentDifference = playerPosition.x - transform.position.x; 

        if( parentSprite.flipX != true && currentDifference >0.0f)
        {
           /* Debug.Log("should flip " + (currentDifference > 0.0f));*/
            parentSprite.flipX = true;
            transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,transform.localScale.z);


        }



       
        

    }
    private bool isInNotCameraView()
    { 

        Vector3 camViewPortPos = playerCam.WorldToViewportPoint(new Vector3(banditTransform.position.x + boxCollider.bounds.size.x/2.0f,banditTransform.position.y,banditTransform.position.z ));
        return camViewPortPos.x < 0.0f;
    }
    private void Shooting()
    {
        aimingTime += Time.deltaTime; // incremements the aiming timer
        /*        Debug.Log("aiming time is" + aimingTime);*/
        parentLineRenderer.enabled = true;

        if (aimingTime > 1) // after 1 second stop looking at the player position
        {
            if (shootPosition == Vector3.zero)
            {

               /* Debug.Log("shoot position " + shootPosition);*/
                shootPosition = playerPosition; // shoot position is the same as the last player position
                LookAtPlayer(shootPosition); // turn the arm to look at where the bandit is going to shoot
                bulletTrail.transform.position = gunTransform.position;
                Debug.Log("position locked "+bulletTrail.transform.position +"gun position "+gunTransform.position);
            }
            canFlip = false;
            //            Debug.Log("Player position locked in");
            fireDelay += Time.deltaTime; // increment the firing delay timer

            /*Debug.Log("fire delay is " + fireDelay);*/
            if (fireDelay > 2) // after 2 seconds shoot
            {
                Debug.Log("FIRE");
                aimingTime = 0;
                fireDelay = 0;
                parentLineRenderer.enabled = false;
                bulletTrail.enabled = true;
                Debug.Log("gun pos " + gunTransform.position + "trail pos" + bulletTrail.transform.position);
                Fire();
                shootPosition = Vector3.zero;
                canFlip = true;



            }
            
            setLinePosition(gunTransform.position, shootPosition);
            return;
        }
        LookAtPlayer(playerPosition); // turn the arm to look at where the bandit is going to shoot
        setLinePosition(gunTransform.position, playerPosition);

    }


    private void Fire()
    {

        Vector2 dir =  (Vector2)shootPosition - (Vector2) gunTransform.transform.position;
        dir.Normalize();
        RaycastHit2D hit  =  Physics2D.Raycast(gunTransform.transform.position, dir, float.MaxValue,contactLayers);
        
        if (hit)
        {
            Debug.Log("has hit player or ground  " + hit.point);
            StartCoroutine( drawTrailToHit(hit));
            return;
        }

        StartCoroutine( drawTrailToOfScreen());




    }


    private IEnumerator drawTrailToHit(RaycastHit2D hit)
    {
        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = true;
        float time = 0.0f;
      /*  bulletTrail.enabled = true;*/
        Vector3 hitPos = hit.point;
      
        Vector3 trailStart = gunTransform.position;
        while (time < 1.0f)
        { 

            Debug.Log("SPANWING TRAIL DUE TO HIT  ");
            bulletTrail.transform.position = Vector3.Lerp(trailStart, hitPos, time);

            time += Time.deltaTime / bulletTrail.time;
            yield return null;
        }
        if (hit.collider.gameObject.CompareTag("Player"))
        {
            deathChecker.isAlive = false;
            bulletTrail.transform.position = gunTransform.position;
            bulletTrail.enabled = false;
        }
        Debug.Log("SPANWING TRAIL DUE TO HIT FINISHED  ");

        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = false;




    }

  
    private IEnumerator drawTrailToOfScreen()
    {
        float direction = shootPosition.x < banditTransform.position.x ? -1.0f : 1.0f;
        Debug.Log("player pixel width " + playerCam.pixelWidth +"shoot position smaller "+(shootPosition.x<banditTransform.position.x));
        Vector3 screenPos = playerCam.ScreenToWorldPoint( (playerCam.WorldToScreenPoint(new Vector3( Mathf.Abs(gunTransform.position.x),gunTransform.position.y,gunTransform.position.z) + new Vector3(playerCam.pixelWidth * direction,0.0f,0.0f))));
        Vector3 worldPosOffScreen = playerCam.ScreenToWorldPoint(screenPos);
        Debug.Log( "trail target for off screen " + worldPosOffScreen +"direction "+direction);
        float time = 0.0f;
        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = true;
        Vector3 trailStart = gunTransform.position;
        while (time<1.0f)
        {
            Debug.Log("SPANWING TRAIL OFFSCREEN DUE TO MISS  ");

            bulletTrail.transform.position = Vector3.Lerp(trailStart, worldPosOffScreen, time);

            time += Time.deltaTime/bulletTrail.time;            
            yield return null;
        }
        Debug.Log("SPANWING TRAIL OFFSCREEN DUE TO MISS FINISHED  ");
        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = false;














    }

    







}
