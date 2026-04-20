using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamerManager : MonoBehaviour
{
    public static GamerManager Instance { get; private set; }

    public PlayerData playerData = new PlayerData();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void LoadCombat(EnemyData enemy)
    {
        PendingEnemy = enemy;
        SceneManager.LoadScene("CombatScene");
    }
    public void ReturnToMap()
    {
        SceneManager.LoadScene("DungeonScene");
    }

    public static EnemyData PendingEnemy;
}
