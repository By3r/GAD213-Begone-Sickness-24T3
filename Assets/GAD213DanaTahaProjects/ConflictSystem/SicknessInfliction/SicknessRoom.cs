using UnityEngine;

public class SicknessRoom : MonoBehaviour
{
    #region Variables
    public bool isPlayerInRoom;
    [SerializeField] private SicknessBar sicknessBar;
    [SerializeField] private TreeCurer treeCurer;

    #endregion

    private void Start()
    {
        isPlayerInRoom = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && treeCurer.isCured == false)
        {
            isPlayerInRoom = true;
            sicknessBar.EnterSicknessRoom();
        }
        else
        {
            isPlayerInRoom = false;
            sicknessBar.ExitSicknessRoom();
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

    public void ResetSicknessRoom()
    {
        isPlayerInRoom = false;
        sicknessBar.ExitSicknessRoom();
    }
}
