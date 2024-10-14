using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;

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

    // Script References.
    [SerializeField] private Player_ClimbingSystem climbingSystemScript;

    // Const
    private const float _noValue = 0f;
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
        }

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
        if (_isGrounded && _velocity.y < _noValue)
        {
            _velocity.y = -5f; // Adds force to fake gravity.
        }
    }

    /// <summary>
    /// Handles player input for movement.
    /// </summary>
    protected virtual void HandleMovementInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(inputX, _noValue, inputZ).normalized;

        _inputDirection = inputDir;

        _isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (_inputDirection != Vector3.zero)
        {
            Vector3 moveDirection = transform.TransformDirection(_inputDirection);
            _controller.Move(moveDirection * Time.deltaTime * 4f);
        }
    }

    /// <summary>
    /// Handles player rotation based on input and mouse movement.
    /// </summary>
    private void PlayerRotation()
    {
        if (_inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    /// <summary>
    /// Applies gravity feel to the player, but only if they are not climbing.
    /// </summary>
    private void ApplyGravity()
    {
        if (!climbingSystemScript.IsClimbing())
        {
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    ///<summary>
    /// Checks whether the player touched a climbable wall to climb or not.
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.GetComponent<ClimbableWall>() && Input.GetAxis("Vertical") > _noValue)
        {
            climbingSystemScript.StartClimbing();
        }
        else if (!hit.collider.gameObject.GetComponent<ClimbableWall>())
        {
            climbingSystemScript.StopClimbing();
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
