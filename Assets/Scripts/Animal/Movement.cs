using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1.0f;
    
    NavMeshAgent agent = null;
    
    void Awake() 
    {
        agent = this.GetComponent<NavMeshAgent>();      
    }

    void Start() 
    {
        agent.speed = movementSpeed;
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

    public bool CheckPathLength(GameObject target)
    {        
        if(agent.hasPath)
        {
            if(agent.path.corners.Length > 4)
            {
                target = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void EnableMotion(bool state)
    {
        agent.enabled = state;
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
}
