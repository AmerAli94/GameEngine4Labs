using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowState : ZombieStates
{
    private readonly GameObject FollowTarget;
    private static readonly int MovementZhash = Animator.StringToHash("MovementZ"); 
    private const float stopDistance = 1f;
    public ZombieFollowState(GameObject followTarget, ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {
        FollowTarget = followTarget;
        UpdateInterval = 2.0f;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        OwnerZombie.zombieNavMesh.SetDestination(FollowTarget.transform.position);
    }


    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        OwnerZombie.zombieNavMesh.SetDestination(FollowTarget.transform.position);

    }

    // Update is called once per frame
    public override void update()
    {
        base.update();
        OwnerZombie.zombieAnimator.SetFloat(MovementZhash, OwnerZombie.zombieNavMesh.velocity.normalized.z);
        float distanceBetween = Vector3.Distance(OwnerZombie.transform.position, FollowTarget.transform.position);

        if (distanceBetween < stopDistance)
        {
            Debug.Log("attacking");
            StateMachine.ChangeState(ZombieStateType.Attack);
        }


    }
}
