using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHeartsManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;

    private const float HeartSpacing = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RefreshHealthBar(int health)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int fullHeartsNum = health / 2;
        bool halfHeart = health % 2 == 1;

        for (int i = 0; i < fullHeartsNum; i++)
        {
            var heartRectTransform = Instantiate(heartPrefab, transform).GetComponent<RectTransform>();
            heartRectTransform.anchoredPosition =
                new Vector2(i * (heartPrefab.GetComponent<RectTransform>().rect.width + HeartSpacing), 0);
        }

        if (halfHeart)
        {
            var heartRectTransform = Instantiate(heartPrefab, transform).GetComponent<RectTransform>();
            heartRectTransform.anchoredPosition =
                new Vector2((fullHeartsNum) * (heartPrefab.GetComponent<RectTransform>().rect.width + HeartSpacing),
                    0);
            heartRectTransform.GetComponent<RawImage>().texture = Resources.Load<Texture2D>("Textures/HalfHeart");
        }
    }
}