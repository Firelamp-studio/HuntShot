using UnityEngine;

public abstract class Item
{
    public readonly string Name;
    public readonly Texture Texture;

    protected Item(string name, Texture texture)
    {
        Name = name;
        Texture = texture;
    }
}