using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Wander : IState
{
    public float timeStuck = 0f;

    AnimalBehavior animal = null;
    Senses senses = null;
    Movement mover = null;

    float closeEnoughToTargetLoc = 0.1f;
    Vector3 lastPosition = Vector3.zero;


    public Wander(AnimalBehavior animal, Senses senses, Movement mover)
    {
        this.animal = animal;
        this.senses = senses;
        this.mover = mover;
    }

    public void Tick()
    {
        Debug.Log("Wandering...");
        animal.BioTickers(AnimalBehavior.Drive.Nothing);
    }

    public void OnEnter()
    {
        Vector3 target = senses.RandomSpotInSenseArea();
        animal.TargetLocation = target;
    }

    public void OnExit()
    {

    }
}
