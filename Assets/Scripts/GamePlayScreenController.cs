using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreenController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private TMP_Text moveCount;

    private void Awake()
    {
        Managers.SudokuManager.sudokuCreated += s => UpdateMoveCountText(Managers.SudokuManager.moveCount.ToString()); 
        Managers.SudokuManager.controllerItemClicked += s => UpdateMoveCountText(Managers.SudokuManager.moveCount.ToString()); 
    }

    private void OnDestroy()
    {
        Managers.SudokuManager.sudokuCreated -= s => UpdateMoveCountText(Managers.SudokuManager.moveCount.ToString());
        Managers.SudokuManager.controllerItemClicked -= s => UpdateMoveCountText(Managers.SudokuManager.moveCount.ToString());
    }
    private void Start()
    {
        backButton.onClick.AddListener((() => { BackButtonClicked(); }));
    }

    private void BackButtonClicked()
    {
        gameObject.SetActive(false);
        startScreen.SetActive(true);
        Managers.SudokuManager.ClearSudoku();
    }

    private void UpdateMoveCountText(string move)
    {
        moveCount.text = move;
    }
}