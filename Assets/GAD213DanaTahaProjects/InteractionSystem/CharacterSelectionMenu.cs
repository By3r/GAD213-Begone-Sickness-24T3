using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    #region Variables.
    public GameObject mainMenu;
    public GameObject characterSelectionMenu;
    public GameObject player;
    public Material[] characterMaterials;
    public MainMenuCameraSwitcher cameraSwitcher;

    private Material _currentSelectedMaterial;
    #endregion

    private void Start()
    {
        LoadSelectedCharacter();
        ShowMainMenu();
    }

    #region Public Functions.
    public void ToggleCharacterSelectionMenu()
    {
        if (characterSelectionMenu.activeSelf)
        {
            ShowMainMenu();
        }
        else
        {
            ShowCharacterSelectionMenu();
        }
    }
    public void SaveSelectedCharacter(Material selectedMaterial)
    {
        PlayerData data = new PlayerData { selectedMaterialName = selectedMaterial.name };
        CharacterSave.SaveData(data);
        ApplyMaterialToPlayer(selectedMaterial);
    }

    public void LoadSelectedCharacter()
    {
        PlayerData data = CharacterSave.LoadData();
        if (data != null)
        {
            Material loadedMaterial = GetMaterialName(data.selectedMaterialName);
            if (loadedMaterial != null)
            {
                ApplyMaterialToPlayer(loadedMaterial);
            }
        }
        else
        {
            ApplyMaterialToPlayer(characterMaterials[0]);
        }
    }

    public void SetCurrentSelectedMaterial(Material selectedMaterial)
    {
        _currentSelectedMaterial = selectedMaterial;
        SaveSelectedCharacter(selectedMaterial);
    }
    #endregion


    #region Private Regions.
    private void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        characterSelectionMenu.SetActive(false);
        cameraSwitcher.ShowMainCamera();
    }

    private void ShowCharacterSelectionMenu()
    {
        mainMenu.SetActive(false);
        characterSelectionMenu.SetActive(true);
        cameraSwitcher.ShowCharacterSelectionCamera();
    }



    private Material GetMaterialName(string materialName)
    {
        foreach (Material mat in characterMaterials)
        {
            if (mat.name == materialName)
            {
                return mat;
            }
        }
        return null;
    }

    private void ApplyMaterialToPlayer(Material material)
    {
        if (player.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = material;
        }
    }
    #endregion
}
