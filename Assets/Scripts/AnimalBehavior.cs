using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public float hunger = 0.0f;
    public float thirst = 0.0f;

    
    void Update()
    {
        hunger += Time.deltaTime;
        thirst += Time.deltaTime;
    }


}
