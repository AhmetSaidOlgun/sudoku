using System;
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

    public List<SudokuItem> changebleSudokuItems = new List<SudokuItem>();

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
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = Instantiate(sudokuControllerPrefab, sudokuControllerPanel.transform);
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
            _currentSudokuItem = sudokuItem;
        }
    }

    private void ControllerItemClicked(ControllerItem controllerItem)
    {
        bool gameFinished = true;
        if (_currentSudokuItem != null)
        {
            _currentSudokuItem.SetItemNumber(controllerItem.number);
            Managers.SudokuManager.ItemValueChanged(_currentSudokuItem);
            
            foreach (var item in changebleSudokuItems)
            {
                if (item.Number == 0)
                {
                    gameFinished = false;
                }
            }
        }

        if (gameFinished)
        {
            Debug.Log("Oyun bitti");
        }
    }

    public void ClearSudoku()
    {
        _gameObject = null;
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
        bool showSuccessfulText = true;

        if (sudokuItem.isChangeable)
        {
            if (_finalObject.values[sudokuItem._row, sudokuItem._column] == sudokuItem.Number)
            {
                sudokuItem.SuccessfulItem();
            }
            else
            {
                sudokuItem.FailItem();
                showSuccessfulText = false;
            }
        }
    }
}