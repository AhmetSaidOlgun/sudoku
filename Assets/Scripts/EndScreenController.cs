using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] protected GamePlayScreenController _gamePlayScreen;
    [SerializeField] protected Button _playAgainButton;

    protected virtual void Awake()
    {
        _playAgainButton.onClick.AddListener(PlayAgainButton);
    }

    protected virtual void PlayAgainButton()
    {
        Managers.SudokuManager.ClearSudoku();
        Managers.SudokuManager.CreateSudokuObject();
        _gamePlayScreen.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Managers.SudokuManager.sudokuCreated.Invoke(Managers.SudokuManager.moveCount.ToString());
    }
}