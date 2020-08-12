using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] bool water = false;
    [SerializeField] bool food = false;

    public bool IsWater()
    {
        return water;
    }

    public bool IsFood()
    {
        return food;
    }
}
