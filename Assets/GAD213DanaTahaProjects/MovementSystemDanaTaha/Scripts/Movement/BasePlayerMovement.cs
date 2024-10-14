using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    public float moveSpeed = 4f;

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

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
       
        if (!climbingSystemScript.IsClimbing())
        {
            CheckGroundStatus(); 
            HandleMovementInput();
            ApplyGravity();
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
    protected virtual void HandleMovementInput()
    {
      
        if (climbingSystemScript.IsClimbing()) return; 

        float inputZ = Input.GetAxisRaw("Vertical");
        bool inputRotateLeft = Input.GetKey(KeyCode.A); 
        bool inputRotateRight = Input.GetKey(KeyCode.D); 

        if (inputZ != 0f)
        {
            Vector3 moveDirection = transform.forward * inputZ;
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        if (inputRotateLeft)
        {
            _controller.Move(transform.forward * moveSpeed * Time.deltaTime);
            _isRunning = true;
            transform.Rotate(0f, -rotationSpeed, 0f); 
        }
        else if (inputRotateRight)
        {
            _controller.Move(transform.forward * moveSpeed * Time.deltaTime);
            _isRunning = true;
            transform.Rotate(0f, rotationSpeed, 0f); 
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
    }

    /// <summary>
    /// Updates Animator parameters based on movement.
    /// </summary>
    protected virtual void UpdateAnimatorParameters()
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
