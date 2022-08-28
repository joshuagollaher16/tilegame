using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private Action<Tile> on_tile_pressed_callback;

    public Tile CurrentTile = null;
    
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        on_tile_pressed_callback(CurrentTile);
    }

    public void RegisterOnTilePressedCallback(Action<Tile> callback)
    {
        on_tile_pressed_callback += callback;
    }
    
    public void UnregisterOnTilePressedCallback(Action<Tile> callback)
    {
        on_tile_pressed_callback -= callback;
    }
}
