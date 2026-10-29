using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodZone : MonoBehaviour
{
    public BoxCollider2D kolajder;

    private void Start()
    {
        RectTransform t = gameObject.GetComponent<RectTransform>();

        kolajder.size = new Vector2(t.rect.width, t.rect.height);
    }
}
