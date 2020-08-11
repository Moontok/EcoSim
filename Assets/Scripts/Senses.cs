using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{
    public GameObject visionArea = null;
    public float senseRadius = 1.0f;

    public Vector3 RandomSpotInSenseArea()
    {
        // Select a random position inside the sightRadius of the visionArea        
        float randX = Random.Range(-1.0f, 1.0f) * senseRadius;
        float randZ = Random.Range(-1.0f, 1.0f) * senseRadius;

        Vector3 focusPoint = visionArea.transform.position;
        return new Vector3(focusPoint.x + randX, 0.0f, focusPoint.z + randZ);
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(visionArea.transform.position, senseRadius);
    }
}
