using UnityEngine;
using System.Collections;

public class Player_ClimbingSystem : MonoBehaviour
{
    #region Variables

    [SerializeField] private float climbingSpeed = 5f;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask climbableLayer;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private float wallCheckDistance = 1f;
    [SerializeField] private float sphereCastRadius = 0.5f;
    [SerializeField] private Transform torsoTransform;
    [SerializeField] private float groundCheckDistance = 1f; 
    [SerializeField] private float groundCheckDelay = 4f; 

    private Vector3 _playerClimbDirection;
    private float _lastInputTime;
    private Animator _animator;
    private bool _groundCheckActive = false; 

    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsNearClimbableWallFromTorso() && !isClimbing)
        {
            Debug.Log("Starting to climb");
            StartClimbing();
        }

        if (isClimbing)
        {
            ClimbingMovementLogic();

            if (!IsNearClimbableWallFromTorso() && IsNearClimbableWallFromFeet())
            {
                Debug.Log("Player reached a point where torso can't detect the wall, using feet detection.");
            }
            else if (!IsNearClimbableWallFromFeet())
            {
                Debug.Log("Player is no longer near any climbable surface, stopping climb.");
                StopClimbing();
            }

            if (_groundCheckActive && IsNearGround())
            {
                Debug.Log("Player is close to the ground, dismounting.");
                StopClimbing();
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && isClimbing)
        {
            CancelTheClimb();
        }
    }

    #region Public Functions
    public void StartClimbing()
    {
        isClimbing = true;
        _animator.SetBool("PlayerClimbing", true); 
        StartCoroutine(DelayedGroundCheck());
    }

    public void StopClimbing()
    {
        isClimbing = false;
        _groundCheckActive = false; 
        _animator.SetBool("PlayerClimbing", false); 
    }

    public bool IsClimbing()
    {
        return isClimbing;
    }
    #endregion

    #region Private Functions
    private void ClimbingMovementLogic()
    {
        float verticalInput = 0;
        float horizontalInput = 0;

        if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.S)) verticalInput = -1f; 
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f; 
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f; 

        if (verticalInput != 0 || horizontalInput != 0)
        {
            _lastInputTime = Time.time;
            _animator.speed = 1f;
        }
        else
        {
            _animator.speed = 0f; 
        }

        _playerClimbDirection = new Vector3(horizontalInput, verticalInput, 0);
        characterController.Move(_playerClimbDirection * climbingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Checks if the player is near a climbable wall using a spherecast from the torso.
    /// </summary>
    private bool IsNearClimbableWallFromTorso()
    {
        RaycastHit hit;
        if (Physics.SphereCast(torsoTransform.position, sphereCastRadius, torsoTransform.forward, out hit, wallCheckDistance, climbableLayer))
        {
            Debug.Log("Climbable wall detected from torso.");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the player is near a climbable wall using a spherecast from the feet (transform.position).
    /// </summary>
    private bool IsNearClimbableWallFromFeet()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, wallCheckDistance, climbableLayer))
        {
            Debug.Log("Climbable wall detected from feet.");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the player is near the ground, used for dismounting.
    /// </summary>
    private bool IsNearGround()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            Debug.Log("Ground detected close to player.");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Coroutine to delay ground checking for the specified time.
    /// </summary>
    private IEnumerator DelayedGroundCheck()
    {
        yield return new WaitForSeconds(groundCheckDelay); 
        _groundCheckActive = true; 
    }

    private void OnDrawGizmos()
    {
        if (torsoTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(torsoTransform.position, sphereCastRadius); 
            Gizmos.DrawWireSphere(transform.position, sphereCastRadius);
        }
    }

    private void CancelTheClimb()
    {
        StopClimbing();
    }
    #endregion
}
