using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnManager : MonoBehaviour
{
    public Color PlayerColor => playerColor;
    [SerializeField] private Color playerColor;

    [SerializeField] private MeshRenderer bodyMesh;

    void Start()
    {
        if (!Application.isPlaying) return;
        
        bodyMesh.gameObject.SetActive(false);
    }


    private void OnValidate()
    {
        if(bodyMesh == null)
            return;
        
        
        var tempMaterial = new Material(bodyMesh.sharedMaterial);
        tempMaterial.SetColor("_Color", playerColor);
        bodyMesh.sharedMaterial = tempMaterial;
    }
}