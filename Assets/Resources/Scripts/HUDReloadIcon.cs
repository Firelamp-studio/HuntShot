using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDReloadIcon : MonoBehaviour
{
    [SerializeField] private Image fullImage;

    private float _tot;

    public delegate float GetValue();

    private GetValue _getValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            fullImage.fillAmount = 1 - _getValue.Invoke() / _tot;
        }
    }

    public void EnableReloadIcon(GetValue getValue, float tot)
    {
        _getValue = getValue;
        _tot = tot;
        gameObject.SetActive(true);
    }
    
    public void DisableReloadIcon()
    {
        _getValue = null;
        _tot = 0;
        gameObject.SetActive(false);
    }
}