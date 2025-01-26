using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EventObjectDescriptor : MonoBehaviour
{


    //generice base class for objects to define information that exsists across all event objects 
    [SerializeField] private int amountThatCanSpawn;



    public int AmountThatSpawn 
    {
        get { return amountThatCanSpawn; }




    }


}
