using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Map/Enemies")]
public class EnemyData : ScriptableObject 
{
    public string EnemyName = "False Knight";
    public int maxHP = 100;
    public int BaseAttack = 20;

   
}