using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class tutorialtrigger : MonoBehaviour
{
    // Start is called before the first frame update
     Collider2D trigger;
     TouchControls playerTouch;
     SpriteRenderer tutorialSpirte;
     [SerializeField]  string expectedInput;
    [SerializeField] private GameObject symbol;
    [SerializeField] private GameObject text;
    SpriteRenderer symbolSprite;
    SpriteRenderer textSprite;
    [SerializeField] private float timeScalar;
    [SerializeField] private float distcanceCheck;
    private bool shouldUpdateDistance = false;
    playerController playerController;
    [SerializeField] bool hasGameObject;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] GameObject postionForobject;
    [SerializeField] bool objectCanKillPlayer;
    [SerializeField] float waitDeathTimer;
    GameObject objectInstance = null;
    private bool objectHasKilledPlayer = false;
    private playerController player;
    private Collider2D playerCol;
    private Collider2D objectSpawnCol;
    private bool waitingForDeath = false;
    [SerializeField] float deathWaitMax;
    private bool inputStore;
    private bool hasCollided = false;
    void Start()
    {
        
        trigger  = GetComponent<Collider2D>();
        playerTouch = FindFirstObjectByType<TouchControls>();
        Debug.Log("tutorial trigger bool set to  " + Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)));
        player = FindFirstObjectByType<playerController>();
        Debug.Log(expectedInput);
        symbolSprite = symbol.GetComponent < SpriteRenderer >(); 
        textSprite = text.GetComponent < SpriteRenderer >();
        textSprite.enabled = false;
        symbolSprite.enabled = false;
        playerController = FindFirstObjectByType<playerController>();
        trigger.enabled = Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)); ;
        playerCol = playerController.GetComponent< Collider2D>();
        gameObject.SetActive(Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput))); 
        
    }



    private void updateDistance()
    {
        if (shouldUpdateDistance)
        {

            if (Vector3.Distance(transform.position, playerController.transform.position) > distcanceCheck) {
                
                Time.timeScale = 1.0f;
                gameObject.SetActive(false);
            
            }



        }
    }

    // Update is called once per frame
    void Update()
    {


       
        if (hasCollided)
        {
            updateDistance();
            if (!inputStore)
            {
                inputStore = Convert.ToBoolean(playerTouch.inputDictionary[expectedInput]);
            }



            if (inputStore)
            {

                Time.timeScale = 1.0f;
                if (hasGameObject && objectCanKillPlayer && objectSpawnCol.bounds.Intersects(playerCol.bounds) && objectInstance != null)
                {

                    StartCoroutine(waitForDeath());
                }
                if (objectCanKillPlayer && !waitingForDeath)
                {
                    return;
                }

                if (!objectHasKilledPlayer)
                {
                 
                    Time.timeScale = 1.0f;
                    PlayerPrefs.SetInt(expectedInput, 1);
                    symbolSprite.enabled = false;
                    textSprite.enabled = false;
                    gameObject.SetActive(false);
                }


            }


        }
        

       


    }



  


    private IEnumerator waitForDeath()
    {
        waitingForDeath = true;
        Debug.Log("waiting for death " + waitingForDeath);
        yield return new WaitForSeconds(waitDeathTimer);
        waitingForDeath = false;
        objectHasKilledPlayer = player.IsEnabledFalse; 


    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
            if (hasGameObject)
            {
                
               objectInstance =  Instantiate(objectToSpawn,postionForobject.transform.position, Quaternion.identity) ;
                objectSpawnCol = objectInstance.GetComponent<Collider2D>() ;
            }
            shouldUpdateDistance = true;
            Time.timeScale = timeScalar;
            Debug.Log(Time.timeScale + "new time scale for " + expectedInput);
            Debug.Log("input required " + expectedInput);
            symbolSprite.enabled=true;
            textSprite.enabled=true;

        }


    }


}
