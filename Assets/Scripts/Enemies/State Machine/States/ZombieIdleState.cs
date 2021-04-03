using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : ZombieStates
{
    public ZombieIdleState(ZombieComponent zombie, StateMachine stateMachine) : base(zombie,stateMachine)
    {
        OwnerZombie = zombie;
    }

    public override void Start()
    {
        base.Start();
        OwnerZombie.zombieNavMesh.isStopped = true;
        OwnerZombie.zombieNavMesh.ResetPath();
        OwnerZombie.zombieAnimator.SetFloat("MovementZ", 0.0f);
    }
}
