﻿using UnityEngine.AI;

internal class Idle : IState
{
    AnimalBehavior animal = null;


    public Idle(AnimalBehavior animal)
    {
        this.animal = animal;
    }

    public void Tick()
    {
        animal.BioTickers(Drive.Nothing);
    }

    public void OnEnter()
    {
        animal.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void OnExit()
    {
        animal.GetComponent<NavMeshAgent>().enabled = true;
    }
}
