using UnityEngine;

public class SicknessRoom : MonoBehaviour
{
    #region Variables
    public bool isPlayerInRoom;
    [SerializeField] private SicknessBar sicknessBar;

    #endregion

    void Star()
    {
        isPlayerInRoom = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = true;
            sicknessBar.EnterSicknessRoom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            sicknessBar.ExitSicknessRoom();
        }
    }
}
