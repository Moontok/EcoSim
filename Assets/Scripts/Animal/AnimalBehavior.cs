using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] float size = 0.1f;
    [SerializeField] Material normalMaterial = null;
    [SerializeField] Material deathMaterial = null;
    [SerializeField] float ageSeed = 300f;
    [SerializeField] float age = 0f;
    [SerializeField] float lifeSpan = 360f;
    [SerializeField] BioBar ageBar = null;
    [SerializeField] float decayTime = 10f;

    [SerializeField] float thirstSeed = 40f;
    [SerializeField] float thirst = 0f;
    [SerializeField] float thirstThreshold = 10f;
    [SerializeField] float maxThirst = 50f;
    [SerializeField] BioBar thirstBar = null;
    [SerializeField] float thirstRate = 1f;
    [SerializeField] float drinkRate = 2f;
    [SerializeField] Material thirstMaterial = null;

    [SerializeField] float hungerSeed = 40f;
    [SerializeField] float hunger = 0f;
    [SerializeField] float hungerThreshold = 10f;
    [SerializeField] float maxHunger = 60f;
    [SerializeField] BioBar hungerBar = null;
    [SerializeField] float hungerRate = 1f;
    [SerializeField] float eatRate = 2f;
    [SerializeField] Material hungerMaterial = null;

    [SerializeField] float stuckTime = 0.5f;
    [SerializeField] float closeEnoughToTarget = 1f;
    [SerializeField] float closeEnoughToLoc = 0.1f;
    [SerializeField] GameObject particalSystemController = null;
    [SerializeField] GameObject body = null;
    [SerializeField] GameObject bodyMesh = null;

    StateMachine stateMachine = null;
    Movement locomotion = null;
    Senses senses = null;
    float decay = 0f;
    bool alive = true;

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
        At(moveToTarget, randomLocation, PathToLong());
        At(moveToTarget, consume, ReachedTarget());
        At(randomLocation, moveToTarget, HasTarget());
        At(randomLocation, idle, ReachedLocation());
        At(randomLocation, idle, CantReachLocation());
        At(consume, idle, DoneConsuming());
        At(idle, randomLocation, HasNoDrive());
        At(idle, search, HasDrive());
        
        stateMachine.SetState(idle);

        void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);

        Func<bool> HasTarget() => () => TargetObject != null;
        Func<bool> HasNoTarget() => () => TargetObject == null;
        Func<bool> CantReachTarget() => () => moveToTarget.timeStuck > stuckTime;
        Func<bool> CantReachLocation() => () => randomLocation.timeStuck > stuckTime;
        Func<bool> ReachedTarget() => () => TargetObject != null && Vector3.Distance(this.transform.position, TargetObject.transform.position) < closeEnoughToTarget;
        Func<bool> ReachedLocation() => () => TargetLocation != null && Vector3.Distance(this.transform.position, TargetLocation) < closeEnoughToLoc;
        Func<bool> HasDrive() => () => this.Seeking != Drive.Nothing;
        Func<bool> DoneConsuming() => () => thirst <= 0 || hunger <= 0;
        Func<bool> HasNoDrive() => () => this.Seeking == Drive.Nothing;
        Func<bool> PathToLong() => () => locomotion.CheckPathLength(TargetObject);
    }

    void Start() 
    {
        ageBar.SetMaxAmount(lifeSpan);
        thirstBar.SetMaxAmount(maxThirst);
        hungerBar.SetMaxAmount(maxHunger);
        body.transform.localScale = new Vector3(size, size, size);
    }

    void Update() 
    {
        if(alive)
        {
            stateMachine.Tick();
            LifeChecks();
        }
        else
        {
            decay += Time.deltaTime;
            if(decay >= decayTime) Destroy(this.gameObject);
        }
    }

    public void DoneSeeking()
    {
        this.Seeking = Drive.Nothing;
    }

    public void Drink()
    {
        thirst -= Time.deltaTime * drinkRate;
        thirstBar.SetAmount(thirst);
    }

    public void Eat()
    {
        hunger -= Time.deltaTime * eatRate;
        hungerBar.SetAmount(hunger);
    }

    public void BioTickers(Drive bio)
    {
        float time = Time.deltaTime;
        if(bio != Drive.Water)
        {
            thirst += time * thirstRate;
            thirstBar.SetAmount(thirst);
        }
        if(bio != Drive.Food)
        {
            hunger += time * hungerRate;
            hungerBar.SetAmount(hunger);
        }

        age += time;
        size = Math.Max(0.1f, age/lifeSpan);
        body.gameObject.transform.localScale = new Vector3(size, size, size);
        ageBar.SetAmount(age);
    }

    public void ParticalSystemControllerSwitch(bool state)
    {
        particalSystemController.SetActive(state);
    }

    public void SetNormalMaterial()
    {
        bodyMesh.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    private void RandomlySetAttributes()
    {   
        lifeSpan = UnityEngine.Random.Range(ageSeed * .5f, ageSeed * 1.5f); 
        locomotion.SetSpeed(UnityEngine.Random.Range(0.5f, 2f));
        stuckTime = UnityEngine.Random.Range(0.1f, 1f);

        maxThirst = UnityEngine.Random.Range(thirstSeed * .5f, thirstSeed * 2);
        thirstThreshold = UnityEngine.Random.Range(thirstSeed * .2f, thirstSeed * .3f);
        thirstRate = UnityEngine.Random.Range(0.5f, 2f);
        drinkRate = UnityEngine.Random.Range(5f, 10f);

        maxHunger = UnityEngine.Random.Range(hungerSeed * .5f, hungerSeed * 2);
        hungerThreshold = UnityEngine.Random.Range(hungerSeed * .2f, hungerSeed * .3f);
        hungerRate = UnityEngine.Random.Range(0.5f, 2f);
        eatRate = UnityEngine.Random.Range(5f, 10f);
    }

    private void Death()
    {
        locomotion.EnableMotion(false);
        alive = false;
        bodyMesh.GetComponent<MeshRenderer>().material = deathMaterial;
    }

    private void LifeChecks()
    {
        if(this.Seeking == Drive.Nothing)
        {
            if(Seeking == Drive.Nothing)
            {
                if(thirst >= thirstThreshold)
                {
                    Seeking = Drive.Water;                
                    bodyMesh.GetComponent<MeshRenderer>().material = thirstMaterial;
                }
                
                else if(hunger >= hungerThreshold)
                {
                    Seeking = Drive.Food;                
                    bodyMesh.GetComponent<MeshRenderer>().material = hungerMaterial;
                }
            }

        }

        if(age >= lifeSpan) Death();
        if(thirst >= maxThirst) Death();
        if(hunger >= maxHunger) Death();
    }
}
