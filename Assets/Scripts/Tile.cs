using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    North,
    West,
    East,
    South,
    Northwest,
    Northeast,
    Southwest,
    Southeast
}

public enum TileType
{
    Water,
    Grass
}

public class Tile
{
    private int x, y;
    private TileType type;
    private Action<Tile> onTileChanged;

    public int X => x;
    public int Y => y;
    public Building CurrentBuilding;

    public TileType Type
    {
        get => type;
        set
        {
            type = value;
            onTileChanged(this);
        }
    }

    public Tile(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    public List<Tile> GetNeighbors()
    {
        var on_top_row = y == 0;
        var on_bottom_row = y == World.the.Height-1;
        var on_left_column = x == 0;
        var on_right_column = x == World.the.Width-1;
        
        
        var neighbors = new List<Tile>();
        if (!on_left_column) neighbors.Add(World.the.GetTileAt(x-1, y));
        if (!on_right_column) neighbors.Add(World.the.GetTileAt(x+1, y));
        if (!on_top_row) neighbors.Add(World.the.GetTileAt(x, y+1));
        if (!on_bottom_row) neighbors.Add(World.the.GetTileAt(x, y-1));
        
        if (!on_left_column && !on_top_row) neighbors.Add(World.the.GetTileAt(x-1, y+1));
        if (!on_right_column && !on_top_row) neighbors.Add(World.the.GetTileAt(x+1, y+1));
        if(!on_left_column && !on_bottom_row) neighbors.Add(World.the.GetTileAt(x-1, y-1));
        if(!on_right_column && !on_bottom_row) neighbors.Add(World.the.GetTileAt(x+1, y-1));

        return neighbors;
    }
    
    public Dictionary<Direction, Tile> GetNeighborsByDirection()
    {
        var on_top_row = y == World.the.Height-1;
        var on_bottom_row = y == 0;
        var on_left_column = x == 0;
        var on_right_column = x == World.the.Width-1;
        
        
        var neighbors = new Dictionary<Direction, Tile>();
        if (!on_left_column) neighbors.Add(Direction.West, World.the.GetTileAt(x-1, y));
        if (!on_right_column) neighbors.Add(Direction.East, World.the.GetTileAt(x+1, y));
        if (!on_top_row) neighbors.Add(Direction.North, World.the.GetTileAt(x, y+1));
        if (!on_bottom_row) neighbors.Add(Direction.South, World.the.GetTileAt(x, y-1));

        if (!on_left_column && !on_top_row) neighbors.Add(Direction.Northwest, World.the.GetTileAt(x-1, y+1));
        if (!on_right_column && !on_top_row) neighbors.Add(Direction.Northeast, World.the.GetTileAt(x+1, y+1));
        if(!on_left_column && !on_bottom_row) neighbors.Add(Direction.Southwest, World.the.GetTileAt(x-1, y-1));
        if(!on_right_column && !on_bottom_row) neighbors.Add(Direction.Southeast, World.the.GetTileAt(x+1, y-1));

        return neighbors;
    }

    public void RegisterTileChangedCallback(Action<Tile> onTileChanged)
    {
        this.onTileChanged += onTileChanged;
    }

    public void UnregisterTileChangedCallback(Action<Tile> onTileChanged)
    {
        this.onTileChanged -= onTileChanged;
    }

    public void CallTileChangedCallback()
    {
        this.onTileChanged(this);
    }
    
}
