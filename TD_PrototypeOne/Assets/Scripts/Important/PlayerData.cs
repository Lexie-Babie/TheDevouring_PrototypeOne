using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int MaxHp = 100;
    public int currentHP = 100;
    public int currentNodeIndex = 0; //the node the player is on

    public List<IngredientData> inventory = new List<IngredientData>();

    public bool IsAlive => currentHP > 0; 

    public void TakeDamage(int amount)
    {
        currentHP -= Mathf.Max(0, currentHP - amount);
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(MaxHp, currentHP + amount);
    }
}
