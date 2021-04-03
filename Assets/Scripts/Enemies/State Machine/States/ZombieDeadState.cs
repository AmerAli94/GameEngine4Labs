using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeadState : ZombieStates
{

    private static readonly int MovementZHash = Animator.StringToHash("MovementZ");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    public ZombieDeadState(ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        OwnerZombie.zombieNavMesh.isStopped = true;
        OwnerZombie.zombieNavMesh.ResetPath();
        OwnerZombie.zombieAnimator.SetFloat(MovementZHash, 0);
        OwnerZombie.zombieAnimator.SetBool(IsDeadHash, true);
    }

    public override void exit()
    {
        base.exit();

        OwnerZombie.zombieNavMesh.isStopped = false;
        OwnerZombie.zombieAnimator.SetBool(IsDeadHash, false);
    }
}
