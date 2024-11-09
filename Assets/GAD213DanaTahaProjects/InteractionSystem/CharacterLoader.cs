using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public Material[] availableMaterials;  

    private void Start()
    {
        LoadAndApplySavedTheMaterial();
    }


    public void LoadAndApplySavedTheMaterial()
    {
        PlayerData data = CharacterSave.LoadData();
        Material savedMaterial = null;

        if (data != null)
        {
            savedMaterial = GetMaterialName(data.selectedMaterialName);
        }

        if (savedMaterial != null && TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = savedMaterial;
        }
        else if (availableMaterials.Length > 0)
        {
            TryGetComponent<Renderer>(out Renderer rend);
            rend.material = availableMaterials[0];
        }
    }


    private Material GetMaterialName(string materialName)
    {
        foreach (Material mat in availableMaterials)
        {
            if (mat.name == materialName)
            {
                return mat;
            }
        }
        return null;  
    }
}
