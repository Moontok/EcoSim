using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResourceType
    {
        Water,
        Food,
    }

    [SerializeField] ResourceType type = ResourceType.Water;

    public ResourceType GetResourceType()
    {
        return type;
    }
}
