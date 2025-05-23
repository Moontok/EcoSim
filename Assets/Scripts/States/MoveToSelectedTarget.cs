﻿using UnityEngine;

internal class MoveToSelectedTarget : IState
{
    public float timeStuck = 0f;

    AnimalBehavior animal = null;
    Movement mover = null;

    Vector3 lastPosition = Vector3.zero;

    public MoveToSelectedTarget(AnimalBehavior animal, Movement mover)
    {
        this.animal = animal;
        this.mover = mover;
    }

    public void Tick()
    {
        if(Vector3.Distance(animal.transform.position, lastPosition) <= 0f)
        {
            timeStuck += Time.deltaTime;
        }
        lastPosition = animal.transform.position;
        animal.BioTickers(Drive.Nothing);
    }

    public void OnEnter()
    {
        timeStuck = 0f;
        mover.MoveTo(animal.TargetObject.transform.position);
    }

    public void OnExit()
    {
        timeStuck = 0;
    }
}
