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
    private Vector3 shootPosition = Vector3.zero;
    private LayerMask contactLayers;
    private float aimingTime;
    private float fireDelay;
    private float distance;
    public float range;
    private SpriteRenderer parentSprite;
    public Sprite aimingSprite;
    public Sprite shootSprite;
    private Camera playerCam;
    private BoxCollider2D boxCollider;
    private Transform banditTransform;
    private LineRenderer parentLineRenderer;
    private Transform gunTransform;
    private TrailRenderer bulletTrail;
    private bool canFlip = true;
    private DeathChecker deathChecker;
    private Animator indicator;
    private float reload;
    private playerController playerController;
    private Vector3 shootDirection;
    public AudioSource cockGun;

    public  void AimingStart()
    {
        parentSprite = GetComponentInParent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = Camera.main;

        gunTransform = transform.Find("Arm").transform.Find("GunPos");
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        banditTransform = transform.parent.GetComponent<Transform>();
        contactLayers = LayerMask.GetMask("Player", "Ground");
        bulletTrail = transform.parent.Find("TrailPos").GetComponent<TrailRenderer>();
        bulletTrail.time = 0.15f;
        parentLineRenderer = transform.parent.GetComponent<LineRenderer>();

        parentLineRenderer.startColor = Color.red;
        parentLineRenderer.endColor = Color.red;
        parentLineRenderer.startWidth = 0.02f;
        parentLineRenderer.endWidth = 0.02f;
        Debug.Log("enemy start");
        deathChecker = player.GetComponent<DeathChecker>();
        bulletTrail.enabled = false;
        indicator = transform.parent.Find("Indicator").GetComponent<Animator>();
        playerController = FindFirstObjectByType<playerController>();
        fireDelay = 0;
        aimingTime = 0;

        Debug.Log("arm not null " + (transform.Find("Arm") != null));
        /*        boxCollider = GetComponent<BoxCollider2D>();*/
    }

    public void AimingUpdate()
    {
        playerPosition = player.transform.position; // get player position into a vec 3 
        playerCam.WorldToViewportPoint(transform.position);
        distance = Vector2.Distance(transform.position, playerPosition); // gets the distance between the player and the enemy
        /*  Debug.Log("distance is  " + distance);*/
        if (canFlip == true)
        {
            shouldFlipTowardsPlayer();
        }

        if (distance < range) //&&!playerController.isCloseToEndOfCurrentterrain( )
        {
            /*            LookAtPlayer(playerPosition); // turns the arm to look at the player position*/
            Shooting();
        }
        else if (isInNotCameraView())
        {
            Debug.Log("set active false " + isInNotCameraView());
            banditTransform.gameObject.SetActive(false);
        }

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

        if (parentSprite.flipX != true && currentDifference > 0.0f)
        {
            /* Debug.Log("should flip " + (currentDifference > 0.0f));*/
            parentSprite.flipX = true;
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);


        }







    }
    private bool isInNotCameraView()
    {

        Vector3 camViewPortPos = playerCam.WorldToViewportPoint(new Vector3(banditTransform.position.x + boxCollider.bounds.size.x / 2.0f, banditTransform.position.y, banditTransform.position.z));
        return camViewPortPos.x < 0.0f;
    } 

    public void aimingEnable()
    {

        fireDelay = 0;
        aimingTime = 0;
        bulletTrail.enabled = false;
        parentLineRenderer.enabled = true;
        shootDirection = Vector3.zero;
        shootPosition = Vector3.zero;
        indicator.SetBool("Warning ", false);
        canFlip = true ;
        bulletTrail.transform.position = gunTransform.position;
        setLinePosition(gunTransform.position, gunTransform.position);

    }
    private void Shooting()
    {
        aimingTime += Time.deltaTime; // incremements the aiming timer
        /*        Debug.Log("aiming time is" + aimingTime);*/
        parentLineRenderer.enabled = !bulletTrail.enabled;
        if (aimingTime > 1) // after 1 second stop looking at the player position
        {

            if (shootPosition == Vector3.zero)
            {
                shootPosition = playerPosition; // shoot position is the same as the last player position
                LookAtPlayer(shootPosition); // turn the arm to look at where the bandit is going to shoot
                bulletTrail.transform.position = gunTransform.position;
                indicator.SetBool("Warning", true);
                cockGun.PlayDelayed(0.5f);
                Debug.Log("position locked " + bulletTrail.transform.position + "gun position " + gunTransform.position);
                shootDirection = shootPosition - gunTransform.position;
                shootDirection.Normalize();
            }
            canFlip = false;
            fireDelay += Time.deltaTime; // increment the firing delay timer

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
                indicator.SetBool("Warning", false);



            }
            if (parentSprite.flipX == true)
            {
                setLinePosition(gunTransform.position, shootPosition + shootDirection * 5.0f); //playerController.getPlayerSpeed
                return;
            }
            else
            {
                setLinePosition(gunTransform.position, shootPosition);
                return;
            }
        }

        if (bulletTrail.enabled == false)
        {
            LookAtPlayer(playerPosition); // turn the arm to look at where the bandit is going to shoot
            setLinePosition(gunTransform.position, playerPosition);
        }


    }


    private void Fire()
    {

        Vector2 dir = (Vector2)shootPosition - (Vector2)gunTransform.transform.position;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(gunTransform.transform.position, dir, float.MaxValue, contactLayers);

        if (hit)
        {
            Debug.Log("has hit player or ground  " + hit.point);
            StartCoroutine(drawTrailToHit(hit));
            return;
        }

        StartCoroutine(drawTrailToOfScreen());






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
            DeathChecker playerDeath =   hit.collider.gameObject.GetComponent<DeathChecker>();
            playerDeath.isAlive = false;
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
        Debug.Log("player pixel width " + playerCam.pixelWidth + "shoot position smaller " + (shootPosition.x < banditTransform.position.x));


        Vector3 directionVec = new Vector3(shootPosition.x - gunTransform.position.x, shootPosition.y - gunTransform.position.y, 0.0f);
        directionVec.Normalize();
        Vector2 screenOffset = new Vector2(playerCam.scaledPixelWidth * direction, 0.0f);
        float screenPositionDisstance = Vector3.SqrMagnitude((playerCam.WorldToScreenPoint(shootPosition) + (Vector3)screenOffset) - playerCam.WorldToScreenPoint(shootPosition));


        Vector3 finalPos = new Vector3(shootPosition.x + directionVec.x * screenPositionDisstance, shootPosition.y + directionVec.y * screenPositionDisstance, shootPosition.z);




        Debug.Log("trail target for off screen " + finalPos + "direction " + direction);
        float time = 0.0f;
        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = true;
        Vector3 trailStart = gunTransform.position;
        while (time < 1.0f)
        {
            Debug.Log("SPANWING TRAIL OFFSCREEN DUE TO MISS  ");

            bulletTrail.transform.position = Vector3.Lerp(trailStart, finalPos, time);

            time += Time.deltaTime / bulletTrail.time;
            yield return null;
        }
        Debug.Log("SPANWING TRAIL OFFSCREEN DUE TO MISS FINISHED  ");
        bulletTrail.transform.position = gunTransform.position;
        bulletTrail.enabled = false;
















    }









}
