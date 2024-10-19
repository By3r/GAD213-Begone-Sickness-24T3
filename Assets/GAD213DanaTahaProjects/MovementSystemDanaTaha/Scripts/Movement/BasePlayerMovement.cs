using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    public float moveSpeed = 10f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    // Animation Parameter Labels
    protected string _idle = "PlayerNotMoving";
    protected string _run = "PlayerRunning";

    // Animator and controller
    protected Animator _animator;
    protected CharacterController _controller;

    // Movement Variables
    private Vector3 _inputDirection = Vector3.zero;
    private bool _isRunning = false;

    // Gravity and Ground Variables
    private Vector3 _velocity;
    private bool _isGrounded;

    // Climbing Script Reference
    [SerializeField] private Player_ClimbingSystem climbingSystemScript;

    // Camera Reference
    [SerializeField] private Transform cameraTransform;
    #endregion

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (!climbingSystemScript.IsClimbing())
        {
            CheckGroundStatus();
            ApplyGravity();
        }
    }

    private void Update()
    {
        if (!climbingSystemScript.IsClimbing())
        {
            HandleMovementInput();
        }
        UpdateAnimatorParameters();
    }

    #region Private Functions
    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    private void CheckGroundStatus()
    {
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.1f, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    /// <summary>
    /// Handles player input for movement.
    /// </summary>
    private void HandleMovementInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * inputZ + right * inputX;

        if (moveDirection.magnitude > 0.1f)
        {
            _isRunning = true;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            _isRunning = false;
        }
    }

    /// <summary>
    /// Applies gravity to the player, but only if they are not grounded.
    /// </summary>
    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
        else
        {
            _velocity.y = -2f;
        }
    }

    /// <summary>
    /// Updates Animator parameters based on movement.
    /// </summary>
    private void UpdateAnimatorParameters()
    {
        if (_isRunning)
        {
            _animator.SetBool(_idle, false);
            _animator.SetBool(_run, true);
        }
        else
        {
            _animator.SetBool(_idle, true);
            _animator.SetBool(_run, false);
        }
    }
    #endregion
}