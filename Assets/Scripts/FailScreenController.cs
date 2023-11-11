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

    private void ContinueButton()
    {
        gameObject.SetActive(false);
        _gamePlayScreen.gameObject.SetActive(true);
        //Reklam çıkar
        //Hak arttır
    }
}
