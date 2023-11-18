using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    [SerializeField] private GameObject GamePlayScreen;

    private void Start()
    {
        easyButton.onClick.AddListener((() => { EasyButtonClicked(); }));
        mediumButton.onClick.AddListener((() => { MediumButtonClicked(); }));
        hardButton.onClick.AddListener((() => { HardButtonClicked(); }));
    }

    private void EasyButtonClicked()
    {
        GameSettings.easyMiddleHardNumber = 1;
        gameObject.SetActive(false);
        GamePlayScreen.SetActive(true);
        Managers.SudokuManager.sudokuCreated.Invoke(Managers.SudokuManager.moveCount.ToString());
    }

    private void MediumButtonClicked()
    {
        GameSettings.easyMiddleHardNumber = 2;
        gameObject.SetActive(false);
        GamePlayScreen.SetActive(true);
        Managers.SudokuManager.sudokuCreated.Invoke(Managers.SudokuManager.moveCount.ToString());
    }

    private void HardButtonClicked()
    {
        GameSettings.easyMiddleHardNumber = 3;
        gameObject.SetActive(false);
        GamePlayScreen.SetActive(true);
        Managers.SudokuManager.sudokuCreated.Invoke(Managers.SudokuManager.moveCount.ToString());
    }
}