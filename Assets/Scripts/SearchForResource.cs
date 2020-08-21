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
        Debug.Log("Searching...");
        if(animal.TargetObject == null)
        {
            RaycastHit[] sensedObjects = senses.EntitiesInVisionArea();

            foreach (RaycastHit hit in sensedObjects)
            {
                Resource resource = hit.transform.GetComponent<Resource>();
                if(resource != null && resource.GetResourceType() == Resource.ResourceType.Water)
                {
                    animal.TargetObject = hit.transform.gameObject;
                }
            }
        }
    }

    public void OnExit()
    {
    }
}
