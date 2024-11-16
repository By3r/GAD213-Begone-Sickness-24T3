using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_ClimbingSystem : MonoBehaviour
{
    #region Variables

    [Header("Climbing Settings")]
    [SerializeField] private float climbingSpeed = 5f;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform torsoTransform;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float groundCheckDelay = 4f;

    [Header("Spherecast vars")]
    [SerializeField] private LayerMask climbableLayer;
    [SerializeField] private float wallCheckDistance = 1f;
    [SerializeField] private float sphereCastRadius = 0.5f;


    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 60f;
    [SerializeField] private float staminaDrainRate = 10f;
    [SerializeField] private float staminaRegenRate = 5f;
    private float currentStamina;

    [Header("UI")]
    [SerializeField] private Slider staminaBar;

    private Vector3 _wallNormal;
    private Animator _animator;
    private bool _groundCheckActive = false;
    private bool _atTopTrigger = false;
    private bool _atBottomTrigger = false;

    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (IsNearClimbableWallFromTorso() && !isClimbing && currentStamina > 0 && !_atTopTrigger && !_atBottomTrigger)
        {
            StartClimbing();
        }

        if (isClimbing)
        {
            ClimbingMovementLogic();

            if (staminaBar != null)
            {
                staminaBar.value = currentStamina / maxStamina;
            }

            if (currentStamina <= 0 || !IsNearClimbableWallFromFeet() || (_groundCheckActive && IsNearGround()))
            {
                StopClimbing();
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && isClimbing)
        {
            StopClimbing();
            _animator.SetBool("PlayerClimbing", false);
        }
    }

    #region Public Functions

    public void StartClimbing()
    {
        isClimbing = true;
        _animator.SetBool("PlayerClimbing", true);

        if (staminaBar != null)
        {
            staminaBar.gameObject.SetActive(true);
        }

        StartCoroutine(DelayedGroundCheck());
    }

    public void StopClimbing()
    {
        isClimbing = false;
        _groundCheckActive = false;
        _animator.SetBool("PlayerClimbing", false);

        if (staminaBar != null)
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    public bool IsClimbing()
    {
        return isClimbing;
    }

    #endregion

    #region Private Functions


    /// <summary>
    /// A vector is created according to the wall's normal to move the player accross it.
    /// </summary>
    private void ClimbingMovementLogic()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0 || horizontalInput != 0)
        {
            _animator.speed = 1f;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            _animator.speed = 0f;
        }
        Vector3 _moveDirection = new Vector3(horizontalInput, verticalInput, 0);
        Vector3 _playerClimbDirection = Vector3.ProjectOnPlane(transform.TransformDirection(_moveDirection), _wallNormal);
        characterController.Move(_playerClimbDirection * climbingSpeed * Time.deltaTime);
    }


    /// <summary>
    /// Checks if the player is near a climbable wall using a spherecast from the torso and stores wall normal.
    /// </summary>
    private bool IsNearClimbableWallFromTorso()
    {
        RaycastHit hit;
        if (Physics.SphereCast(torsoTransform.position, sphereCastRadius, torsoTransform.forward, out hit, wallCheckDistance, climbableLayer))
        {
            _wallNormal = hit.normal;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the player is near a climbable wall using a spherecast from the feet (transform.position) and updates the wall normal.
    /// </summary>
    private bool IsNearClimbableWallFromFeet()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, wallCheckDistance, climbableLayer))
        {
            _wallNormal = hit.normal;
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
        if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, groundCheckDistance))
        {
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

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClimbTop"))
        {
            _atTopTrigger = true;
            StopClimbing();
            Debug.Log("Player reached the top of the climbable area and dismounted.");
        }
        else if (other.CompareTag("ClimbBottom"))
        {
            _atBottomTrigger = true;
            StopClimbing();
            Debug.Log("Player reached the bottom of the climbable area and dismounted.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ClimbTop"))
        {
            _atTopTrigger = false;
        }
        else if (other.CompareTag("ClimbBottom"))
        {
            _atBottomTrigger = false;
        }
    }

    #endregion
    #endregion


}