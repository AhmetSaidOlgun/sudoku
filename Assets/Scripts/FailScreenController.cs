using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailScreenController : EndScreenController
{
    [SerializeField] private Button _continueButton;

    protected override void Awake()
    {
        _continueButton.onClick.AddListener(ContinueButton);
        base.Awake();
    }

    protected override void PlayAgainButton()
    {
        base.PlayAgainButton();
    }

    private async void ContinueButton()
    {
        _continueButton.interactable = false;
        await Managers.AdManager.LoadRewardedAd();
        await Managers.AdManager.ShowRewardedAd();
        _continueButton.interactable = true;
        Managers.SudokuManager.SetControllerItemsInteraction(true);
        Managers.SudokuManager.moveCount = 3;
        Managers.SudokuManager.sudokuCreated.Invoke(Managers.SudokuManager.moveCount.ToString());
        gameObject.SetActive(false);
        _gamePlayScreen.gameObject.SetActive(true);
    }
}