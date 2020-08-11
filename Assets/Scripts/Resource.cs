using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    [SerializeField] bool food = false;
    [SerializeField] bool water = false;

    public bool IsFood()
    {
        return food;
    }

    public bool IsWater()
    {
        return water;
    }

}
