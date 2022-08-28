using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building
{
    protected int width, height;
    protected string name;
    protected Sprite image;

    public int Width => width;
    public int Height => height;
    public string Name => name;
    public Sprite Image => image;

    public BuildingController CurrentBuildingController;

    protected Building(int width, int height, string name, string sprite_name)
    {
        this.width = width;
        this.height = height;
        this.name = name;
        this.image = Resources.Load<Sprite>(sprite_name);
    }

    public virtual void Update() {}
}
