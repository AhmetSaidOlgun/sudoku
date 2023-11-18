using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SudokuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject sudokuFieldPrefab;
    [SerializeField] private GameObject sudokuFieldPanel;
    [SerializeField] private GameObject sudokuControllerPrefab;
    [SerializeField] private GameObject sudokuControllerPanel;


    [SerializeField] private GameObject congratulationsScreen;
    [SerializeField] private GameObject failedScreen;

    private SudokuItem _currentSudokuItem;
    private SudokuObjects _gameObject;
    private SudokuObjects _finalObject;

    private Dictionary<Tuple<int, int>, SudokuItem> _sudokuItemDic = new Dictionary<Tuple<int, int>, SudokuItem>();

    public List<SudokuItem> changebleSudokuItems;
    private List<Button> sudokuButtons;

    public int moveCount;
    public Action<string> controllerItemClicked;
    public Action<string> sudokuCreated;

    private void Awake()
    {
        Managers.SudokuManager = this;
        GenerateSudokuItems();
        GenerateControllerItems();
    }

    private void OnEnable()
    {
        CreateSudokuObject();
    }

    private void OnDisable()
    {
        ClearSudoku();
    }

    public void FinishButton()
    {
        bool showSuccessfulText = true;
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                SudokuItem sudokuItem = _sudokuItemDic[new Tuple<int, int>(row, column)];
                if (sudokuItem.isChangeable)
                {
                    if (_finalObject.values[row, column] != sudokuItem.Number)
                    {
                        showSuccessfulText = false;
                    }
                }
            }
        }

        if (showSuccessfulText)
        {
            congratulationsScreen.SetActive(true);
        }
        else
        {
            failedScreen.SetActive(true);
        }
    }

    public void CreateSudokuObject()
    {
        moveCount = 5;
        SetControllerItemsInteraction(true);
        changebleSudokuItems = new List<SudokuItem>();
        SudokuGenerator.CreateSudokuObjects(out SudokuObjects finalObject, out SudokuObjects gameObject);
        _gameObject = gameObject;
        _finalObject = finalObject;
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var currenValue = _gameObject.values[row, column];
                if (currenValue != 0)
                {
                    SudokuItem fieldObject = _sudokuItemDic[new Tuple<int, int>(row, column)];
                    fieldObject.SetItemNumber(currenValue);
                    fieldObject.isChangeable = false;
                }
                else
                {
                    SudokuItem fieldObject = _sudokuItemDic[new Tuple<int, int>(row, column)];
                    changebleSudokuItems.Add(fieldObject);
                }
            }
        }
    }

    private void GenerateSudokuItems()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                GameObject instance = Instantiate(sudokuFieldPrefab, sudokuFieldPanel.transform);
                SudokuItem sudokuItem = new SudokuItem(instance, row, column);
                _sudokuItemDic.Add(new Tuple<int, int>(row, column), sudokuItem);
                instance.GetComponent<Button>().onClick
                    .AddListener(() => SudokuItemClicked(sudokuItem));
            }
        }
    }

    private void GenerateControllerItems()
    {
        sudokuButtons = new List<Button>();
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = Instantiate(sudokuControllerPrefab, sudokuControllerPanel.transform);
            sudokuButtons.Add(instance.GetComponent<Button>());
            instance.GetComponent<Image>().sprite = Managers.ImageManager.images[i - 1];
            ControllerItem controllerItem = new ControllerItem();
            controllerItem.number = i;
            instance.GetComponentInChildren<Button>().onClick.AddListener((() =>
            {
                ControllerItemClicked(controllerItem);
            }));
        }
    }

    private void SudokuItemClicked(SudokuItem sudokuItem)
    {
        Debug.Log(_sudokuItemDic.FirstOrDefault(x => x.Value == sudokuItem).Key);
        if (sudokuItem.isChangeable)
        {
            if (_currentSudokuItem != null)
            {
                _currentSudokuItem.SetInteraction(true);
            }

            _currentSudokuItem = sudokuItem;
            _currentSudokuItem.SetInteraction(false);
        }
    }

    private void ControllerItemClicked(ControllerItem controllerItem)
    {
        bool gameFinished = true;
        if (_currentSudokuItem != null)
        {
            moveCount--;
            bool moveFinished = MoveFinished();
            _currentSudokuItem.SetItemNumber(controllerItem.number);
            ItemValueChanged(_currentSudokuItem);

            foreach (var item in changebleSudokuItems)
            {
                if (item.Number != _finalObject.values[item._row, item._column] && !moveFinished)
                {
                    gameFinished = false;
                }
            }

            controllerItemClicked?.Invoke(moveCount.ToString());
            
            if (gameFinished)
            {
                StartCoroutine(GameFinished(1));
            }
        }
    }

    public void ClearSudoku()
    {
        _gameObject = null;
        changebleSudokuItems = null;
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                SudokuItem fieldObject = _sudokuItemDic[new Tuple<int, int>(row, column)];
                fieldObject.isChangeable = true;
                fieldObject.SetItemNumber(null);
                fieldObject.ClearSudoku();
            }
        }
    }

    public void ItemValueChanged(SudokuItem sudokuItem)
    {
        if (sudokuItem.isChangeable)
        {
            if (_finalObject.values[sudokuItem._row, sudokuItem._column] == sudokuItem.Number)
            {
                sudokuItem.SuccessfulItem();
                Managers.SudokuManager.DeselectSudokuItem();
            }
            else
            {
                sudokuItem.FailItem();
            }
        }
    }

    private bool MoveFinished()
    {
        if (moveCount <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator GameFinished(float duration)
    {
        DeselectSudokuItem();
        SetControllerItemsInteraction(false);
        yield return new WaitForSeconds(duration);
        FinishButton();
    }

    private void DeselectSudokuItem()
    {
        _currentSudokuItem = null;
    }

    public void SetControllerItemsInteraction(bool isInteractable)
    {
        foreach (var button in sudokuButtons)
        {
            button.interactable = isInteractable;
        }
    }
}