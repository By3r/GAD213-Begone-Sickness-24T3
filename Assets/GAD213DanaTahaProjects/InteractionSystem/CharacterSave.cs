using System.IO;
using UnityEngine;

#region Serialized Class for data saving.
[System.Serializable]
public class PlayerData
{
    public string selectedMaterialName;
    public string playerMainMenuMaterialName;
}
#endregion

public static class CharacterSave
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "playerData.json");

    public static void SaveData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        // Debug.Log("Data saved to: " + savePath);
    }

    public static PlayerData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            // Debug.Log("Data loaded from: " + savePath);
            return data;
        }
        else
        {
            // Debug.LogWarning("No saved file found. Using default materials.");
            return null;
        }
    }
}
