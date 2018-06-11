using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : ISortingAlgorithm {

    private List<int[]> sortSteps;

    public int[] ArrayToSort 
	{ 
		get 
		{
            return sortSteps[currentStep]; 
		}
        set 
		{
            sortSteps = new List<int[]>();
            sortSteps.Add(value);
            _arrayToSort = (int[]) value.Clone();
            currentStep = 0;
            Solve();
            debugShit();
        }
	}

    private int[] _arrayToSort;

    private int currentStep;

	public InsertionSort()
	{

	}
    public int[] Current()
    {
        return sortSteps[currentStep];
    }

    public int CurrentStep()
    {
        return currentStep;
    }

    public string GetInstruction()
    {
        return "InsertionSort instructions";
    }

    public bool Next()
    {
        if (currentStep < sortSteps.Count - 1)
        {
            currentStep++;
        }
        if (currentStep == sortSteps.Count - 1)
        {
            currentStep++;
            Debug.Log("Is Final Step: " + currentStep);
            return true;
        }
        else return false;
    }

    public bool IsSolved()
    {
        return (currentStep >= sortSteps.Count - 1);
    }

    public int[] Previous()
    {
        if(currentStep != 0) currentStep--;
        return sortSteps[currentStep];
    }

    public void Solve()
    {
        for (int i = 0; i < _arrayToSort.Length - 1; i++)
		{
            for (int j = i + 1; j > 0; j--)
            {
                if (_arrayToSort[j - 1] > _arrayToSort[j])
                {
                    int temp = _arrayToSort[j - 1];
                    _arrayToSort[j - 1] = _arrayToSort[j];
                    _arrayToSort[j] = temp;
                }
            }
			int[] cloneArray = (int[]) _arrayToSort.Clone();
            sortSteps.Add(cloneArray);
        }
    }
    private void debugShit() {
        int b = 0;
        foreach (int[] step in sortSteps) {
            string s = "[";
            foreach (int i in step) {
                s += string.Format("{0}, ", i);
            }
            s += "]";
            Debug.Log("step " + b + ": " + s);
            b++;
        }
    }
}
