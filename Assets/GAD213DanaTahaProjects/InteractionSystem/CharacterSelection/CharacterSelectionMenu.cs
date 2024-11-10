using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    #region Variables
    public GameObject mainMenu;
    public GameObject characterSelectionMenu;
    public GameObject mainMenuPlayer;  
    public Material[] characterMaterials;
    public MainMenuCameraSwitcher cameraSwitcher;

    private Material _currentSelectedMaterial;
    #endregion

    private void Start()
    {
        LoadSelectedCharacter(); 
        ShowMainMenu();
    }

    #region Public Functions
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
        PlayerData data = new PlayerData
        {
            selectedMaterialName = selectedMaterial.name,           
            playerMainMenuMaterialName = selectedMaterial.name    
        };
        CharacterSave.SaveData(data);
        ApplyMaterialToMainMenuPlayer(selectedMaterial);         
    }

    public void LoadSelectedCharacter()
    {
        PlayerData data = CharacterSave.LoadData();
        if (data != null)
        {
            Material mainMenuMaterial = GetMaterialByName(data.playerMainMenuMaterialName);
            if (mainMenuMaterial != null)
            {
                ApplyMaterialToMainMenuPlayer(mainMenuMaterial);  
            }
        }
        else
        {
            ApplyMaterialToMainMenuPlayer(characterMaterials[0]);
        }
    }

    public void SetCurrentSelectedMaterials(Material inGameMaterial, Material mainMenuMaterial)
    {
        _currentSelectedMaterial = inGameMaterial;

        PlayerData data = new PlayerData
        {
            selectedMaterialName = inGameMaterial.name,
            playerMainMenuMaterialName = mainMenuMaterial.name
        };

        CharacterSave.SaveData(data);
        ApplyMaterialToMainMenuPlayer(mainMenuMaterial);          
    }

    public void SetCurrentSelectedMaterial(Material selectedMaterial)
    {
        _currentSelectedMaterial = selectedMaterial;
        SaveSelectedCharacter(selectedMaterial);                   
    }
    #endregion

    #region Private Functions
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

    private Material GetMaterialByName(string materialName)
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

    private void ApplyMaterialToMainMenuPlayer(Material material)
    {
        if (mainMenuPlayer.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = material;
        }
    }
    #endregion
}
