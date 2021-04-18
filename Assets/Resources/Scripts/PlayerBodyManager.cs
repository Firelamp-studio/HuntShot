using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BodyParts
{
    public MeshRenderer body;
    public MeshRenderer leftFoot;
    public MeshRenderer rightFoot;
}

public class PlayerBodyManager : MonoBehaviour
{
    private Color _color;
    public Color color
    {
        get => _color;
        set
        {
            _color = value;
            
            bodyParts.body.material.SetColor("_Color", _color);
            bodyParts.leftFoot.material.SetColor("_Color", _color);
            bodyParts.rightFoot.material.SetColor("_Color", _color);
        }
    }
    
    [SerializeField] private BodyParts bodyParts;
    private PlayerController _playerController;
    
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }
}
