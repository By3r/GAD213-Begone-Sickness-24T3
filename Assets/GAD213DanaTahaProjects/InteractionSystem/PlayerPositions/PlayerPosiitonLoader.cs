using UnityEngine;

public class PlayerPosiitonLoader : MonoBehaviour
{
    private void Start()
    {
        LoadPlayerPosition();
    }

    public void LoadPlayerPosition()
    {
        PlayerData data = CharacterSave.LoadData();
        if (data != null)
        {
            Vector3 savedPosition = new Vector3(data.positionX, data.positionY, data.positionZ);
            transform.position = savedPosition;
        }
    }
}
