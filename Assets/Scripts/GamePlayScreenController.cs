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
       
    }

    private void OnEnable()
    {
        UpdateMoveCountText();
        Managers.SudokuManager.sudokuCreated += UpdateMoveCountText;
        Managers.SudokuManager.controllerItemClicked += UpdateMoveCountText;
    }

    private void OnDisable()
    {
        Managers.SudokuManager.controllerItemClicked -= UpdateMoveCountText;
        Managers.SudokuManager.sudokuCreated -= UpdateMoveCountText;
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

    private void UpdateMoveCountText()
    {
        moveCount.text = Managers.SudokuManager.moveCount.ToString();
    }
}