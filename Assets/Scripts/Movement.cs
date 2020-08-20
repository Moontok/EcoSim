using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    
    NavMeshAgent agent = null;
    
    void Awake() 
    {
        agent = this.GetComponent<NavMeshAgent>();      
    }

    void Start() 
    {
        agent.speed = speed;
    }

    public void MoveTo(Vector3 destination) {
        {

            agent.SetDestination(destination);
        }
    }

    public bool HavePath()
    {
        return agent.hasPath;
    }


}
