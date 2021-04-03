using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]

public class ZombieComponent : MonoBehaviour
{

    public NavMeshAgent zombieNavMesh { get; private set; }
    public Animator zombieAnimator { get; private set; }

    public StateMachine stateMachine { get; private set; }

    public GameObject FollowTarget;

    [SerializeField] private bool debug;

    private void Awake()
    {
        zombieNavMesh = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
    }
    // Start is called before the first frame update
    void Start()
    {
     if(debug)
        {
            Initialize(FollowTarget);
        }
    }
    public void Initialize(GameObject followTarget)
    {
        FollowTarget = followTarget;

        ZombieIdleState idleState = new ZombieIdleState(this, stateMachine);
        stateMachine.AddState(ZombieStateType.Idle, idleState);

        ZombieFollowState followState = new ZombieFollowState(followTarget, this, stateMachine);
        stateMachine.AddState(ZombieStateType.Follow, followState);


        ZombieAttackState attackState = new ZombieAttackState(followTarget, this, stateMachine);
        stateMachine.AddState(ZombieStateType.Attack, attackState);

        ZombieDeadState deadState = new ZombieDeadState(this, stateMachine);
        stateMachine.AddState(ZombieStateType.Dead, deadState);


        stateMachine.Initialize(ZombieStateType.Follow);
     }

}
