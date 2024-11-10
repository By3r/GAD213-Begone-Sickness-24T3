using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionSaver : MonoBehaviour
{
    public void SavePlayerPosition()
    {
        PlayerData data = CharacterSave.LoadData() ?? new PlayerData();

        data.positionX = transform.position.x;
        data.positionY = transform.position.y;
        data.positionZ = transform.position.z;

        CharacterSave.SaveData(data);
    }
}
