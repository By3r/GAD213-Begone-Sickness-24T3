using UnityEngine;

public class SicknessRoom : MonoBehaviour
{
    #region Variables
    public bool isPlayerInRoom;
    [SerializeField] private SicknessBar sicknessBar;
    [SerializeField] private TreeCurer treeCurer;

    #endregion

    void Start()
    {
        isPlayerInRoom = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && treeCurer.isCured == false)
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
