using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public  abstract class  Collectible : MonoBehaviour
{

  
    [SerializeField] private int maxAmount;
    [SerializeField] private int minAmount;
    private LayerMask overlaps;
    protected playerController playerController;
    protected Camera playerCam;
    private Collider2D collectibleCollider;
    public  AudioSource collectibleSound;
    protected bool startInteraction = false;
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
               
        Debug.Log("collider null " + (collectibleCollider == null));
     
        CollectibleStart();
        
        Debug.Log("collectible sound is null " + (collectibleSound == null));
    }



    public void CollectibleEnable()
    {
        if(collectibleCollider== null)
        {
            

            collectibleCollider = GetComponent<Collider2D>();
            Debug.Log("COLLECTIBLE COLLIDER SET  collider set " + collectibleCollider.GetType());
        }
        overlaps = LayerMask.GetMask("Collectible", "Interactable");
        StartCoroutine(checkLateOverlap());
    }


    private IEnumerator checkLateOverlap()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("COLLECITBLE overlap checks " + overlaps.ToString() + "is overlaping " + collectibleCollider.IsTouchingLayers(overlaps));

        Vector2 startDir = new Vector2(0.0f, 0.0f); 


        for(float angle = 0.0f; angle<360.0f; angle += 90.0f )
        {
            
            startDir.x = Mathf.Cos(angle);
            startDir.y = Mathf.Sin(angle);


            RaycastHit2D ray = Physics2D.Raycast(transform.position, startDir,overlaps);
            if (ray && Mathf.Abs(transform.position.x - ray.collider.transform.position.x) < collectibleCollider.bounds.size.x && Mathf.Abs(transform.position.x - ray.collider.transform.position.x) < collectibleCollider.bounds.size.x  && 
                Mathf.Abs(transform.position.x - ray.collider.transform.position.y) < collectibleCollider.bounds.size.y)
            {

                float side = transform.position.x - ray.collider.transform.position.x <= 0.0f ? -1.0f : 1.0f;

                transform.position = new Vector3(transform.position.x + ray.collider.bounds.size.x * side, transform.position.y, transform.position.z);


                Debug.Log("COLLECTIBLES overlap found repositioning " + transform.position);




            }



            



        }




        



    }

    public abstract void CollectibleStart();
  
    public abstract void interact();

    public abstract void CollectibleUpdate();
   
    
    private void Update()
    {
        if (!playerController.IsViewingNextTerrain)
        {
            if (!isOnScreen())
            {

                gameObject.SetActive(false);


            }


            if (startInteraction)
            {
                interact();
            }

        }



    }


    public abstract void increaseStat();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            startInteraction = true;           

        }
        

    }

    
    protected bool isOnScreen()
    {
        return playerCam.WorldToViewportPoint(transform.position).x >= 0.0f;


    }









}
