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
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(this.transform.position, position, NavMesh.AllAreas, path);
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected() 
    {
        // Draw sensory region in editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionArea.transform.position, senseRadius);
    }
}
