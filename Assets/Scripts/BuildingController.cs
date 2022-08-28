using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour // Represents a building's gameobject
{
    private Building building;
    private List<Tile> tiles;

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = building?.Image;
    }
    
    public void SetBuilding(Building building)
    {
        this.building = building;
        this.building.CurrentBuildingController = this;
    }

    public void SetTiles(List<Tile> tiles)
    {
        this.tiles = tiles;
    }

    void Start()
    {
        UpdateSprite();
    }
    
    void Update()
    {
        building?.Update();
    }
}
