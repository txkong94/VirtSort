using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveInsertionSort : ISortingAlgorithm {

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

	public RecursiveInsertionSort()
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
        RecursiveInsertion(_arrayToSort);
    }

    private void RecursiveInsertion(int[] arrayToSort, int arrayLength = -1)
    {
        if(arrayLength == -1) arrayLength = arrayToSort.Length;
        
        //Base case. Array with elements = 1 is sorted.
        if(arrayLength <= 1) return;

        //Recursively sort array of length - 1
        RecursiveInsertion(arrayToSort, arrayLength - 1);

        //Sort the last element correctly inside the already sorted array from above.
        int elementToSort = arrayToSort[arrayLength - 1];
        int currentElement = arrayLength - 2;
        while(currentElement >= 0 && arrayToSort[currentElement] > elementToSort)
        {
            arrayToSort[currentElement + 1] = arrayToSort[currentElement];
            currentElement--;
        }
        arrayToSort[currentElement + 1] = elementToSort;

        //Insert recursion step into array of steps.
        int[] cloneArray = (int[]) _arrayToSort.Clone();
        sortSteps.Add(cloneArray);
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
