using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTarget : MonoBehaviour
{
    
    [SerializeField] Transform targetMarker = null;

    AnimalBehavior animal = null;

    void Awake() 
    {
        animal = this.GetComponent<AnimalBehavior>();
    }

    void Update() 
    {        
        if(animal.TargetObject != null)
        {
            targetMarker.transform.position = animal.TargetObject.transform.position + new Vector3(0,0.3f,0);
        }
    }


}
