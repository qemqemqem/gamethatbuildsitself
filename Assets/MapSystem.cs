using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tile wall;
    public UnityEngine.Tilemaps.Tile stairsUp;
    public UnityEngine.Tilemaps.Tile stairsDown;
    public UnityEngine.Tilemaps.Tile spikes;
    public UnityEngine.Tilemaps.Tile character;
    public UnityEngine.Tilemaps.Tile empty;

    static MapSystem map; // Singleton

    // Start is called before the first frame update
    void Start()
    {
        map = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void RenderWorldMap(World world)
    {
        UnityEngine.Tilemaps.Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<UnityEngine.Tilemaps.Tilemap>();
        for (int i = 0; i < world.simpleMap.GetLength(0); i++)
        {
            for (int j = 0; j < world.simpleMap.GetLength(1); j++)
            {
                UnityEngine.Tilemaps.Tile tile = map.empty;
                if (world.simpleMap[i, j] == "wall") tile = map.wall;
                if (world.simpleMap[i, j] == "up") tile = map.stairsUp;
                if (world.simpleMap[i, j] == "down") tile = map.stairsDown;
                if (world.simpleMap[i, j] == "spikes") tile = map.spikes;
                if (world.simpleMap[i, j] == "adventurer") tile = map.character;
                if (tile != null) tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }
}
