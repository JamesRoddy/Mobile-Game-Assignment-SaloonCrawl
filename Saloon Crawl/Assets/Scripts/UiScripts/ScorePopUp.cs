using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    // Start is called before the first frame update 


    
    [SerializeField] private  float popUpTime;

    [SerializeField] private float speed;
 

    private float currentTimer = 0.0f;

    private bool displayScore = false;
    private Vector2 randomDirection;
    private Vector3 startPos;
   
    

    void Start()
    {
        randomDirection.x = Random.Range(-1.0f, 1.0f);
        randomDirection.y = Random.Range(-1.0f, 1.0f);
        currentTimer = 0.0f;

        Debug.Log("SCORE POP UP START  instantiating score pop up  random dir x "+ randomDirection.x+" random dir  y  "+randomDirection.y +" current timer "+currentTimer);


    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimer < popUpTime )
        {
            currentTimer += Time.deltaTime;
            Debug.Log("SCORE POP UP moving for " +currentTimer);
            transform.Translate((Vector3)randomDirection * speed * Time.deltaTime);
            return;
        }
        Debug.Log(" SCORE POP UP destroying score pop up");
        Destroy(this.gameObject);



    } 


    


    


}
