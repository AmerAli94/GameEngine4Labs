using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : ZombieStates
{
    private GameObject FollowTarget;
    private float AttackRange = 2.0f;


    private static readonly int MovementZHash = Animator.StringToHash("MovementZ");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    public ZombieAttackState(GameObject followTarget, ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {
        FollowTarget = followTarget;
        UpdateInterval = 2.0f;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        OwnerZombie.zombieNavMesh.isStopped = true;
        OwnerZombie.zombieNavMesh.ResetPath();
        OwnerZombie.zombieAnimator.SetFloat(MovementZHash, 0.0f);
        OwnerZombie.zombieAnimator.SetBool(IsAttackingHash, true);
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        //Add damage
    }

    // Update is called once per frame
    public override void update()
    {
        OwnerZombie.transform.LookAt(FollowTarget.transform.position, Vector3.up);

        float distanceBetween = Vector3.Distance(OwnerZombie.transform.position, FollowTarget.transform.position);

        if(distanceBetween > AttackRange)
        {
            StateMachine.ChangeState(ZombieStateType.Follow);
        }
    }

    public override void exit()
    {
        base.exit();
        OwnerZombie.zombieAnimator.SetBool(IsAttackingHash, false);
    }
}
