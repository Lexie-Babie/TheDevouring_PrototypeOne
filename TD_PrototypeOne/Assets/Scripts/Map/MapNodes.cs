using UnityEngine;
using UnityEngine.UIElements;

public class MapNodes : MonoBehaviour
{
    public enum NodeType { Start, Battle, Ingredient, Health }

    [Header("Identity")]
    public NodeType nodeType;
    public string nodeLabel;
    public int nodeIndex;

    [Header("connections - next node")]
    public int[] nextNodeIndices = new int[0];

    [Header("PayLoad")]
    public EnemyData enemyData;
    public IngredientData ingredientData;


    //runtime state
    private bool accessible;
    private SpriteRenderer sr;

    public bool IsAccessible => accessible;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    

    //light up or dim node
    public void SetAccessible(bool value)
    {
        accessible = value;
        if (sr != null)
        {
            Color c = sr.color;
            c.a = value ? 1f : 0.3f;
            sr.color = c;
        }

    }

    //defining what the nodes do
    public void Activate()
    {
        if (!accessible) return;

        switch (nodeType)
        {
            case NodeType.Battle:
                if (enemyData != null)
                GamerManager.Instance.LoadCombat(enemyData);

                else
                    Debug.LogWarning("No enemies on this node: " + nodeLabel);
                break;

            case NodeType.Ingredient:
                if (ingredientData != null)
                {
                    GamerManager.Instance.playerData.inventory.Add(ingredientData);
                    Debug.Log("Added to inventory: " + ingredientData.ingredientName); //fix this pls
                }
                //need to make different types of ingredients (health and key ingredient)
          
                MapManager.Instance.CompleteNode(nodeIndex);
                break;

            case NodeType.Start:
                MapManager.Instance.CompleteNode(nodeIndex);
                break;
        }
    }




}
