using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Rotation Settings")]
    public float rotationSpeed = 400f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    // Animation Parameter Labels.
    protected string _idle = "PlayerNotMoving";
    protected string _run = "PlayerRunning";

    // Animation and controller.
    protected Animator _animator;
    protected CharacterController _controller;

    // Movement Variables.
    protected Vector3 _inputDirection = Vector3.zero;
    protected bool _isRunning = false;

    // 'False' gravity Variables.
    protected Vector3 _velocity;
    protected bool _isGrounded;
    #endregion

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        CheckGroundStatus();
        HandleMovementInput();
        ApplyGravity();
        PlayerRotation();
        UpdateAnimatorParameters();
    }

    #region Private Functions
    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    private void CheckGroundStatus()
    {
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Adds force to fake gravity.
        }
    }

    /// <summary>
    /// Handles player input for movement.
    /// </summary>
    protected virtual void HandleMovementInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(inputX, 0f, inputZ).normalized;

        _inputDirection = inputDir;

        _isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    /// <summary>
    /// Applies gravity feel to the player.
    /// </summary>
    private void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles smooth rotation based on input direction.
    /// </summary>
    private void PlayerRotation()
    {
        if (_inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Updates Animator parameters based on movement.
    /// </summary>
    protected virtual void UpdateAnimatorParameters()
    {
        if (_inputDirection != Vector3.zero)
        {
            _animator.SetBool(_idle, false);
            _animator.SetBool(_run, _isRunning);
        }
        else
        {
            _animator.SetBool(_idle, true);
            _animator.SetBool(_run, false);
        }
    }
    #endregion
}