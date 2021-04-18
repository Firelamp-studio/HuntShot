using System;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public string Name;
    public Texture2D Texture;


    public Item()
    {
    }

    protected Item(string name, Texture2D texture)
    {
        Name = name;
        Texture = texture;
    }
}