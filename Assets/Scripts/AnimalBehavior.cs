using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public enum Drive
    {
        Nothing,
        Water,
    }

    [SerializeField] float thirst = 0;
    [SerializeField] float thirstThreshold = 10;
    [SerializeField] float closeEnoughToTarget = 1f;
    [SerializeField] float closeEnoughToLoc = 0.1f;
    [SerializeField] GameObject locationPoint = null;

    StateMachine stateMachine = null;
    Movement locomotion = null;
    Senses senses = null;

    public GameObject TargetObject { get; set; }
    public Vector3 TargetLocation { get; set; }
    public Drive Seeking { get; set; }

    void Awake() 
    {
        locomotion = this.GetComponent<Movement>();
        senses = this.GetComponent<Senses>();

        stateMachine = new StateMachine();

        this.Seeking = Drive.Nothing;

        SearchForResource search = new SearchForResource(this, senses);
        MoveToRandomLocation randomLocation = new MoveToRandomLocation(this, senses, locomotion);
        MoveToSelectedTarget moveToTarget = new MoveToSelectedTarget(this, locomotion);
        Consuming consume = new Consuming(this);
        Idle idle = new Idle(this);

        At(search, moveToTarget, HasTarget());
        At(search, randomLocation, HasNoTarget());
        At(moveToTarget, randomLocation, CantReachTarget());
        At(moveToTarget, consume, ReachedTarget());
        //At(randomLocation, search, HasDrive());
        At(randomLocation, moveToTarget, HasTarget());
        At(randomLocation, idle, ReachedLocation());
        At(randomLocation, idle, CantReachLocation());
        At(consume, idle, DoneConsuming());
        At(idle, randomLocation, HasNoDrive());
        At(idle, search, HasDrive());
        
        stateMachine.SetState(randomLocation);

        void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);

        Func<bool> HasTarget() => () => TargetObject != null;
        Func<bool> HasNoTarget() => () => TargetObject == null;
        Func<bool> CantReachTarget() => () => moveToTarget.timeStuck > 1f;
        Func<bool> CantReachLocation() => () => randomLocation.timeStuck > 1f;
        Func<bool> ReachedTarget() => () => TargetObject != null && Vector3.Distance(this.transform.position, TargetObject.transform.position) < closeEnoughToTarget;
        Func<bool> ReachedLocation() => () => TargetLocation != null && Vector3.Distance(this.transform.position, TargetLocation) < closeEnoughToLoc;
        Func<bool> HasDrive() => () => this.Seeking != Drive.Nothing;
        Func<bool> DoneConsuming() => () => thirst <= 0;
        Func<bool> HasNoDrive() => () => this.Seeking == Drive.Nothing;

    }

    void Update() 
    {
        if(this.Seeking == Drive.Nothing)
        {
            if(thirst >= thirstThreshold) this.Seeking = Drive.Water;
        }

        stateMachine.Tick();
        locationPoint.transform.position = this.TargetLocation;
    }

    public void DoneSeeking()
    {
        this.Seeking = Drive.Nothing;
    }

    public void Drink()
    {
        Debug.Log("Drinking...");
        this.thirst -= Time.deltaTime;
    }

    public void BioTickers(Drive bio)
    {
        if(bio != Drive.Water)
        {
            thirst += Time.deltaTime;
        }
    }
}
