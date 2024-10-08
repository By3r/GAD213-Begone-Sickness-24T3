using UnityEngine;

public class PlayerFollowingCam : MonoBehaviour
{
    #region Variables.
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 distanceFromPlayer;

    #endregion 
    private void LateUpdate()
    {
        transform.position = player.position + distanceFromPlayer;

    }
}
