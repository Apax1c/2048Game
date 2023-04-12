using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillCellPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image fillImage;

    private int cellValue;
    private float speed = 5000;

    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        if (transform.parent.GetChild(0) != this.transform)
        {
            Destroy(transform.parent.GetChild(0).gameObject);
        }
    }

    private int GetColorIndex(int newCellValue)
    {
        int index = 0;
        while (newCellValue != 1)
        {
            index++;
            newCellValue /= 2;
        }

        index--;
        return index;
    }

    public void SetValueUpdate(int newCellValue)
    {
        cellValue = newCellValue;
        valueText.text = cellValue.ToString();

        int colorIndex = GetColorIndex(cellValue);
        fillImage.color = GameController.Instance.fillColors[colorIndex];
    }

    public void Double()
    {
        SetValueUpdate(cellValue * 2);
        GameController.Instance.ScoreUpdate(cellValue);
        GameController.Instance.WinningCheck(cellValue);
    }

    public int GetCellValue()
    {
        return cellValue;
    }
}