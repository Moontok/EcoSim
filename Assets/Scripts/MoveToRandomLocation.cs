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
            //Debug.Log("Stuck...");
        }
        lastPosition = animal.transform.position;
        animal.BioTickers(AnimalBehavior.Drive.Nothing);
    }

    public void OnEnter()
    {
        //Debug.Log("Wandering...");
        timeStuck = 0f;
        Vector3 target = senses.RandomSpotInSenseArea();
        animal.TargetLocation = target;
        mover.MoveTo(target);
    }

    public void OnExit()
    {
        timeStuck = 0f;
    }
}
