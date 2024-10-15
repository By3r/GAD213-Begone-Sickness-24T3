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

    // Animation and controller
    protected Animator _animator;
    protected CharacterController _controller;

    // Movement Variables
    protected Vector3 _inputDirection = Vector3.zero;
    protected bool _isRunning = false;

    // Gravity and Ground Variables
    protected Vector3 _velocity;
    protected bool _isGrounded;

    // Climbing Script Reference
    [SerializeField] private Player_ClimbingSystem climbingSystemScript;
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
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");

        Vector3 _moveDirection = new Vector3(inputX, 0f, inputZ);
        _moveDirection.Normalize();

        if (_moveDirection.magnitude > 0.1f)
        {
            Vector3 _FaceDirection = transform.TransformDirection(_moveDirection);

            float speedMultiplier = inputZ < 0 ? 0.5f : 1f;

            _controller.Move(_FaceDirection * moveSpeed * speedMultiplier * Time.deltaTime);


            if (inputZ > 0 || inputX != 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_FaceDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            _isRunning = true;
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