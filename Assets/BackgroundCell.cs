using System.Collections.Generic;
using UnityEngine;

public class BackgroundCell : MonoBehaviour
{
    public List<Sprite> dirtSprites;
    public SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = dirtSprites[Random.Range(0, dirtSprites.Count)];
        spriteRenderer.flipX = Random.Range(0, 2) == 1;
        spriteRenderer.flipY = Random.Range(0, 2) == 1;
    }
}
