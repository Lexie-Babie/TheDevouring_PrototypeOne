using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    // Assign these in the Inspector with your ScriptableObject assets
    [Header("Enemies (assign in order of appearance)")]
    public EnemyData[] enemyPool;

    [Header("Ingredients")]
    public IngredientData[] ingredientPool;

    [Header("Player Piece")]
    public PlayerPiece playerPiece; 

    // Internal list of every node, in index order
    public List<MapNodes> nodes = new List<MapNodes>();

    void Awake() => Instance = this;

    void Start()
    {
        BuildMap();
    }

    // building map

    void BuildMap()
    {
        //  Defining nodes
        var defs = new (string label, MapNodes.NodeType type, Vector2 pos)[]
        {
            ("Start",    MapNodes.NodeType.Start,  new Vector2(-6,  0)),
            ("Health Herb", MapNodes.NodeType.Health,   new Vector2(-3,  1.5f)),
            ("Flesh Hunk",     MapNodes.NodeType.Ingredient,   new Vector2( 0,  1.5f)),
            ("Battle",    MapNodes.NodeType.Battle, new Vector2(-3, -1.5f)),


        };

        // Define connections
        var connections = new int[][]
        {
            new[]{ 1 },   //start -> flesh Hunk
            new[]{ 2 },    // Flesh Hunk -> health herb  
            new[]{ 3 },    // health herb -> battle
            new int[]{ },    // battle (end)
        };

        // connection lines
        foreach (var node in nodes)
            foreach (int next in node.nextNodeIndices)
                DrawLine(node.transform.position, nodes[next].transform.position);

        // 5. Place the player piece on the start node
        RefreshAccess();
        Vector3 startPos = nodes[0].transform.position + (Vector3.up * 0.4f);
        playerPiece.SnapTo(startPos);
    }

    // to prevent player iput while player piece moves

    void Update()
    {
        // Blocks all input while the piece is moving
        if (playerPiece != null && playerPiece.IsMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(worldPos);
            if (hit == null) return;

            MapNodes node = hit.GetComponent<MapNodes>();
            if (node == null || !node.IsAccessible) return;

            // Move the piece first, then node event
            Vector3 target = node.transform.position + Vector3.up * 0.4f;
            playerPiece.MoveTo(target, () => node.Activate());
        }
    }

    // Called by nodes when they complete (non-combat)

    public void CompleteNode(int index)
    {
        GamerManager.Instance.playerData.currentNodeIndex = index;
        RefreshAccess();
    }

    // Call this from wherever combat returns to the map too
    public void OnReturnFromCombat(int completedNodeIndex)
    {
        GamerManager.Instance.playerData.currentNodeIndex = completedNodeIndex;
        RefreshAccess();
    }

    // ── Access refresh ────────────────────────────────────────────────────

    public void RefreshAccess()
    {
        int currentIndex = GamerManager.Instance.playerData.currentNodeIndex;

        // Nodes reachable from current position
        int[] reachable = (currentIndex < nodes.Count)
            ? nodes[currentIndex].nextNodeIndices
            : new int[0];

        for (int i = 0; i < nodes.Count; i++)
        {
            bool canReach = System.Array.IndexOf(reachable, i) >= 0;
            nodes[i].SetAccessible(canReach);
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("EndScene");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    void DrawLine(Vector3 from, Vector3 to)
    {
        var go = new GameObject("Line");
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = new Color(0.6f, 0.55f, 0.45f, 0.5f);
        lr.endColor = new Color(0.6f, 0.55f, 0.45f, 0.5f);
        lr.sortingOrder = -1;
    }

}

