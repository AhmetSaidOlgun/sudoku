using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreenController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject startScreen;

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
}