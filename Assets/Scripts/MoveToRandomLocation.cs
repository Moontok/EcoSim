using UnityEngine;

internal class MoveToRandomLocation : IState
{
    public float timeStuck = 0f;

    AnimalBehavior animal = null;
    Senses senses = null;
    Movement mover = null;

    Vector3 lastPosition = Vector3.zero;


    public MoveToRandomLocation(AnimalBehavior animal, Senses senses, Movement mover)
    {
        this.animal = animal;
        this.senses = senses;
        this.mover = mover;
    }

    public void Tick()
    {
        if(Vector3.Distance(animal.transform.position, lastPosition) <= 0f)
        {
            timeStuck += Time.deltaTime;
        }
        lastPosition = animal.transform.position;
        animal.BioTickers(AnimalBehavior.Drive.Nothing);
    }

    public void OnEnter()
    {
        timeStuck = 0f;
        animal.TargetLocation = senses.RandomSpotInSenseArea();
        mover.MoveTo(animal.TargetLocation);
    }

    public void OnExit()
    {
        timeStuck = 0f;
    }
}
