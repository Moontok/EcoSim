using UnityEngine;

public class Senses : MonoBehaviour
{
    [SerializeField] Transform visionArea = null;
    [SerializeField] float senseRadius = 1.0f;

    public Vector3 RandomSpotInSenseArea()
    {
        // Select a random position inside the sightRadius of the visionArea        
        float randX = Random.Range(-1.0f, 1.0f) * senseRadius;
        float randZ = Random.Range(-1.0f, 1.0f) * senseRadius;

        Vector3 focusPoint = visionArea.position;
        return new Vector3(focusPoint.x + randX, 0.0f, focusPoint.z + randZ);
    }

    public RaycastHit[] EntitiesInVisionArea()
    {
        return Physics.SphereCastAll(visionArea.position, senseRadius, Vector3.up, 0);
    }

    void OnDrawGizmosSelected() 
    {
        // Draw sensory region in editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionArea.transform.position, senseRadius);
    }
}
