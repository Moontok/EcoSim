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

    [SerializeField] Material normalMaterial = null;
    [SerializeField] float thirst = 0;
    [SerializeField] float thirstThreshold = 10;
    [SerializeField] float thirstRate = 1f;
    [SerializeField] float drinkRate = 2f;
    [SerializeField] Material thirstMaterial = null;
    [SerializeField] float stuckTime = 0.5f;
    [SerializeField] float closeEnoughToTarget = 1f;
    [SerializeField] float closeEnoughToLoc = 0.1f;
    [SerializeField] GameObject particalSystemController = null;
    [SerializeField] GameObject body = null;

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

        RandomlySetAttributes();


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
        Func<bool> CantReachTarget() => () => moveToTarget.timeStuck > stuckTime;
        Func<bool> CantReachLocation() => () => randomLocation.timeStuck > stuckTime;
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
            if(thirst >= thirstThreshold)
            {
                this.Seeking = Drive.Water;                
                body.GetComponent<MeshRenderer>().material = thirstMaterial;
            }
        }

        stateMachine.Tick();
    }

    public void DoneSeeking()
    {
        this.Seeking = Drive.Nothing;
    }

    public void Drink()
    {
        this.thirst -= Time.deltaTime * drinkRate;
    }

    public void BioTickers(Drive bio)
    {
        if(bio != Drive.Water)
        {
            thirst += Time.deltaTime * thirstRate;
        }
    }

    public void ParticalSystemControllerSwitch(bool state)
    {
        particalSystemController.SetActive(state);
    }

    public void SetNormalMaterial()
    {
        body.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    private void RandomlySetAttributes()
    {        
        thirstThreshold = UnityEngine.Random.Range(5, 15);
        thirstRate = UnityEngine.Random.Range(0.5f, 2f);
        drinkRate = UnityEngine.Random.Range(1f, 5f);
        stuckTime = UnityEngine.Random.Range(0.1f, 1f);
    }
}
