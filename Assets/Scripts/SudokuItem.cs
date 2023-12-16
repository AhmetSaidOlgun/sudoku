using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SudokuItem
{
    private GameObject _instance;

    public int? Number = 0;
    public int _row;
    public int _column;

    [SerializeField] private Color _failPressedColor;

    public bool isChangeable = true;

    public SudokuItem(GameObject instance, int row, int column)
    {
        _instance = instance;
        _row = row;
        _column = column;
    }

    public void SuccessfulItem()
    {
        var button = _instance.GetComponent<Button>();
        isChangeable = false;
        button.interactable = false;
        var buttonColors = button.colors;
        buttonColors.normalColor = Color.green;
        buttonColors.disabledColor = Color.green;
        buttonColors.highlightedColor = Color.green;
        buttonColors.selectedColor = Color.green;
        buttonColors.pressedColor = Color.green;
        button.colors = buttonColors;
    }

    public void FailItem()
    {
        var button = _instance.GetComponent<Button>();
        isChangeable = true;
        button.interactable = true;
        var buttonColors = button.colors;
        buttonColors.normalColor = Color.red;
        buttonColors.disabledColor = Color.red;
        buttonColors.highlightedColor = Color.red;
        buttonColors.selectedColor = Color.red;
        buttonColors.pressedColor = HexToColor("C31919");
        button.colors = buttonColors;
    }

    public void SetItemNumber(int? number)
    {
        Number = 0;
        if (number == null) return;
        Number = number;
        _instance.GetComponent<Image>().sprite = Managers.ImageManager.images[(int)number - 1];
    }

    public void ClearSudoku()
    {
        _instance.GetComponent<Button>().interactable = true;
        _instance.GetComponent<Image>().sprite = null;
        var colorBlock = _instance.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        colorBlock.pressedColor = HexToColor("C8C8C8");
        colorBlock.highlightedColor =HexToColor("D6D6D6");
        colorBlock.disabledColor =HexToColor("D6D6D6");
        colorBlock.selectedColor = HexToColor("D6D6D6");
        _instance.GetComponent<Button>().colors = colorBlock;
    }

    public void SetInteraction(bool isInteractable)
    {
        _instance.GetComponent<Button>().interactable = isInteractable;
    }

    public void SetInteractableButtonColors()
    {
        var a = _instance.GetComponent<Button>().colors;
        a.disabledColor = Color.white;
        a.normalColor = Color.white;
        a.highlightedColor = Color.white;
        a.selectedColor = Color.white;
        _instance.GetComponent<Button>().colors = a;
    }
    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
}
