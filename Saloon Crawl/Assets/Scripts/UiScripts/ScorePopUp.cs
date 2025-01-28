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

    private Vector2 randomDirection;
    private Vector3 startPos;
   
    

    void Start()
    {
        randomDirection.x = Random.Range(-1.0f, 1.0f);
        randomDirection.y = Random.Range(-1.0f, 1.0f);
        currentTimer = 0.0f; 




    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimer < popUpTime )
        {
            currentTimer += Time.deltaTime;
            transform.Translate((Vector3)randomDirection * speed * Time.deltaTime);
            return;
        }
        Destroy(this.gameObject);



    } 


    


    


}
