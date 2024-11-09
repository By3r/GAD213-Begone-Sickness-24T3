using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharacterLoader : MonoBehaviour
{
    public Material[] availableMaterials;

    private void Start()
    {
        LoadAndApplyMainMenuMaterial();
    }

    private void LoadAndApplyMainMenuMaterial()
    {
        PlayerData data = CharacterSave.LoadData();
        Material mainMenuMaterial = null;

        if (data != null)
        {
            mainMenuMaterial = GetMaterialByName(data.playerMainMenuMaterialName);
        }

        if (mainMenuMaterial != null && TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = mainMenuMaterial;
        }
        else if (availableMaterials.Length > 0)
        {
            TryGetComponent<Renderer>(out Renderer render);
            render.material = availableMaterials[0];
        }
    }

    private Material GetMaterialByName(string _materialName)
    {
        foreach (Material mat in availableMaterials)
        {
            if (mat.name == _materialName)
            {
                return mat;
            }
        }
        return null;
    }
}
