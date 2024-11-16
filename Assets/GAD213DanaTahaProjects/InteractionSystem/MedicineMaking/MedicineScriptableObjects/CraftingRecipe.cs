using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string resultName; 
    public Sprite resultSprite; 
    public string[] ingredients; 
}
