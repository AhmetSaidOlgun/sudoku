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

    public bool isChangeable = true;

    public SudokuItem(GameObject instance, int row, int column)
    {
        _instance = instance;
        _row = row;
        _column = column;
    }

    public void SuccessfulItem()
    {
        var buttonColors = _instance.GetComponent<Button>().colors;
        buttonColors.normalColor = Color.green;
        buttonColors.disabledColor = Color.green;
        buttonColors.highlightedColor = Color.green;
        buttonColors.pressedColor = Color.green;
        _instance.GetComponent<Button>().colors = buttonColors;
    }

    public void FailItem()
    {
        var buttonColors = _instance.GetComponent<Button>().colors;
        buttonColors.normalColor = Color.red;
        buttonColors.disabledColor = Color.red;
        buttonColors.highlightedColor = Color.red;
        buttonColors.pressedColor = Color.red;
        _instance.GetComponent<Button>().colors = buttonColors;
    }
    
    public void SetItemNumber(int? number)
    {
        if (number == null) return;
        Number = number;
        _instance.GetComponent<Image>().sprite = Managers.ImageManager.images[(int) number - 1];
    }

    public void ClearSudoku()
    {
        _instance.GetComponent<Image>().sprite = null;
        var colorBlock = _instance.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        _instance.GetComponent<Button>().colors = colorBlock;
    }
}