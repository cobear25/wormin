using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject backgroundPrefab;
    public GameObject player;
    public TextMeshPro playerValueText;
    public GameObject depthMarkerLeft;
    public GameObject depthMarkerRight;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public Sprite headHappy;
    public Sprite headSad;
    public AudioSource audioSource;
    public AudioClip crunchSound;
    public AudioClip rockyCrunchSound;
    public AudioClip victorySound;
    public AudioClip gameOverSound;

    bool isGameOver = false;
    int playerValue = 10;
    int maxDepth = 101;
    Vector2Int playerPosition = new Vector2Int(0, 0);
    Vector2Int previousPlayerPosition = new Vector2Int(0, 0);
    List<Cell> cells = new List<Cell>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawBackground();
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (playerPosition.x > -5) {
                playerPosition.x -= 1;
                player.transform.position = new Vector2(player.transform.position.x - 1, player.transform.position.y);
                player.transform.localScale = new Vector3(-0.45f, 0.45f, 0.45f);
                playerValueText.transform.localScale = new Vector3(-1, 1, 1);
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                PlayerMoved();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (playerPosition.x < 4) {
                playerPosition.x += 1;
                player.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
                player.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                playerValueText.transform.localScale = new Vector3(1, 1, 1);
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                PlayerMoved();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (playerPosition.y > 0) {
                playerPosition.y--;
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1);
                player.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                playerValueText.transform.localScale = new Vector3(1, 1, 1);
                player.transform.rotation = Quaternion.Euler(0, 0, 90);
                PlayerMoved();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
                playerPosition.y++;
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 1);
                player.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                playerValueText.transform.localScale = new Vector3(1, 1, 1);
                player.transform.rotation = Quaternion.Euler(0, 0, -90);
                PlayerMoved();
        }
    }

    void PlayerMoved()
    {
        Cell cell = cells.Find(x => x.cellModel.x == playerPosition.x && x.cellModel.y == playerPosition.y);
        CellModel cellModel = cell.cellModel;
        if (cellModel.value == 0)
        {
            playerPosition = previousPlayerPosition;
            Cell prevCell = cells.Find(x => x.cellModel.x == playerPosition.x && x.cellModel.y == playerPosition.y);
            player.transform.position = prevCell.transform.position;
            return;
        }
        playerValue += cellModel.value;
        if (cellModel.value < 0)
        {
            player.GetComponent<SpriteRenderer>().sprite = headSad;
            audioSource.PlayOneShot(rockyCrunchSound);
            StartCoroutine(FlashPlayerTextPink());
        }
        else
        {
            player.GetComponent<SpriteRenderer>().sprite = headHappy;
            audioSource.PlayOneShot(crunchSound);
            StartCoroutine(FlashPlayerTextGreen());
        }
        player.GetComponentInChildren<TextMeshPro>().text = playerValue.ToString();
        cell.SetCellModel(new CellModel
        {
            x = cellModel.x,
            y = cellModel.y,
            value = 0,
            cellNumber = cellModel.cellNumber
        });
        player.GetComponent<LineRenderer>().positionCount++;
        player.GetComponent<LineRenderer>().SetPosition(player.GetComponent<LineRenderer>().positionCount - 1, player.transform.position);
        previousPlayerPosition = playerPosition;
        if (playerValue <= 0)
        {
            GameOver();
        }
        if (playerPosition.y == maxDepth - 1)
        {
            Victory();
        }
    }

    void DrawBackground()
    {
        for (int i = -12; i < -5; i++)
        {
            for (int j = -2; j < maxDepth; j++)
            {
                GameObject cell = Instantiate(backgroundPrefab);
                cell.transform.position = new Vector3(i, -j + 5, 0);
            }
        }
        for (int i = 5; i < 12; i++)
        {
            for (int j = -2; j < maxDepth; j++)
            {
                GameObject cell = Instantiate(backgroundPrefab);
                cell.transform.position = new Vector3(i, -j + 5, 0);
            }
        }
        for (int i = -5; i < 5; i++)
        {
            for (int j = -2; j < 0; j++)
            {
                GameObject cell = Instantiate(backgroundPrefab);
                cell.transform.position = new Vector3(i, -j + 5, 0);
            }
        }
    }

    void GenerateGrid()
    {
        int cellNumber = 0;
        for (int i = -5; i < 5; i++)
        {
            for (int j = 0; j < maxDepth; j++)
            {
                GameObject cell = Instantiate(cellPrefab);
                cell.transform.position = new Vector3(i, -j + 5, 0);
                cell.GetComponent<Cell>().SetCellModel(new CellModel
                {
                    x = i,
                    y = j,
                    value = GetRandomNonZero(j),
                    cellNumber = cellNumber
                });
                cellNumber++;
                cells.Add(cell.GetComponent<Cell>());
                // cell.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
                if (i == 0 && j == 0)
                {
                    cell.GetComponent<Cell>().SetCellModel(new CellModel
                    {
                        x = 0,
                        y = 0,
                        value = 0,
                        cellNumber = cellNumber - 1
                    });
                }
                if (Random.value < 0.03f)
                {
                    cell.GetComponent<Cell>().MakeSolid();
                }
            }
        }
        for (int i = 0; i < maxDepth; i++)
        {
            GameObject depthMarkerLeftClone = Instantiate(depthMarkerLeft);
            depthMarkerLeftClone.transform.position = new Vector3(-6.7f, -i + 5 - 0.25f, 0);
            depthMarkerLeftClone.GetComponent<TextMeshPro>().text = "" + i + " cm";
            GameObject depthMarkerRightClone = Instantiate(depthMarkerRight);
            depthMarkerRightClone.transform.position = new Vector3(5.7f, -i + 5 - 0.25f, 0);
            depthMarkerRightClone.GetComponent<TextMeshPro>().text = "" + i + " cm";
        }
    }

    int GetRandomNonZero(int depth)
    {
        bool isPositive = Random.value < 0.25f;
        if (isPositive)
        {
            if (Random.value < 0.02f)
            {
                return 20;
            }
            if (Random.value < 0.003f)
            {
                return 50;
            }

            return Random.Range(1, 6 + (depth / 20)); // Returns 1 to 5
        }
        else
        {
            return Random.Range(-5 - (depth / 5), -1); // Returns -5 to -1
        }
    }

    IEnumerator FlashPlayerTextPink()
    {
        playerValueText.color = new Color(1, 0.2f, 0.2f, 1);
        yield return new WaitForSeconds(0.1f);
        playerValueText.color = new Color(1, 1, 1, 1);
    }
    IEnumerator FlashPlayerTextGreen()
    {
        playerValueText.color = new Color(0.2f, 1, 0.2f, 1);
        yield return new WaitForSeconds(0.1f);
        playerValueText.color = new Color(1, 1, 1, 1);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        isGameOver = true;
        audioSource.PlayOneShot(gameOverSound, 2.5f);
    }

    void Victory()
    {
        Debug.Log("Victory!");
        gameOverPanel.SetActive(true);
        isGameOver = true;
        audioSource.PlayOneShot(victorySound, 2.5f);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
