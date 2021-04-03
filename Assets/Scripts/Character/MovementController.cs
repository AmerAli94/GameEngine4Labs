using System.Collections;
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
        [SerializeField] private float MoveDirectionBuffer = 2f;

        private Animator PlayerAnimator;
        private PlayerController PlayerController;
        private Rigidbody PlayerRigidBody;
        private Transform PlayerTransform;
        private NavMeshAgent PlayerNavMesh;

        private Vector2 InputVector = Vector2.zero;
        private Vector3 MoveDirection = Vector3.zero;
        private Vector3 NextPositionCheck;

        //animator hashes
        private readonly int MovementXHash = Animator.StringToHash("MovementX");
        private readonly int MovementZHash = Animator.StringToHash("MovementZ");
        private readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        private readonly int IsJumpingHash = Animator.StringToHash("IsJumping");




        private void Awake()
        {
            PlayerTransform = transform;

            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerRigidBody = GetComponent<Rigidbody>();
            PlayerNavMesh = GetComponent<NavMeshAgent>();

        }
        private void Start()
        {
            Debug.Log("Start");
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

            if (PlayerController.IsJumping) return;

            PlayerController.IsJumping = button.isPressed;
            PlayerAnimator.SetBool(IsJumpingHash, button.isPressed);
            PlayerRigidBody.AddForce((PlayerTransform.up + MoveDirection) * JumpForce, ForceMode.Impulse);
            PlayerNavMesh.enabled = false;
        }


        private void Update()
        {
            if (PlayerController.IsJumping) return;

            MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

            float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;

            Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);

            NextPositionCheck = transform.position + MoveDirection * MoveDirectionBuffer;
            if (NavMesh.SamplePosition(NextPositionCheck, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                transform.position += movementDirection;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Ground") || !PlayerController.IsJumping) return;
            Debug.Log("Collided");

            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);
        }
    }
}
