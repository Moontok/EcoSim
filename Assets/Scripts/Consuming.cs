using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Consuming : IState
{
    AnimalBehavior animal = null;

    public Consuming(AnimalBehavior animal)
    {
        this.animal = animal;
    }

    public void Tick()
    {
        Debug.Log("Drinking...");
        animal.Drink();
        animal.BioTickers(AnimalBehavior.Drive.Water);
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}
