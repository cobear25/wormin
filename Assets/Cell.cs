using UnityEngine;
using TMPro;
using Unity.Collections;
using System.Collections.Generic;

public struct CellModel
{
    public int x;
    public int y;
    public int cellNumber;
    public int value;
}
public class Cell : MonoBehaviour
{
    public CellModel cellModel;
    public TextMeshPro cellText;
    public List<Sprite> dirtSprites;
    public List<Sprite> rockSprites;
    public Sprite solidRockSprite;

    public SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCellModel(CellModel cellModel)
    {
        this.cellModel = cellModel;
        cellText.text = cellModel.value.ToString();
        // cellText.text = "" + cellModel.x + "," + cellModel.y;
        // cellText.text = cellModel.cellNumber.ToString();
        if (cellModel.value == 0) 
        {
            cellText.text = "";
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
        else if (cellModel.value < 0)
        {
            spriteRenderer.sprite = rockSprites[Random.Range(0, rockSprites.Count)];
            spriteRenderer.flipX = Random.Range(0, 2) == 1;
            spriteRenderer.flipY = Random.Range(0, 2) == 1;
            cellText.color = new Color(1, 0.37f, 0.37f, 1);
        } else {
            spriteRenderer.sprite = dirtSprites[Random.Range(0, dirtSprites.Count)];
            spriteRenderer.flipX = Random.Range(0, 2) == 1;
            spriteRenderer.flipY = Random.Range(0, 2) == 1;
            cellText.color = new Color(0, 0.75f, 0, 1);
            if (cellModel.value >= 20)
            {
                cellText.color = Color.yellow;
            }
        }
    }

    public void MakeSolid()
    {
        spriteRenderer.sprite = solidRockSprite;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        spriteRenderer.flipX = Random.Range(0, 2) == 1;
        spriteRenderer.flipY = Random.Range(0, 2) == 1;
        cellText.color = new Color(0, 0, 0, 0);
        cellModel = new CellModel
        {
            x = cellModel.x,
            y = cellModel.y,
            value = 0,
            cellNumber = cellModel.cellNumber
        };
    }

    public void SetAsBackground()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.2f);
        cellText.color = new Color(0, 0, 0, 0);
    }
}
