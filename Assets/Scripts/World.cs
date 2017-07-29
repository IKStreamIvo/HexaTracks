using Koxel;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class World : MonoBehaviour {

    public static World instance;

    public Tile startTile;
    public Tile goalTile;
    public int mapRadius = 16;
    public GameObject tilePrefab;
    public GameObject robotPrefab;
    public Dictionary<Vector2, Tile> map;

    public Material walkable;
    public Material unknown;
    public Material blocked;
    public Material startend;

	void Awake () {
        instance = this;
        Generate();
	}

	public bool ShowTile(Tile tile)
    {
        if (tile.walkable)
        {
            tile.SetColor(walkable);
            return true;
        }
        else
        {
            tile.SetColor(blocked);
            return false;
        }
    }

    void Generate()
    {
        //Placement
        map = new Dictionary<Vector2, Tile>();
        HexData hexData = new HexData(3f);

        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
            int r2 = Mathf.Min(mapRadius, -q + mapRadius);
            for (int r = r1; r <= r2; r++)
            {
                Vector3 realPos = new Vector3(
                    r * hexData.Width() + q * (.5f * hexData.Width()),
                    0,
                    q * (hexData.Height() * .75f)
                );
                GameObject tileObj = Instantiate(tilePrefab, realPos, Quaternion.identity, transform);
                tileObj.name = "Tile (" + q + ", " + r + ")";
                tileObj.transform.localScale = tileObj.transform.localScale * hexData.Size();
                Tile tile = tileObj.GetComponent<Tile>();
                tile.coords = new Vector2(q, r);
                map.Add(new Vector2(q, r), tile);

                tile.SetColor(unknown);
                tile.walkable = true;
            }
        }

        List<Tile> path = new List<Tile>();
        List<Tile> firsts = GenerateMaze();
        startTile = firsts[0];
        goalTile = firsts[1];
        path = AStar(startTile, goalTile);
        
        GameObject robot = Instantiate(robotPrefab, startTile.transform.position, Quaternion.identity);
        robot.name = "Robot";
        robot.transform.Rotate(new Vector3(0, 30f, 0));
        robot.GetComponent<PlayerController>().currentTile = startTile;
    }

    List<Tile> GenerateMaze()
    {
        //Maze
        List<Tile> visited = new List<Tile>();
        Tile currentTile = map[new Vector2(-mapRadius, Random.Range(1, mapRadius - 1))];

        Tile startTile = currentTile;
        Tile goalTile = map[new Vector2(mapRadius, Random.Range(-1, -mapRadius + 1))];
        startTile.walkable = true;
        goalTile.walkable = true;
        goalTile.SetColor(startend);
        currentTile.SetColor(startend);

        //Block some tiles
        List<Tile> blockedTiles = new List<Tile>();
        List<int> usedXs = new List<int>();
        List<int> usedYs = new List<int>();
        usedXs.Add(5000);
        usedYs.Add(5000);

        //                      Amount of blockers
        for (int i = 0; i < Random.Range(mapRadius, mapRadius * 1.5f); i++)
        {
            int x, y;
            //int x = 5000, y = 5000;
            //while (usedXs.Contains(x) || usedYs.Contains(y))
            //{
                x = Random.Range(-mapRadius + 2, mapRadius - 2);

                int yBegin = 0;
                int yEnd = 0;
                if (x <= 0)
                {
                    yBegin = (-(mapRadius + x));
                    yEnd = mapRadius;
                }
                else
                {
                    yBegin = -mapRadius;
                    yEnd = mapRadius - x;
                }
                y = Random.Range(yBegin, yEnd);
            //}
            usedXs.Add(x);
            usedYs.Add(y);
            //map[new Vector2(x, y)].SetColor(blocked);

            int width = Random.Range(2, 5);
            int start = y - width / 2;
            //Debug.Log(width + " start at " + start);
            for (int f = start; f < start + width; f++)
            {
                if (map.ContainsKey(new Vector2(x, f)))
                {
                    //map[new Vector2(x, f)].SetColor(blocked);
                    blockedTiles.Add(map[new Vector2(x, f)]);
                    map[new Vector2(x, f)].walkable = false;
                }
            }
        }
        return new List<Tile> { startTile, goalTile };
    }

    int maxMoveDist = 100;
    public List<Tile> AStar(Tile start, Tile goal)
    {
        if (HexDistance(start, goal) <= maxMoveDist)
        {
            //Debug.Log(HexDistance(start, goal));
            FastPriorityQueue<Node> frontier;
            Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();
            List<Tile> path;
            Node current = new Node(start, 0);

            frontier = new FastPriorityQueue<Node>(maxMoveDist * 6);
            frontier.Enqueue(current, 0);

            visited[start] = true;

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();

                if (current.tile == goal)
                    break;

                foreach (Tile next in TileNeighbours(current.tile))
                {
                    if (next == null)
                    {
                        Debug.Log(next + " from " + current.tile);
                        current.tile.SetColor(new Color());
                    }
                    if (next.walkable) {
                        var new_cost = current.cost + 1;
                        if (!visited.ContainsKey(next))
                        {
                            visited[next] = true;

                            var heuristic = HexDistance(goal, next);
                            float priority = (float)new_cost + heuristic;
                            frontier.Enqueue(new Node(next, new_cost, current), priority);
                        }
                    }
                }
            }

            if (current.tile != goal) // path not found
            {
                return new List<Tile>(); // empty path
            }

            // current = goal;
            path = new List<Tile>();
            path.Add(current.tile);
            while (current.tile != start)
            {
                current = current.prev;
                path.Add(current.tile);
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
        return new List<Tile>();
    }

    public List<Tile> TileNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        Vector2 coords = tile.coords;

        if (map.ContainsKey(coords + new Vector2(0, 1)))
        {
            neighbours.Add(map[coords + new Vector2(0, 1)]);
        }
        if (map.ContainsKey(coords + new Vector2(0, -1)))
        {
            neighbours.Add(map[coords + new Vector2(0, -1)]);
        }
        if (map.ContainsKey(coords + new Vector2(-1, 1)))
        {
            neighbours.Add(map[coords + new Vector2(-1, 1)]);
        }
        if (map.ContainsKey(coords + new Vector2(-1, 0)))
        {
            neighbours.Add(map[coords + new Vector2(-1, 0)]);
        }
        if (map.ContainsKey(coords + new Vector2(1, -1)))
        {
            neighbours.Add(map[coords + new Vector2(1, -1)]);
        }
        if (map.ContainsKey(coords + new Vector2(1, 0)))
        {
            neighbours.Add(map[coords + new Vector2(1, 0)]);
        }
        return neighbours;
    }

    public Dictionary<int, Tile> TileNeighboursDict(Tile tile)
    {
        Dictionary<int, Tile> neighbours = new Dictionary<int, Tile>();
        Vector2 coords = tile.coords;

        if (map.ContainsKey(coords + new Vector2(0, 1)))
        {
            neighbours.Add(1, map[coords + new Vector2(0, 1)]);
        }
        if (map.ContainsKey(coords + new Vector2(0, -1)))
        {
            neighbours.Add(4, map[coords + new Vector2(0, -1)]);
        }
        if (map.ContainsKey(coords + new Vector2(-1, 1)))
        {
            neighbours.Add(2, map[coords + new Vector2(-1, 1)]);
        }
        if (map.ContainsKey(coords + new Vector2(-1, 0)))
        {
            neighbours.Add(3, map[coords + new Vector2(-1, 0)]);
        }
        if (map.ContainsKey(coords + new Vector2(1, -1)))
        {
            neighbours.Add(5, map[coords + new Vector2(1, -1)]);
        }
        if (map.ContainsKey(coords + new Vector2(1, 0)))
        {
            neighbours.Add(0, map[coords + new Vector2(1, 0)]);
        }
        return neighbours;
    }

    float HexDistance(Tile tileA, Tile tileB)
    {
        Vector2 a = tileA.coords;
        Vector2 b = tileB.coords;
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs((-a.x - a.y) - (- b.x - b.y)));
    }

    void SetWalkable(Tile tile)
    {
        tile.walkable = true;
        tile.SetColor(walkable);
    }
}
