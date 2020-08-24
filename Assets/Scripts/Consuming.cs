using UnityEngine;
using UnityEngine.AI;

internal class Consuming : IState
{
    AnimalBehavior animal = null;

    public Consuming(AnimalBehavior animal)
    {
        this.animal = animal;
    }

    public void Tick()
    {
        animal.Drink();
        animal.BioTickers(AnimalBehavior.Drive.Water);
    }

    public void OnEnter()
    {
        animal.GetComponent<NavMeshAgent>().enabled = false;
        animal.ParticalSystemControllerSwitch(true);
    }

    public void OnExit()
    {
        animal.GetComponent<NavMeshAgent>().enabled = true;
        animal.TargetObject = null;
        animal.Seeking = AnimalBehavior.Drive.Nothing;
        animal.ParticalSystemControllerSwitch(false);
        animal.SetNormalMaterial();
    }
}
