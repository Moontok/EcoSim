using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public float speed = 1.0f;
    public float actionRate = 1.0f;
    
    NavMeshAgent agent = null;
    AnimalBehavior behavior = null;
    Senses senses = null;
    
    float actionTimer = 0.0f;

    void Awake() 
    {
        agent = this.GetComponent<NavMeshAgent>();
        behavior = this.GetComponent<AnimalBehavior>();
        senses = this.GetComponent<Senses>();        
    }

    void Start() 
    {
        agent.speed = speed;
    }

    void Update()
    {
        actionTimer += Time.deltaTime;

        if(actionTimer > actionRate && !agent.hasPath)
        {
            actionTimer = 0.0f;
            Move();
            
        }
    }

    void Move() {
        {
            Vector3 newDestination = senses.RandomSpotInSenseArea();

            agent.SetDestination(newDestination);
        }
    }


}
