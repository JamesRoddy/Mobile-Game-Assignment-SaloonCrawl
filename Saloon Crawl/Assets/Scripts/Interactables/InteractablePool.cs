using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePool : MonoBehaviour
{


    private int maxSpawnCount = 0;
    private bool hasDeffered = false;
    private GameObject defferedInteractable;
    // interactable pool has a random object chosen to be activated therefore to keep track of this the objects that are active are stored in the active list and the objects that are ianctive are stored in the pool
    private List<GameObject> pool = new List<GameObject>();
    private List<GameObject> active = new List<GameObject>();

    public void setValues(List<GameObject> interactables, int  terrainMultipler)
    {
        // instantiate on start based on amout needed from the terrain the pool is assigned  to 
      for (int i = 0; i < interactables.Count; i++)
        {
            GameObject interactable = Instantiate(interactables[i], Vector3.zero, Quaternion.identity);
            int count = interactable.GetComponent<Interactable>().NumberThatCanSpawn*terrainMultipler;
            interactable.SetActive(false);
            pool.Add(interactable);
            for (int j = 0; j < count - 1; j++)
            {
                GameObject interactableObj = Instantiate(interactable, Vector3.zero, Quaternion.identity);
                interactableObj.SetActive(false);
                pool.Add(interactableObj);
            }


        }
        maxSpawnCount = pool.Count;


    }

    public void updateActiveObjects()
    {
        for (int i = 0; i < active.Count; i++)
        {
     
            if (!(active[i].activeSelf))
            {
                pool.Add(active[i]);
                active.Remove(active[i]);
            }
        }





    }
    public GameObject getRandomAvailableObject()
    {

        if(hasDeffered && active.Count>0) // if the pool has run out of objects and need to send one to the inetractale spawn manager 
        {

            if (active[0].activeSelf == false) // check if the first object added to the active list is deactived as alll objects are deactived when they leave the camera therefore this object is the most likley to have been deactivated  
            {
                hasDeffered = false;
                pool.Add(active[0]); // shif the object into the pool to be used 
                active.Remove(active[0]);
                

                
            }



        }

        if (pool.Count > 0) // if we still have objects in the pool 
        {
            GameObject interactable;
            defferedInteractable = null;
            hasDeffered = false;
            interactable = pool[Random.Range(0, pool.Count)];
 
            active.Add(interactable);
            pool.Remove(interactable);
            return interactable;
        }


        if (defferedInteractable == null)
        {


            defferedInteractable = active[Random.Range(0, active.Count)];
            defferedInteractable.GetComponent<Interactable>().DefferedSpawn = true;
        }


        return defferedInteractable;




    }

    public bool hasAvailableObject(TerrainType currentTerrain)
    {
        hasDeffered = pool.Count == 0 && !currentTerrain.hasMetInteractableRequest() ? true : false;
 

        return pool.Count > 0 || hasDeffered;

    }

    public bool HasDeffered
    {
        get { return hasDeffered; }

    }
}

