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

    StateMachine stateMachine = null;
    Movement locomotion = null;
    Senses senses = null;

    public GameObject TargetObject { get; set; }
    public Vector3 TargetLocation { get; set; }
    public Resource ResourceSearchingFor { get; set; }
    public Drive Seeking { get; set; }

    void Awake() 
    {
        locomotion = this.GetComponent<Movement>();
        senses = this.GetComponent<Senses>();

        stateMachine = new StateMachine();

        this.Seeking = Drive.Nothing;

        SearchForResource search = new SearchForResource(this, senses);
        Wander wander = new Wander(this, senses, locomotion);
        MoveToSelectedTarget moveToTarget = new MoveToSelectedTarget(this, locomotion);
        Consuming consume = new Consuming(this);

        At(search, moveToTarget, HasTarget());
        At(search, wander, HasNoTarget());
        At(moveToTarget, wander, StuckForOverASecond());
        At(moveToTarget, search, Seeking());
        At(moveToTarget, consume, ReachedTarget());
        At(wander, moveToTarget, Wandering());
        At(wander, moveToTarget, Seeking());
        At(consume, wander, DoneConsuming());
        
        stateMachine.SetState(wander);

        void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> HasTarget() => () => TargetObject != null;
        Func<bool> StuckForOverASecond() => () => moveToTarget.timeStuck > 1f;
        Func<bool> ReachedTarget() => () => TargetObject != null && Vector3.Distance(transform.position, TargetObject.transform.position) < closeEnoughToTarget;
        Func<bool> Seeking() => () => this.Seeking != Drive.Nothing;
        Func<bool> Wandering() => () => this.Seeking == Drive.Nothing;
        Func<bool> HasNoTarget() => () => TargetObject == null;
        Func<bool> DoneConsuming() => () => thirst <= 0;

    }

    void Update() 
    {
        if(this.Seeking == Drive.Nothing)
        {
            if(thirst >= thirstThreshold) this.Seeking = Drive.Water;
        }

        stateMachine.Tick();
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
