using UnityEngine;

public class Player_ClimbingSystem : MonoBehaviour
{
    #region Variables

    [SerializeField] private float climbingSpeed = 5f;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Vector3 playerClimbDirection;

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
        }
        if (Input.GetKeyDown(KeyCode.G) && isClimbing)
        {
            CancelTheClimb();
        }
        if (Input.GetAxis("Vertical") > 0 && isClimbing)
        {
            // Stop climbing in whatever position the player is at currently.
        }
    }

    #region Public Functions.
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


    #region Private Functions.
    private void ClimbingMovementLogic()
    {
        float _vertical = Input.GetAxis("Vertical");
        float _horizontal = Input.GetAxis("Horizontal");

        playerClimbDirection = new Vector3(_horizontal, _vertical, 0);
        Debug.Log("Player is trying to climb " + playerClimbDirection);
        characterController.Move(playerClimbDirection * climbingSpeed * Time.deltaTime);
    }

    private void CancelTheClimb()
    {
        StopClimbing(); 
    }

    #endregion

}
