using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public  abstract class  Collectible : MonoBehaviour
{

  
    [SerializeField] private int maxAmount;
    [SerializeField] private int minAmount;
    protected bool startInteraction = false;
    public LayerMask overlaps;
    protected playerController playerController;
    protected Camera playerCam;
    private Collider2D collectibleCollider;
    public AudioSource sound; 
    protected InstaniateScorePopUp scorePopUp;
    
    public int MaxAmount
    {
        get
        {
            return maxAmount;
        }

    }
    public int MinAmount
    {
        get
        {
            return minAmount;
        }

    }
    private void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        playerCam = Camera.main;
        scorePopUp = GetComponent<InstaniateScorePopUp>();
         
     
        Debug.Log("collider null " + (collectibleCollider == null));
     




        CollectibleStart();
    }



    public void CollectibleEnable()
    {
        if(collectibleCollider== null)
        {


            collectibleCollider = GetComponent<Collider2D>();
             
            Debug.Log("COLLECTIBLE COLLIDER SET  collider set " + collectibleCollider.GetType());
        }
        GetComponent<SpriteRenderer>().enabled = true;
      
       /*StartCoroutine( checkLateOverlap());*/
    }

    public void instaniateScoreObject()
    {

        scorePopUp.inistantiateScorePop(transform.position, Quaternion.identity);


    }
   
    public abstract void CollectibleStart();
  
    public abstract void interact();

    public abstract void CollectibleUpdate();
   
    
    private void Update()
    {
        if (!playerController.IsViewingNextTerrain)
        {

            if (startInteraction)
            {
                interact();


            }

            if (!isOnScreen())
            {

                gameObject.SetActive(false);


            }

        }



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           Debug.Log(" colllectible sound play");
            sound.Play();
            scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
            startInteraction = true;
           


        }
       
        

    }



    private void OnCollisionStay2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Kickable"))
        {

            float directionX = transform.position.x - collision.transform.position.x>=0.0f ? 1.0f :-1.0f ;
            float bottomOfColliderY = collision.collider.transform.position.y - collision.collider.bounds.size.y / 2.0f;
            float topOfColliderY = collision.collider.transform.position.y + collision.collider.bounds.size.y / 2.0f;
           
            float overlap = Mathf.Abs(transform.position.y - bottomOfColliderY)/(topOfColliderY - bottomOfColliderY);
            transform.position = new Vector3(transform.position.x , transform.position.y + collision.collider.bounds.size.y * overlap , transform.position.z);
            Debug.Log(" COLLECTIBLE  new position due to overlap ");
        }
    }



    protected bool isOnScreen()
    {
        return playerCam.WorldToViewportPoint(transform.position).x >= 0.0f;


    }









}
