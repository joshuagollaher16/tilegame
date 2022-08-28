using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private World world;

    public static WorldController the;

    public Dictionary<Tile, GameObject> tile_gameobject_dictionary;

    public List<BuildingController> building_controllers = new List<BuildingController>();
    
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite NWLandSprite;
    [SerializeField] private Sprite WLandSprite;
    [SerializeField] private Sprite SWLandSprite;
    [SerializeField] private Sprite SLandSprite;
    [SerializeField] private Sprite SELandSprite;
    [SerializeField] private Sprite ELandSprite;
    [SerializeField] private Sprite NELandSprite;
    [SerializeField] private Sprite NLandSprite;
    
    void Awake()
    {
        the = this;
        world = new World(width, height);
        tile_gameobject_dictionary = new Dictionary<Tile, GameObject>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = world.GetTileAt(i, j);
                var tile_representation = new GameObject($"Tile {i}, {j}");

                tile_gameobject_dictionary.Add(tile, tile_representation);
                
                tile_representation.transform.position = new Vector2(i, j);
                tile_representation.AddComponent<SpriteRenderer>();
                var collider = tile_representation.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(1f, 1f);
                collider.isTrigger = true;
                var tile_controller = tile_representation.AddComponent<TileController>();
                tile_controller.CurrentTile = tile;
                
                tile.RegisterTileChangedCallback(OnTileChanged);
                tile.CallTileChangedCallback();
            }
        }
    }

    public void OnTileChanged(Tile tile)
    {
        var tile_gameobject = tile_gameobject_dictionary[tile];
        var sprite_renderer = tile_gameobject.GetComponent<SpriteRenderer>();
        
        switch (tile.Type)
        {
            case TileType.Grass:

                var new_sprite = grassSprite;

                var neighbors = tile.GetNeighborsByDirection();
                var was_water_in_NWSE = false; // Do the 4 directions first, since a tile can be touching multiple water tiles it gets weird
                
                if (neighbors.ContainsKey(Direction.North) && neighbors[Direction.North].Type == TileType.Water)
                {
                    new_sprite = SLandSprite;
                    was_water_in_NWSE = true;
                }
                if (neighbors.ContainsKey(Direction.West) && neighbors[Direction.West].Type == TileType.Water)
                {
                    new_sprite = ELandSprite;
                    was_water_in_NWSE = true;
                }
                if (neighbors.ContainsKey(Direction.South) && neighbors[Direction.South].Type == TileType.Water)
                {
                    new_sprite = NLandSprite;
                    was_water_in_NWSE = true;
                }
                if (neighbors.ContainsKey(Direction.East) && neighbors[Direction.East].Type == TileType.Water)
                {
                    new_sprite = WLandSprite;
                    was_water_in_NWSE = true;
                }
                if (!was_water_in_NWSE && neighbors.ContainsKey(Direction.Northwest) && neighbors[Direction.Northwest].Type == TileType.Water)
                {
                    new_sprite = SELandSprite;
                }
                if (!was_water_in_NWSE && neighbors.ContainsKey(Direction.Northeast) && neighbors[Direction.Northeast].Type == TileType.Water)
                {
                    new_sprite = SWLandSprite;
                }
                if (!was_water_in_NWSE && neighbors.ContainsKey(Direction.Southwest) && neighbors[Direction.Southwest].Type == TileType.Water)
                {
                    new_sprite = NELandSprite;
                }
                if (!was_water_in_NWSE && neighbors.ContainsKey(Direction.Southeast) && neighbors[Direction.Southeast].Type == TileType.Water)
                {
                    new_sprite = NWLandSprite;
                }
                
                sprite_renderer.sprite = new_sprite;
                
                break;
            case TileType.Water:
                sprite_renderer.sprite = waterSprite;
                break;
        }
        
    }

}
