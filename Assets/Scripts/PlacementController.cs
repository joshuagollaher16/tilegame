using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public static PlacementController the;


    bool CanPlaceAt(int x, int y, int width, int height)
    {
        if (x + width >= World.the.Width || y + height >= World.the.Height) return false;
        
        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                var tile = World.the.GetTileAt(i, j);
                if (tile.Type == TileType.Water || tile.CurrentBuilding != null) return false;
            }
        }

        return true;
    }
    
    public bool PlaceBuildingOn(Building building, Tile tile)
    {
        if (!CanPlaceAt(tile.X, tile.Y, building.Width, building.Height)) return false;
        
        var x = tile.X;
        var y = tile.Y;
        
        Debug.Log("Pressed tile " + tile.X + ", " + tile.Y);
        
        GameObject factory = new GameObject("Factory");
        factory.transform.position = new Vector3(x - 0.5f, y + 0.5f, -0.1f);

        var building_controller = factory.AddComponent<BuildingController>();
        building_controller.AddComponent<SpriteRenderer>();

        building_controller.SetBuilding(building);

        List<Tile> affected_tiles = new List<Tile>();

        for (int i = x; i < x + building.Width; i++)
        {
            for (int j = y; j < y + building.Height; j++)
            {
                if (i < World.the.Width && j < World.the.Height)
                {
                    affected_tiles.Add(World.the.GetTileAt(i, j));
                    World.the.GetTileAt(i, j).CurrentBuilding = building;
                }
            }
        }

        building_controller.SetTiles(affected_tiles);
        return true;
    }

    void OnTilePressed(Tile tile) 
    {
        PlaceBuildingOn(new Factory(), tile);
    }

    void Start()
    {
        the = this;
        
        World.the.ForEachTile(tile =>
        {
            WorldController.the.tile_gameobject_dictionary[tile].GetComponent<TileController>().RegisterOnTilePressedCallback(OnTilePressed);
        });
    }
}
