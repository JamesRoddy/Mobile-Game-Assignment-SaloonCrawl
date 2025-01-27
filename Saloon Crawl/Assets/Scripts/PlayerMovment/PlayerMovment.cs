using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class playerController : MonoBehaviour
{


    private BoxCollider2D playerBoxCollider;
    private Rigidbody2D playerRigidBody;
    private TerrainType currentTerrain;
    private TouchControls touchControls;
    private Camera currentCam;
    private bool shouldIncrementScore = true;
    private int currentTerrainCycles;
    private float playerSpeed = 4.0f;
    private float minDistanceToEndOfCurrentTerrain = 144.0f;
    private bool shouldJump = false;
    [SerializeField] private GameObject spriteCrossHair;
    private SpriteRenderer crossHairRenderer;
    private float crossHairFadeMax = 1.0f;
    private float crossHairFadeTimer = 0.0f;
    [SerializeField] LayerMask groundLayer;
    private float slideVelocity = 5.0f;
    bool grounded = false;
    float jumpVelocity = 7.0f;
    public Animator CowboyAnim;
    private float playerInvul;
    private float slideInVul;
   
    private float scoreIncrement = 0.0f;
    private float scoreIncrementMax = 0.1f;
    private float scoreFadeMax = 0.65f;
    private float scoreFadeIncrement = 0.0f;
    //Shooting variables
    public bool shouldShoot = false;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public bool invoked = false;
    public float fBulletAngle;
    public int iTapCount = 0;
    GameObject bullet;
    Bullet bull;
    public GameObject arm;
    private int score;
    private bool isViewingNextTerrain = false;
    private int currentCoinCount = 0;
    private CamMovement followCam;
    private bool hasConcatenatedScore = false;
    public DeathChecker deathChecker;
    string concatString = "";
    private GameManagerScript gameManagerScript;
    private UpdateScoreText scoreText;
    private Vector3 nextTerrainPos;
    public bool shouldSlide = false;
    bool isSliding = false;
    float fSlidePowerY = -10f;
    private PlayerCameraShift PlayerCameraShift;
    private string scoreString = "";
    //Jump Soundeffects
    public AudioSource runSound;
    public AudioSource bulletShot;
    public AudioSource jumpSound;
    public AudioSource slideSound;
    public AudioSource kickSound;
    public AudioSource gameOver;
    public AudioSource mainMusic;
    private float crossHairDistance = 8.0f;
    public Vector2 Dir;
    public Vector2 touchStore;
    float t;

    [SerializeField] private GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        currentCam = FindFirstObjectByType<Camera>();
        PlayerCameraShift = currentCam.GetComponent<PlayerCameraShift>();
        CowboyAnim = GetComponent<Animator>();
        bull = FindObjectOfType<Bullet>();
        followCam = FindFirstObjectByType<CamMovement>();
        touchControls = FindFirstObjectByType<TouchControls>();
        runSound.Play();
        gameManagerScript = FindFirstObjectByType<GameManagerScript>();
        scoreText = FindFirstObjectByType<UpdateScoreText>();
        crossHairRenderer = spriteCrossHair.GetComponent<SpriteRenderer>(); 
        crossHairRenderer.enabled = false;
        deathChecker = GetComponent<DeathChecker>();
        playerRigidBody.velocity = new Vector2(playerSpeed, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {

           shouldViewNextTerrain();
           grounded = isGrounded();
           updateScoreString();
           updateCrossHair();
           addMomentum();
           jump();
           shoot();
           slide();
           CowboyAnim.SetBool("OnGround", grounded && !IsViewingNextTerrain);
           CowboyAnim.SetBool("IsAlive", deathChecker.IsAlive);
           IsDead();
            
      

    }
    private void shouldViewNextTerrain()
    {
        if(currentTerrain.NextTerrainType != null && (touchControls.accelerationHasHitPositve() || touchControls.SwipeLeft) && !IsViewingNextTerrain &&! PlayerCameraShift.IsShiftingBack)
        {
            IsViewingNextTerrain = true;
            followCam.FollowPlayer = false; 
            nextTerrainPos = new Vector3(currentTerrain.NextTerrainType.transform.position.x, transform.position.y, transform.position.z);
        }

        if (!IsViewingNextTerrain )
        {

            playerRigidBody.simulated = true;
            shouldIncrementScore = true;
            return;
        }
        
        playerRigidBody.simulated = false;
        shouldIncrementScore = false;


    }
  

    public void conactToScore(string concat)
    {
        concatString = "+"+ concat;

        if (hasConcatenatedScore)
        {
            scoreFadeIncrement = 0.0f;
        }

        hasConcatenatedScore = true;
    }


    private void updateScoreString()
    {

        if (hasConcatenatedScore)
        {

            if(scoreFadeIncrement <scoreFadeMax)
            {
                scoreFadeIncrement += Time.deltaTime;
                return;
            }
            scoreFadeIncrement = 0.0f;
            concatString = "";
            hasConcatenatedScore = false;

        }


    }
    private void addMomentum()
    {

        playerRigidBody.velocity = new Vector2(playerSpeed, playerRigidBody.velocity.y);   
    
    }

    
    public bool isOnRightSideByCertainFractionOfScale(float divider)
    {

        return transform.position.x >= (currentTerrain.transform.position.x + currentTerrain.transform.localScale.x / divider);


    }

    private void jump()
    {
        if (shouldJump && grounded && !IsViewingNextTerrain)
        {

            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpVelocity);

            shouldJump = false;
            jumpSound.Play();  
        }



    }

    private void shoot()
    {
        t += Time.deltaTime;
        if (!IsViewingNextTerrain && shouldShoot && t >= 0.5f && !deathChecker.isAlive == false)
        {
            invoked = true;
            shouldShoot = false;
            Dir = touchStore - (new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y));
            Dir.Normalize();
            fBulletAngle = Mathf.Atan2(Dir.y, Dir.x);
           
            arm.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, fBulletAngle * Mathf.Rad2Deg));
            crossHairFadeTimer = 0.0f;
            crossHairRenderer.enabled = true;
          
            spriteCrossHair.transform.position = bulletSpawnPoint.position + new Vector3(Dir.x * crossHairDistance, Dir.y * crossHairDistance, 0.0f);

           
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
          

            

       
            bulletShot.Play();
            t = 0f; 
        }

        else
        {
            shouldShoot = false;
        }

        
    }
 

    public bool CurrentTerrainCyclesIsMultiple(int numberToTest) {

        if (numberToTest == 0)
        {
            return true;
        }

        return currentTerrainCycles % numberToTest == 0;
}
    private void slide()
    {
        if(isSliding || IsViewingNextTerrain)
        {
            return;
        }

        else if (shouldSlide)
        {
            slideSound.Play();
            StartCoroutine(AdjustCollider());
        }
        
    }

    IEnumerator AdjustCollider()
    {
        float fStoreX = playerBoxCollider.size.x;
        float fStoreY = playerBoxCollider.size.y;
        shouldSlide = false;
        isSliding = true;
        CowboyAnim.SetBool("IsSliding", true);

        if (!Grounded)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, fSlidePowerY);
        }

        else
        {
            playerRigidBody.velocity = new Vector2(slideVelocity, playerRigidBody.velocity.y);
            playerBoxCollider.size = new Vector2(fStoreY, fStoreX - 0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        playerRigidBody.velocity = new Vector2(playerSpeed,  playerRigidBody.velocity.y);
        playerBoxCollider.size = new Vector2(fStoreX, fStoreY);
        isSliding = false; 

        CowboyAnim.SetBool("IsSliding", false);

        

    }

    public void updateCrossHair() {


        if (crossHairFadeTimer < crossHairFadeMax && crossHairRenderer.enabled )
        {
            crossHairFadeTimer += Time.deltaTime;
            return;
        }


        crossHairRenderer.enabled = false;
        crossHairFadeTimer = 0;

    }

    public bool isInCurrentTerrian()
    {

        return (transform.position.x <= currentTerrain.SpawnRight.x && transform.position.x >= currentTerrain.SpawnLeft.x);
    }
    public bool isCloseToEndOfCurrentterrain()
    {

        return Vector3.SqrMagnitude((currentTerrain.transform.position + currentTerrain.getHalfScale) - transform.position) <= minDistanceToEndOfCurrentTerrain;

    }

    private void ScoreConstantIncrement()
    {
        string scoreString ;
        
        if (shouldIncrementScore)
        {
            scoreIncrement += Time.deltaTime;
            
            if (scoreIncrement >= scoreIncrementMax)
            {
                CurrentScore++;
                currentScoreString = Convert.ToString(CurrentScore);
                scoreIncrement = 0.0f;
            }

        }

        scoreString = currentScoreString + concatString;

        scoreText.UpdateText(scoreString);
      

    }
    public Vector2 playerPosVec2
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }
       

    public bool ShouldJump
    {
        set { shouldJump = value; }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            collision.collider.enabled = false;
            Rigidbody2D enemyBody = collision.collider.GetComponent<Rigidbody2D>();
            enemyBody.simulated = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }


    public int CurrentCoinCount { 
        get { return currentCoinCount; }
        set { currentCoinCount = value; }
    }
    public Vector3 NextTerrainCamPos
    {
        get { return new Vector3(nextTerrainPos.x,nextTerrainPos.y,currentCam.transform.position.z); }
    }

   
    public bool IsViewingNextTerrain
    {
        set {  isViewingNextTerrain = value; }
        get { return isViewingNextTerrain; }
    }
    public string currentScoreString
    {
        get { return scoreString; }
        set { scoreString = value; }
    }
    public bool Grounded
    {
        get { return grounded; }
    }

    public TerrainType CurrentTerrain
    {
        set { currentTerrain = value; }
        get { return currentTerrain; }
    }

    public GameObject getCurrentTerrainGameObject
    {
        get { return currentTerrain.gameObject; }
    }

    public int getTerrainCycles
    {
        get { return currentTerrainCycles; }
        set { currentTerrainCycles = value; }
    }
    public TerrainClassifications CurrentTerrainClassification
    {

        get { return currentTerrain.GetComponent<TerrainType>().GetClassification; }
    } 

    public  int CurrentScore
    {
        get { return score; }
        set { score = value; }
    } 
    public void addToPoints(int value)
    {
        score += value;
    }
    bool isGrounded()
    {


        RaycastHit2D hit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);


        return hit.collider != null;
    }

    public void StopAllSounds()
    {
        mainMusic.Stop();
        runSound.Stop();
        jumpSound.Stop();
        kickSound.Stop();
        slideSound.Stop();
        bulletShot.Stop();
    }

    private void IsDead()
    {
        if ( deathChecker.IsAlive == false)
        {
            
            gameManagerScript.finalScore = score;


            CowboyAnim.SetBool("IsAlive", false);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            if (grounded == true)
            {
               if(CowboyAnim.GetBool("Kick") == false && CowboyAnim.GetBool("IsSliding") == false)
                {
                    Time.timeScale = 0;
                    gameOverScreen.GetComponent<MenuButtonScript>().EnableMenu();
                    Handheld.Vibrate();
                    StopAllSounds();
                }
            }
            return;
        }
        
        ScoreConstantIncrement();
    }

    public bool IsSliding
    {
        get { return isSliding; }
    }

    

}

