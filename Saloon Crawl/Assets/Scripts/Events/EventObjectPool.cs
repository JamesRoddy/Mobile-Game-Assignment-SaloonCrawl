using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectPool : MonoBehaviour
{



    // main pool class used for event objects 
    private List<GameObject> eventObjectPool = new List<GameObject>();
    private int currentPoolPointer = 0;
 

    
    public void setValues(List<GameObject> eventObjects)
    {

        for (int i = 0; i < eventObjects.Count; i++) 
        { 
            GameObject instance =  Instantiate(eventObjects[i],Vector3.zero,Quaternion.identity);
            int amount = instance.GetComponent<EventObjectDescriptor>().AmountThatSpawn;
            instance.SetActive(false);
            eventObjectPool.Add(instance);
            for(int j  = 0; j < amount - 1; j++)
            {
                
                eventObjectPool.Add(Instantiate(instance,Vector3.zero,Quaternion.identity));
               
            }



        }






    }


    public bool hasAvailableObject()
    {
        if(currentPoolPointer == eventObjectPool.Count)
        {

            currentPoolPointer = 0;
        }

        return !eventObjectPool[currentPoolPointer].activeSelf;



    }
    public GameObject requestAvaialbeObject()
    {

   

        return eventObjectPool[currentPoolPointer++];



    }
   





}
