using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class SudokuGenerator
{
    private static SudokuObjects _finalSudokuObject;

    public static void CreateSudokuObjects(out SudokuObjects finalObject,out SudokuObjects gameObject )
    {
        _finalSudokuObject = null;
        SudokuObjects sudokuObjects = new SudokuObjects();
        CreateRandomGroups(sudokuObjects);
        if (TryToSolve(sudokuObjects))
        {
            sudokuObjects = _finalSudokuObject;
        }
        else
        {
            throw new SystemException("Something went wrong");
        }

        finalObject = sudokuObjects;
        gameObject = RemoveSudokuObjects(sudokuObjects);
    }

    private static SudokuObjects RemoveSudokuObjects(SudokuObjects sudokuObjects)
    {
        int easy = Random.Range(75, 80);
        int medium = Random.Range(55, 70);
        int hard = Random.Range(35, 50);
        SudokuObjects newSudokuObject = new SudokuObjects();
        newSudokuObject.values = (int[,]) sudokuObjects.values.Clone();
        List<Tuple<int, int>> Values = GetValues();
        int endValueIndex = GameSettings.easyMiddleHardNumber switch
        {
            1 => easy, 
            2 => medium,
            3 => hard
        };

        if (GameSettings.easyMiddleHardNumber == 1)
        {
            Managers.SudokuManager.moveCount = (81 - easy) + 3;
        }
        else if((GameSettings.easyMiddleHardNumber == 2))
        {
            Managers.SudokuManager.moveCount = (81 - medium) + 4;
        }
        else if((GameSettings.easyMiddleHardNumber == 3))
        {
            Managers.SudokuManager.moveCount = (81 - hard) + 5;
        }

        bool isFinished = false;
        while (!isFinished)
        {
            int index = Random.Range(0, Values.Count);
            var searchedIndex = Values[index];
            SudokuObjects nextSudokuObject = new SudokuObjects();
            nextSudokuObject.values = (int[,]) newSudokuObject.values.Clone();
            nextSudokuObject.values[searchedIndex.Item1, searchedIndex.Item2] = 0;
            if (TryToSolve(nextSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }
            Values.RemoveAt(index);
            if (Values.Count < endValueIndex)
            {
                isFinished = true;
            }
            
        }

        return newSudokuObject;
    }

    private static List<Tuple<int, int>> GetValues()
    {
        List<Tuple<int, int>> Values = new List<Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Values.Add(new Tuple<int, int>(i, j));
            }
        }

        return Values;
    }

    private static bool TryToSolve(SudokuObjects sudokuObjects, bool onlyOne = false)
    {
        if (HasEmptyFieldsToFill(sudokuObjects, out int row, out int column, onlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObjects, row, column);
            foreach (var possibleValue in possibleValues)
            {
                SudokuObjects nextSudokuObject = new SudokuObjects();
                nextSudokuObject.values = (int[,]) sudokuObjects.values.Clone();
                nextSudokuObject.values[row, column] = possibleValue;
                if (TryToSolve(nextSudokuObject, onlyOne))
                {
                    return true;
                }
            }
        }

        if (HasEmptyFields(sudokuObjects))
        {
            return false;
        }

        _finalSudokuObject = sudokuObjects;
        return true;
    }

    private static bool HasEmptyFields(SudokuObjects sudokuObjects)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObjects.values[i, j] == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static List<int> GetPossibleValues(SudokuObjects sudokuObjects, int row, int column)
    {
        List<int> possibleValues = new List<int>();
        for (int value = 1; value < 10; value++)
        {
            if (sudokuObjects.IsPossibleNumberInPosition(value, row, column))
            {
                possibleValues.Add(value);
            }
        }

        return possibleValues;
    }

    private static bool HasEmptyFieldsToFill(SudokuObjects sudokuObjects, out int row, out int column,
        bool onlyOne = false)
    {
        row = 0;
        column = 0;
        int amountOfPossibleValues = 10;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObjects.values[i, j] == 0)
                {
                    int currentAmount = GetPossibleAmountOfValues(sudokuObjects, i, j);
                    if (currentAmount != 0)
                    {
                        if (currentAmount < amountOfPossibleValues)
                        {
                            amountOfPossibleValues = currentAmount;
                            row = i;
                            column = j;
                        }
                    }
                }
            }
        }

        if (onlyOne)
        {
            if (amountOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (amountOfPossibleValues == 10)
        {
            return false;
        }

        return true;
    }

    private static int GetPossibleAmountOfValues(SudokuObjects sudokuObjects, int row, int column)
    {
        int amount = 0;
        for (int value = 1; value < 10; value++)
        {
            if (sudokuObjects.IsPossibleNumberInPosition(value, row, column))
            {
                amount++;
            }
        }

        return amount;
    }

    public static void CreateRandomGroups(SudokuObjects sudokuObjects)
    {
        List<int> values = new List<int>() {0, 1, 2};
        int index = Random.Range(0, values.Count);
        InsertRandomGroups(sudokuObjects, 1 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroups(sudokuObjects, 4 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroups(sudokuObjects, 7 + values[index]);
    }

    public static void InsertRandomGroups(SudokuObjects sudokuObjects, int group)
    {
        global::SudokuObjects.GetGroupIndex(group, out int startRow, out int startColumn);
        List<int> values = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        for (int row = startRow; row < startRow + 3; row++)
        {
            for (int column = startColumn; column < startColumn + 3; column++)
            {
                int index = Random.Range(0, values.Count);
                sudokuObjects.values[row, column] = values[index];
                values.RemoveAt(index);
            }
        }
    }
}