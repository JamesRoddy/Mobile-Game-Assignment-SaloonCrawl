using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest1 : Interactable
{
    // Start is called before the first frame update
    BoxCollider2D boxcolllider;
    float maxTimer = 1.5f;
    float t = 0.0f;
    public override void interactableStart()
    {

        Debug.Log("interactable 1 start");

        boxcolllider = GetComponent<BoxCollider2D>();
    }


    public override void interactableUpdate()
    {
        t += Time.deltaTime;
        if (t >= maxTimer)
        {
        
            t = 0.0f;
            gameObject.SetActive(false);
        }


    }

}
