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

            string resourceToSeek = TargetResource();

            if(resourceToSeek != "")
            {
                Resource resource = senses.SeekResource(resourceToSeek);

                if(resource != null)
                {                
                    float distance = Vector3.Distance(this.transform.position, resource.transform.position);
                    if(distance <= consumeDistance)
                    {
                        ConsumingResource(resourceToSeek);
                    }
                }
            }

            !locomotion.HavePath()

            Vector3 target = TargetArea(); 


            locomotion.Move(target);            
        }
        
        hunger += Time.deltaTime;
        thirst += Time.deltaTime;
    }

    void ConsumingResource(string resource)
    { 
        if(resource == "water")
        {
            thirst -= Time.deltaTime;
        }
        else if(resource == "food")
        {
            hunger -= Time.deltaTime;
        }
    }

    Vector3 TargetRandomLocation()
    {
        return senses.RandomSpotInSenseArea();
    }

    string TargetResource()
    {

        if( thirst > thirstThreshold && thirst >= hunger)
        {
            print("Seeking water!");
            return "water";
        }
        else if(hunger > hungerThreshold && hunger > thirst)
        {
            print("Seeking food!");
            return "food";
        }
        return "";
    }

}
