using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// inetractables (obstacals,etc) should inherit from this and use the abstract methods for update and start
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected int numberThatCanSpawn; // set in inspector not in code 
    private bool defferedSpawn = false;// tells pool to wait for the object to deactivate then spawn again 
    void Start()
    {
        interactableStart();
    }

    public abstract void interactableStart();

    public abstract void interactableUpdate();





    void Update()
    {

        interactableUpdate();

    }



    public bool DefferedSpawn { 
        
        get { return defferedSpawn; } 
        set { defferedSpawn = value; }  
    
    }


    public int NumberThatCanSpawn
    { 
       get { return numberThatCanSpawn; } 
       set { numberThatCanSpawn = value; }
    }
}
