using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using Random = UnityEngine.Random;

public class World
{
    private int width, height;
    private Tile[,] tiles;
    
    private List<Rect> pools = new List<Rect>();
    
    public static World the = null;
    public int Width => width;
    public int Height => height;
     

    private TileType TileTypeAtLocation(int x, int y)
    {
        /* TODO: USED FOR NON RECTANGLE POOLS, current system is kinda bad
         
        const float NOISE_STRENGTH = 0.15f;
        float noise = Mathf.PerlinNoise(x * NOISE_STRENGTH, y * NOISE_STRENGTH);

        Debug.Log(noise);
        const float WATER_CUTOFF = 0.7f;

        return noise > WATER_CUTOFF ? TileType.Water : TileType.Grass;
        */

        foreach (var pool in pools)
        {
            if (pool.Contains(new Vector2(x, y)))
            {
                return TileType.Water;
            }
        }

        return TileType.Grass;

    }

    private void GeneratePools(int number)
    {
        for (int i = 0; i < number; i++)
        {
            const float POOL_SIZE_DIVIDER = 5;
            
            var x = Random.Range(0, width - 2);
            var y = Random.Range(0, height - 2);
            var w = Random.Range(1, (width - x) / POOL_SIZE_DIVIDER);
            var h = Random.Range(1, (height - y) / POOL_SIZE_DIVIDER);

            var new_pool = new Rect(x, y, w, h);

            if (new_pool.Contains(Vector2.zero)) //TODO: replace with player spawn point
            {
                i--;
                continue;
            }
            
            foreach (var pool in pools) //Theres a better way to do this that im too tired to see
            {
                if (pool.Overlaps(new_pool))
                {
                    i--;
                    break;
                }
                pools.Add(new_pool);
                break;
            }
            if(pools.Count == 0) pools.Add(new_pool);
        }
    }

    public World(int width, int height)
    {
        the = this;
        this.width = width;
        this.height = height;
        tiles = new Tile[width, height];
        
        GeneratePools(Random.Range(5, 12));

        Debug.Log(pools.Count + " pools");
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = new Tile(i, j, TileTypeAtLocation(i, j));
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        Assert.IsTrue(x >= 0 && x < width && y >= 0 && y < height);
        
        return tiles[x, y];
    }

    public bool IsTileType(int x, int y, TileType type)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }

        return tiles[x, y].Type == type;
    }

    public void ForEachTile(Action<Tile> callback)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                callback(tiles[i, j]);
            }
        }
    }
}
