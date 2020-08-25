using UnityEngine;
using UnityEngine.AI;

public class Senses : MonoBehaviour
{
    [SerializeField] Transform visionArea = null;
    [SerializeField] float senseRadius = 1.0f;

    public Vector3 RandomSpotInSenseArea()
    {
        bool invalidPath = true;
        Vector3 focusPoint = visionArea.position;
        Vector3 possiblePosition = Vector3.zero;

        while(invalidPath)
        {
            // Select a random position inside the sightRadius of the visionArea        
            float randX = Random.Range(-1.0f, 1.0f) * senseRadius;
            float randZ = Random.Range(-1.0f, 1.0f) * senseRadius;
            possiblePosition = new Vector3(focusPoint.x + randX, 0.0f, focusPoint.z + randZ);

            invalidPath = !IsValidRandomPath(possiblePosition);
        }
        return possiblePosition;
    }

    public RaycastHit[] EntitiesInVisionArea()
    {
        return Physics.SphereCastAll(visionArea.position, senseRadius, Vector3.up, 0);
    }

    public bool IsValidRandomPath(Vector3 position)
    {
        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(position, path);
        if(path.status == NavMeshPathStatus.PathComplete && GetPathLength(path) < 2)
        {
            return true;
        }
        return false;
    }

        public bool IsValidResourcePath(Vector3 position)
    {
        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(position, path);
        if(GetPathLength(path) < 2)
        {
            return true;
        }
        return false;
    }

    float GetPathLength(NavMeshPath path)
    {
        float total = 0f;
        if(path.corners.Length < 2) return total;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            total += Vector3.Distance(path.corners[i], path.corners[i + 1]);            
        }

        return total;

    }

    void OnDrawGizmosSelected() 
    {
        // Draw sensory region in editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionArea.transform.position, senseRadius);
    }
}
