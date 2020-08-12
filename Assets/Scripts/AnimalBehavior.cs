using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] float hunger = 0.0f;
    [SerializeField] float hungerThreshold = 10.0f;
    [SerializeField] float thirst = 0.0f;
    [SerializeField] float thirstThreshold = 10.0f;
    [SerializeField] float consumeDistance = 1.0f;
    [SerializeField] float actionRate = 1.0f;    
    [SerializeField] float actionTimer = 0.0f;

    Movement locomotion = null;
    Senses senses = null;
    bool consuming = false;
    bool searchingForFood = false;
    bool searchingForWater = false;

    void Awake()
    {
        locomotion = this.GetComponent<Movement>();
        senses = this.GetComponent<Senses>();
    }

    void Update()
    {
        actionTimer += Time.deltaTime;

        if(actionTimer > actionRate)
        {            
            actionTimer = 0.0f;
            ConditionState();
            if(consuming)
            {
                //ConsumeResource();
            }
            else
            {                
                locomotion.Move(TargetRandomLocation());
            }
        }
        
        hunger += Time.deltaTime;
        thirst += Time.deltaTime;
    }

    void ConsumeResource(Resource resource)
    { 
        if(resource.IsWater())
        {
            thirst -= Time.deltaTime;
        }
        if(resource.IsFood())
        {
            hunger -= Time.deltaTime;
        }
    }

    Vector3 TargetRandomLocation()
    {
        return senses.RandomSpotInSenseArea();
    }

    void ConditionState()
    {
        if( thirst > thirstThreshold && thirst >= hunger && !searchingForWater)
        {
            print("Seeking water!");
            searchingForWater = true;
        }
        else if(hunger > hungerThreshold && hunger > thirst && !searchingForFood)
        {
            print("Seeking food!");
            searchingForFood = true;
        }
    }

}
