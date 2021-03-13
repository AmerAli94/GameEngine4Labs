﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;



namespace Character
{

    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float WalkSpeed;
        [SerializeField] private float RunSpeed;
        [SerializeField] private float JumpForce;

        private Animator PlayerAnimator;
        private PlayerController PlayerController;
        private Rigidbody PlayerRigidBody;
        private Transform PlayerTransform;
        private NavMeshAgent PlayerNavMesh;

        private Vector2 InputVector = Vector2.zero;
        private Vector3 MoveDirection = Vector3.zero;


        //animator hashes
        private readonly int MovementXHash = Animator.StringToHash("MovementX");
        private readonly int MovementZHash = Animator.StringToHash("MovementZ");
        private readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        private readonly int IsJumpingHash = Animator.StringToHash("IsJumping");




        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerRigidBody = GetComponent<Rigidbody>();
            PlayerNavMesh = GetComponent<NavMeshAgent>();

            PlayerTransform = transform;
        }
        private void Start()
        {
            Debug.Log("Start");
        }

        private void Update()
        {
            if (PlayerController.IsJumping) return;

            if (!(InputVector.magnitude > 0)) MoveDirection = Vector3.zero;

            MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

            float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;

            Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);

            //transform.position += movementDirection;
            // PlayerTransform.position += movementDirection;
            PlayerNavMesh.Move(movementDirection);
        }
        public void OnMovement(InputValue value)
        {
            InputVector = value.Get<Vector2>();

            PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
            PlayerAnimator.SetFloat(MovementZHash, InputVector.y);

        }

        public void OnRun(InputValue button)
        {
            PlayerController.IsRunning = button.isPressed;
            PlayerAnimator.SetBool(IsRunningHash, button.isPressed);

        }

        public void OnJump(InputValue button)
        {
            PlayerController.IsJumping = true;
            PlayerAnimator.SetBool(IsJumpingHash, true);
            PlayerNavMesh.enabled = false;
            PlayerRigidBody.AddForce((transform.up + MoveDirection) * JumpForce, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Ground") && !PlayerController.IsJumping) return;
            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);
            PlayerNavMesh.enabled = true;
        }
    }
}
