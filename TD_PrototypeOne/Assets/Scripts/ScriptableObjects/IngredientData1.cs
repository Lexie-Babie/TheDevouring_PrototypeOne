using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptableObject", menuName = "Map/Ingredients")]
public class IngredientData : ScriptableObject 
{
    public string IngridentName = "Flesh Hunk";
    public int DamageValue = 15;
    internal string ingredientName;
}