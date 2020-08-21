using UnityEngine;

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
        // Debug.Log("MovingTo...");
        // if(Vector3.Distance(animal.transform.position, lastPosition) <= 0f)
        // {
        //     timeStuck += Time.deltaTime;
        //     Debug.Log("Stuck...");
        // }
        // lastPosition = animal.transform.position;
        animal.BioTickers(AnimalBehavior.Drive.Nothing);
    }

    public void OnEnter()
    {
        timeStuck = 0f;
        if(animal.TargetObject != null)
        {
            mover.MoveTo(animal.TargetObject.transform.position);
        }
        else
        {
            mover.MoveTo(animal.TargetLocation);
        }
    }

    public void OnExit()
    {
    }
}
