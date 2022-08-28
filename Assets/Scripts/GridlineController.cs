using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridlineController : MonoBehaviour
{
    [SerializeField] Sprite grid_sprite;
    
    void Start()
    {
        World.the.ForEachTile(tile =>
        {
            var grid_gameobject = new GameObject("Grid " + tile.X + ", " + tile.Y);
            grid_gameobject.transform.SetParent(WorldController.the.tile_gameobject_dictionary[tile].transform);
            grid_gameobject.transform.localPosition = new Vector3(0, 0, -0.01f);

            var sprite_renderer = grid_gameobject.AddComponent<SpriteRenderer>();
            sprite_renderer.sprite = grid_sprite;
        });
    }
}
