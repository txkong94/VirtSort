using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : ISortingAlgorithm {
    private readonly bool DEBUG = true;
    private List<int[]> sortSteps;
    private List<List<int?[]>> sortChange;
    private int currentStep;

    public int CurrentStep() {

        return currentStep;

    }

    public int[] ArrayToSort 
	{ 
		//TODO probably some input validation or something maybe idk.
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
            if(DEBUG) debugShit();
        }
    }

    private int[] _arrayToSort;


    public BubbleSort() 
    {
        if(DEBUG) Debug.Log("Makes new algo");
        sortChange = new List<List<int?[]>>();
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
    public string GetInstruction()
    {
        if(currentStep <= 1) return "Welcome! Perform a step in the bubblesort algorithm.";
        else if (currentStep >= sortSteps.Count - 1) return "Congratulations! You have successfully solved the array!";
        else return string.Format("You have done {0} iteration of the algorithm! Keep it up!", (currentStep-1));
    }

    public bool Next()
    {
        if (currentStep < sortSteps.Count - 1)
        {
            currentStep++;
        }
        if (currentStep == sortSteps.Count - 1)
        {
            Debug.Log("Is Final Step: " + currentStep);
            return true;
        }

        return false;
    }

    public int[] PeekNext()
    {
        if (currentStep < sortSteps.Count - 1)
        {
            return sortSteps[currentStep + 1];
        }
        return Current();
    }

    public int[] Previous()
    {
        if(currentStep != 0) currentStep--;
        return sortSteps[currentStep];
    }

	public int[] Current() 
	{
        return sortSteps[currentStep];
    }

    public bool IsSolved()
    {
        return (currentStep >= sortSteps.Count - 1);
    }

    public void Solve()
    {
		bool swapped = true;
        int temp;
        for (int i = 0; i <= (_arrayToSort.Length - 1) && swapped; i++) 
		{
            List<int?[]> iterationChanges = new List<int?[]>();
            swapped = false;
            for (int j = 0; j < _arrayToSort.Length - 1; j++) {
                int?[] stepChange = new int?[_arrayToSort.Length];
                
                stepChange[j] = 0;
                stepChange[j + 1] = 0;
                if (_arrayToSort[j + 1] < _arrayToSort[j]) 
                {
                    stepChange[j] = 1;
                    stepChange[j + 1] = -1;
                    temp = _arrayToSort[j];
                    _arrayToSort[j] = _arrayToSort[j + 1];
                    _arrayToSort[j + 1] = temp;
                    swapped = true;
                    //Debug.Log("Did a thing");
                }
                iterationChanges.Add(stepChange);
            }
            sortChange.Add(iterationChanges);
            int[] cloneArray = (int[]) _arrayToSort.Clone();
            sortSteps.Add(cloneArray);
            if(!swapped) break;

        }
        if(DEBUG) printSortChange();
    }

    private void printSortChange()
    {
        int b = 0;
        foreach (List<int?[]> changeList in sortChange) 
        {
            
            string s = "";
            foreach (int?[] change in changeList) 
            {
                s += "[";
                foreach(int? i in change) 
                {
                    s += string.Format("{0}, ", i != null ? i.ToString() : "null");
                }
                s += "]\n";
            }
            s += "\n";
            Debug.Log("Iteration " + b + ": \n" + s);
            b++;
        }
    }
}
