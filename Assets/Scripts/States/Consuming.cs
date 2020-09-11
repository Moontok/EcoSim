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
        Drive drive = animal.Seeking;

        if(drive == Drive.Water)
        {
            animal.Drink();
            animal.BioTickers(drive);
        }
        else if(drive == Drive.Food)
        {
            animal.Eat();
            animal.BioTickers(drive);
        }
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
        animal.Seeking = Drive.Nothing;
        animal.ParticalSystemControllerSwitch(false);
        animal.SetNormalMaterial();
    }
}
