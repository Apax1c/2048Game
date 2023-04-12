using UnityEngine;

public class CellPrefab : MonoBehaviour
{
    public CellPrefab right;
    public CellPrefab left;
    public CellPrefab up;
    public CellPrefab down;

    public CellPrefab[] neighborCellPrefabs;

    public FillCellPrefab fill;

    private void OnEnable()
    {
        GameController.slide += OnSlide;
    }

    private void OnDisable()
    {
        GameController.slide -= OnSlide;
    }

    private void Awake()
    {
        neighborCellPrefabs = new CellPrefab[] { down, right, up, left };
    }

    private void OnSlide(string key)
    {
        CellCheck();
        CellPrefab currentCell = this;

        switch (key)
        {
            case "w":
                if (up != null)
                {
                    return;
                }
                Slide(currentCell, 0);
                break;
            case "a":
                if (left != null)
                {
                    return;
                }
                Slide(currentCell, 1);
                break;
            case "s":
                if (down != null)
                {
                    return;
                }
                Slide(currentCell, 2);
                break;
            case "d":
                if (right != null)
                {
                    return;
                }
                Slide(currentCell, 3);
                break;
        }

        GameController.ticker++;
        if (GameController.ticker == 4)
        {
            GameController.Instance.SpawnFill();
        }
    }

    private void Slide(CellPrefab currentCell, int index)
    {
        if (currentCell.neighborCellPrefabs[index] == null)
        {
            return;
        }

        if (currentCell.fill != null)
        {
            CellPrefab nextCell = currentCell.neighborCellPrefabs[index];
            while (nextCell.neighborCellPrefabs[index] != null && nextCell.fill == null)
            {
                nextCell = nextCell.neighborCellPrefabs[index];
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.GetCellValue() == nextCell.fill.GetCellValue())
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                }
                else if (currentCell.neighborCellPrefabs[index].fill != nextCell.fill)
                {
                    nextCell.fill.transform.SetParent(currentCell.neighborCellPrefabs[index].transform);
                    currentCell.neighborCellPrefabs[index].fill = nextCell.fill;
                    nextCell.fill = null;
                }
            }
        }
        else
        {
            CellPrefab nextCell = currentCell.neighborCellPrefabs[index];
            while (nextCell.neighborCellPrefabs[index] != null && nextCell.fill == null)
            {
                nextCell = nextCell.neighborCellPrefabs[index];
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                Slide(currentCell, index);
            }
        }

        if (currentCell.neighborCellPrefabs[index] == null)
        {
            return;
        }
        Slide(currentCell.neighborCellPrefabs[index], index);
    }

    private void CellCheck()
    {
        if (fill == null)
        {
            return;
        }

        foreach (CellPrefab neighborCell in neighborCellPrefabs)
        {
            if (neighborCell != null && neighborCell.fill != null && neighborCell.fill.GetCellValue() == fill.GetCellValue())
            {
                return;
            }
        }

        GameController.Instance.GameOverCheck();
    }
}