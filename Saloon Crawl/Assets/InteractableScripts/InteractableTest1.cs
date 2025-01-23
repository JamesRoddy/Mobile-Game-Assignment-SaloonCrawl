using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest1 : Interactable
{
    // Start is called before the first frame update
    BoxCollider2D boxcolllider;
 
    Camera cam;
    public override void interactableStart()
    {

        Debug.Log("interactable 1 start");
        cam = Camera.main;
        boxcolllider = GetComponent<BoxCollider2D>();
    }


    public override void interactableUpdate()
    {

        float screenX = cam.WorldToViewportPoint(transform.position).x;

        if (screenX < 0.0f)
        {
            gameObject.SetActive(false);
        }


    }

}
