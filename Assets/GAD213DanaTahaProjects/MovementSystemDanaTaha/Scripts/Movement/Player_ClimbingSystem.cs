using UnityEngine;

public class Player_ClimbingSystem : MonoBehaviour
{
    #region Variables

    [SerializeField] private float climbingSpeed = 5f;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask climbableLayer; 
    [SerializeField] private float wallCheckDistance = 1f; 

    private Vector3 _playerClimbDirection;
    private float _lastInputTime; 

    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isClimbing)
        {
            ClimbingMovementLogic();

            if (!IsNearClimbableWall())
            {
                Debug.Log("Player moved away from the wall, stopping climbing.");
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
    }

    public void StopClimbing()
    {
        isClimbing = false; 
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

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f; 
        }

        if (verticalInput != 0)
        {
            _lastInputTime = Time.time; 
        }

        _playerClimbDirection = new Vector3(0, verticalInput, 0);
        characterController.Move(_playerClimbDirection * climbingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Checks if the player is near a climbable wall.
    /// </summary>
    private bool IsNearClimbableWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, climbableLayer))
        {
            return true; 
        }

        return false;
    }

    private void CancelTheClimb()
    {
        StopClimbing(); 
    }
    #endregion
}
