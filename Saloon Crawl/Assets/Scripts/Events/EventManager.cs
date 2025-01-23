using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> eventObjectHolders;

    List<Event> events =  new List<Event>();
    List<Event> potential = new List<Event>();
    List<Event> inactive = new List<Event>();

    Event currentEvent = null;
    private float genericCoolDown = 0.0f;





    void Start()
    {

        foreach (GameObject obj in eventObjectHolders) 
        {
            EventObjectPool pool =  gameObject.AddComponent<EventObjectPool>();
            Event currentEvent = obj.GetComponent<Event>();
            pool.setValues(currentEvent.EventObjects); 
            currentEvent.ObjectPool = pool;

            currentEvent.AttachedQueue = events;
         
        
        }




    }

    // Update is called once per frame
    void Update()
    {
        checkForEvent();
        if ( currentEvent != null && genericCoolDown <= 0.0f)
        {

            if (!currentEvent.HasFinished())
            {

                currentEvent.Fire();
                return;
            } 






     

        }

        
        genericCoolDown -= Time.deltaTime;


        
    }



    private void checkForEvent()
    {

        
        if (events.Count > 0 && currentEvent ==null)
        {
            currentEvent = events[0];
      
        }



    }



}
