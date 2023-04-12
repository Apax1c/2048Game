using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static int ticker;

    [SerializeField] private GameObject fillCellPrefab;
    [SerializeField] private CellPrefab[] allCells;

    public static Action<string> slide;
    public int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int isGameOver;
    [SerializeField] private GameObject gameOverUI;

    private int winningScore = 2048;
    [SerializeField] private GameObject gameWinningUI;
    private bool hasWon;

    public Color[] fillColors;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartSpawnFill();
        StartSpawnFill();
    }

    void Update()
    {
        SlideInput(KeyCode.W, "w");
        SlideInput(KeyCode.A, "a");
        SlideInput(KeyCode.S, "s");
        SlideInput(KeyCode.D, "d");
    }

    private void SlideInput(KeyCode keyCode, string slideDirection)
    {
        if (Input.GetKeyDown(keyCode))
        {
            ticker = 0;
            isGameOver = 0;
            slide(slideDirection);
        }
    }

    public void SpawnFill()
    {
        bool isFull = true;
        foreach (CellPrefab cell in allCells)
        {
            if (cell.fill == null)
            {
                isFull = false;
                break;
            }
        }

        if (isFull)
        {
            return;
        }

        int cellIndex = UnityEngine.Random.Range(0, allCells.Length);
        float cellValueRandom = UnityEngine.Random.Range(0.0f, 1.0f);

        if (allCells[cellIndex].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }

        if(cellValueRandom < 0.2f)
        {
            return;
        }
        else if(cellValueRandom < 0.8f)
        {
            GameObject tempFill = Instantiate(fillCellPrefab, allCells[cellIndex].transform);
            allCells[cellIndex].GetComponent<CellPrefab>().fill = tempFill.GetComponent<FillCellPrefab>();
            tempFill.GetComponent<FillCellPrefab>().SetValueUpdate(2);
        }
        else
        {
            GameObject tempFill = Instantiate(fillCellPrefab, allCells[cellIndex].transform);
            allCells[cellIndex].GetComponent<CellPrefab>().fill = tempFill.GetComponent<FillCellPrefab>();
            tempFill.GetComponent<FillCellPrefab>().SetValueUpdate(4);
        }
    }

    public void StartSpawnFill()
    {
        int cellIndex = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[cellIndex].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }

        GameObject tempFill = Instantiate(fillCellPrefab, allCells[cellIndex].transform);
        allCells[cellIndex].GetComponent<CellPrefab>().fill = tempFill.GetComponent<FillCellPrefab>();
        tempFill.GetComponent<FillCellPrefab>().SetValueUpdate(2);
    }

    public void ScoreUpdate(int scoreIn)
    {
        score += scoreIn;
        scoreText.text = score.ToString();
    }

    public void GameOverCheck()
    {
        isGameOver++;
        if(isGameOver >= 16)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void WinningCheck(int highestFill)
    {
        if (hasWon)
        {
            return;
        }

        if (highestFill == winningScore)
        {
            gameWinningUI.SetActive(true);
            hasWon = true;
        }
    }
}
