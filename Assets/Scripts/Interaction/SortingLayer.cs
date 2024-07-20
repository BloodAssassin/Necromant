using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    Vector2 worldPosition;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        worldPosition = transform.position;

        spriteRenderer.sortingOrder = -(int)worldPosition.y;
    }
}
