using Priority_Queue;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 coords;
    public bool walkable = true;

    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void SetColor(Material mat)
    {
        Debug.Log("change");
        Material[] mats = GetComponent<MeshRenderer>().materials;
        mats[1] = mat;
        GetComponent<MeshRenderer>().materials = mats;
    }
}

public class Node : FastPriorityQueueNode
{
    public Tile tile;
    public Node prev;
    public float cost;

    public Node(Tile tile, float cost, Node prev = null)
    {
        this.tile = tile;
        this.prev = prev;
        this.cost = cost;
    }
}
