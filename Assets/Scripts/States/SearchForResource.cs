using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

internal class SearchForResource : IState
{
    public float timeStuck = 0f;

    AnimalBehavior animal = null;
    Senses senses = null;
    Vector3 lastPosition = Vector3.zero;


    public SearchForResource(AnimalBehavior animal, Senses senses)
    {
        this.animal = animal;
        this.senses = senses;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        Drive drive = animal.Seeking;
        
        if(animal.TargetObject == null)
        {
            RaycastHit[] sensedObjects = senses.EntitiesInVisionArea();
            float closest = Mathf.Infinity;

            foreach (RaycastHit hit in sensedObjects)
            {
                Resource resource = hit.transform.GetComponent<Resource>();
                
                if(resource != null)
                {
                    if(drive == Drive.Water)
                    {
                        if(resource.GetResourceType() == Resource.ResourceType.Water)
                        {
                            float distance = Vector3.Distance(animal.transform.position, resource.transform.position);
                            if(distance < closest)
                            {
                                closest = distance;
                                animal.TargetObject = hit.transform.gameObject;
                            }
                        }
                    }
                    else if (drive == Drive.Food)
                    {
                        if(resource.GetResourceType() == Resource.ResourceType.Food)
                        {
                            float distance = Vector3.Distance(animal.transform.position, resource.transform.position);
                            if(distance < closest)
                            {
                                closest = distance;
                                animal.TargetObject = hit.transform.gameObject;
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnExit()
    {
    }
}
