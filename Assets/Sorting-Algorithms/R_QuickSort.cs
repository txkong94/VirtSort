using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class R_QuickSort : R_ISortingAlgorithm {

    private List<GameObject> triggers;
    private List<GameObject> boxes;
    private List<GameObject> boxesOnInit;

    private int maxIteration; // Total iterations needed to complete insertion sort.
    private bool isRecursive; // Flag for using a recursive implementation of insertion sort.

    //
    // Internal algorithm variables
    //

    private GameObject leftTrigger;
    private GameObject rightTrigger;
    private GameObject pivotTrigger;
    private R_TriggerScript leftTriggerScript;
    private R_TriggerScript rightTriggerScript;
    private R_TriggerScript pivotTriggerScript;
    private GameObject leftBox;
    private GameObject rightBox;
    private GameObject pivotBox;
    private R_BoxScript leftBoxScript;
    private R_BoxScript rightBoxScript;
    private R_BoxScript pivotBoxScript;
    // current index to be compared (with index-1). Iteration is done if index < 1
    bool boxesChecked;
    // Set to True when the algorithm has been completed.
    bool isCompleted = false;
    // Set to True after Completed() has run, so that it isn't run indefinitely.
    bool hasPivot = false;
    // Set to true if an element on the left of pivot is greater than pivot.
    bool leftSideFound = false;
    bool rightSideFound = false;

    bool endIteration = false;
    bool startRecursion = false;

    int pivotIndex;
    int leftIndex;
    int rightIndex;

    R_QuickSort leftSideAlgorithm;
    R_QuickSort rightSideAlgorithm;

    int left;
    int right;
    bool enableHighlighting;

    bool isRoot;

    public bool EnableHighlighting
    {
        get
        {
            return enableHighlighting;
        }

        set
        {
            enableHighlighting = value;
        }
    }

    public R_QuickSort(ref List<GameObject> triggers, ref List<GameObject> boxes, int? startIndex = null, int? endIndex = null, bool isRoot = false)
    {
        this.isRoot = isRoot;
        this.triggers = triggers;
        this.boxes = boxes;
        boxesOnInit = new List<GameObject>(boxes);
        left = startIndex == null ? 0 : (int) startIndex;
        right = endIndex == null ? triggers.Count - 1 : (int) endIndex;

        leftIndex = left;
        rightIndex = right;
        //Debug.Log("leftIndex: " + leftIndex + ", rightIndex: " + rightIndex);

        InitializeStep();
    }

    public void Update()
    {
        // Base case
        if(right - left < 1)
        {
            
            Debug.Log("right - left: " + (right - left));
            isCompleted = true;
            //Debug.Log("isCompleted");
            triggers[left].GetComponent<R_TriggerScript>().State = ETriggerState.correct;
            Completed();
            return;
        }

        if(startRecursion)
        {
            //TODO: Recursive shit here.
            Recursion();
            //Completed();
            return;
        }

        if (!hasPivot)
        {
            //TODO: Let user pick pivot at random.
            pivotIndex = left;
            leftIndex++;
            hasPivot = true;
            InitializeStep();
        }

        if(leftIndex > rightIndex && !endIteration) 
        {
            //Debug.Log("right smaller than left");
            
            EndIteration();
        }

        if (CheckBoxesGrabbed())
        {
            boxesChecked = true;
        }
        
        if(boxesChecked && !CheckBoxesGrabbed() && IsCorrectlyPositioned())
        {
            //Debug.Log("IsCorrectlyPositioned when endIteration: " + endIteration);
            
            if(endIteration)
            {
                SwapBoxesInList(rightIndex, pivotIndex);
                pivotIndex = rightIndex;
                SetTriggersInactive();
                triggers[pivotIndex].GetComponent<R_TriggerScript>().State = ETriggerState.correct;
                //Debug.Log("Pivot set to Correct.");
                
                startRecursion = true;
            }
            else
            {
                EndStep();
                InitializeStep();
            }
        }

        if(isRoot && isCompleted)
        {
            Completed();       
        }
    }

    private void Completed()
    {
        if (!isRoot)
            return;

        foreach (GameObject box in boxes)
        {
            box.GetComponent<R_BoxScript>().Visibility = EVisibilityOptions.AlwaysVisible;
        }
    }

    private void Recursion()
    {
        if (leftSideAlgorithm == null && pivotIndex > left)
        {
            leftSideAlgorithm = new R_QuickSort(ref triggers, ref boxes, left, pivotIndex - 1);
        }

        if (rightSideAlgorithm == null && pivotIndex < right)
        {
            rightSideAlgorithm = new R_QuickSort(ref triggers, ref boxes, pivotIndex + 1, right);
            rightSideAlgorithm.setBoxesActive(false);
        }

        boxes[pivotIndex].SetActive(false);

        if(leftSideAlgorithm == null && rightSideAlgorithm != null)
        {
            rightSideAlgorithm.setBoxesActive(true);
        }
        
        if (leftSideAlgorithm != null && !leftSideAlgorithm.isCompleted)
        {
            leftSideAlgorithm.Update();
            if(leftSideAlgorithm.isCompleted)
            {
                leftSideAlgorithm.setBoxesActive(false);
                if(rightSideAlgorithm != null)
                    rightSideAlgorithm.setBoxesActive(true);
            }
        }
        else if (rightSideAlgorithm != null && !rightSideAlgorithm.isCompleted)
            rightSideAlgorithm.Update();
        else
        {
            isCompleted = true;
            SetTriggersCorrect();

            if(isRoot)
            {
                setBoxesActive(true);
                foreach (GameObject box in boxes)
                {
                    box.GetComponent<R_BoxScript>().Visibility = EVisibilityOptions.AlwaysVisible;
                }
            }
        }
            
        return;
    }

    void setBoxesActive(bool active)
    {
        for (int i = left; i <= right; i++)
        {
            boxes[i].SetActive(active);
        }
    }

    private bool CheckBoxesGrabbed()
    {
        if(endIteration)
            return rightBoxScript.IsGrabbed && pivotBoxScript.IsGrabbed;
        else if(!leftSideFound)
            return leftBoxScript.IsGrabbed && pivotBoxScript.IsGrabbed;
        else if(!rightSideFound)
            return rightBoxScript.IsGrabbed && pivotBoxScript.IsGrabbed;
        return leftBoxScript.IsGrabbed && rightBoxScript.IsGrabbed;
    }

    private void EndStep() 
    {
        if(!leftSideFound)
        {
            //Debug.Log("left side not found");
            
            //leftTriggerScript.State = ETriggerState.intermediate;
            if(leftBoxScript.Value > pivotBoxScript.Value)
                leftSideFound = true;
            else
                leftIndex++;
        }
        else if(!rightSideFound)
        {
            //rightTriggerScript.State = ETriggerState.intermediate;
            if(rightBoxScript.Value <= pivotBoxScript.Value)
                rightSideFound = true;
            else
                rightIndex--;
        }
        
        if(leftSideFound && rightSideFound)
        {
            //Debug.Log("left and right side found.");
            
            if(IsCorrectlyPositioned())
            {
                SwapBoxesInList(leftIndex, rightIndex);
                leftIndex++;
                rightIndex--;
                leftSideFound = false;
                rightSideFound = false;
            }
        }
    }

    private void EndIteration()
    {
        SetTriggersInactive();
        rightTriggerScript.State = ETriggerState.intermediate;
        pivotTriggerScript.State = ETriggerState.intermediate;

        rightTriggerScript.IsLocked = false;
        pivotTriggerScript.IsLocked = false;

        endIteration = true;
    }

    private bool IsCorrectlyPositioned()
    {
        if(endIteration)
            return rightTriggerScript.ContainsBox(pivotBox) && pivotTriggerScript.ContainsBox(rightBox);
        else if(leftSideFound && rightSideFound)
            return leftTriggerScript.ContainsBox(rightBox) && rightTriggerScript.ContainsBox(leftBox);
        else
            return leftTriggerScript.ContainsBox(leftBox) && rightTriggerScript.ContainsBox(rightBox) && pivotTriggerScript.ContainsBox(pivotBox);
    }

	private void SwapBoxesInList(int index1, int index2)
	{
        GameObject temp = boxes[index2];
		boxes[index2] = boxes[index1];
		boxes[index1] = temp;
	}

    public void InitializeStep()
    {
        if (!hasPivot)
        {
            PivotStep();
            return;
        }

        if(leftIndex <= right)
        {
            leftTrigger = triggers[leftIndex];
            leftTriggerScript = leftTrigger.GetComponent<R_TriggerScript>();
            leftBox = boxes[leftIndex];
            leftBoxScript = leftBox.GetComponent<R_BoxScript>();
        }
        rightTrigger = triggers[rightIndex];
        pivotTrigger = triggers[pivotIndex];
        rightTriggerScript = rightTrigger.GetComponent<R_TriggerScript>();
        pivotTriggerScript = pivotTrigger.GetComponent<R_TriggerScript>();
        rightBox = boxes[rightIndex];
        pivotBox = boxes[pivotIndex];
        rightBoxScript = rightBox.GetComponent<R_BoxScript>();
        pivotBoxScript = pivotBox.GetComponent<R_BoxScript>();

        SetTriggersInactive();

        if (!leftSideFound && !(leftIndex > right))
        {
            leftTriggerScript.IsLocked = false;
            pivotTriggerScript.IsLocked = false;
            leftTriggerScript.State = ETriggerState.incorrect;
            pivotTriggerScript.State = ETriggerState.pivot;
        }
        else if (leftSideFound && rightSideFound)
        {
            pivotTriggerScript.IsLocked = true;
            leftTriggerScript.IsLocked = false;
            rightTriggerScript.IsLocked = false;
            pivotTriggerScript.State = ETriggerState.pivot;
            leftTriggerScript.State = ETriggerState.intermediate;
            rightTriggerScript.State = ETriggerState.intermediate;
        }
        else
        {
            rightTriggerScript.IsLocked = false;
            pivotTriggerScript.IsLocked = false;
            if(leftIndex <= right)
                leftTriggerScript.State = ETriggerState.intermediate;
            pivotTriggerScript.State = ETriggerState.pivot;
            rightTriggerScript.State = ETriggerState.incorrect;
        }

        boxesChecked = false;
    }

    private void SetTriggersInactive()
    {
        for (int i = left; i <= right; i++)
        {
            triggers[i].GetComponent<R_TriggerScript>().IsLocked = true;
            triggers[i].GetComponent<R_TriggerScript>().State = ETriggerState.inactive;
        }
    }
    private void SetTriggersCorrect()
    {
        for (int i = left; i <= right; i++)
        {
            triggers[i].GetComponent<R_TriggerScript>().IsLocked = true;
            triggers[i].GetComponent<R_TriggerScript>().State = ETriggerState.correct;
            if(isRoot)
                triggers[i].GetComponent<R_TriggerScript>().SetHasTriggerColour(true);
        }
    }
    private void PivotStep()
    {
        for (int i = left; i <= right; i++)
        {
            triggers[i].GetComponent<R_TriggerScript>().IsLocked = false;
        }
    }


    public int GetBoxIndex(GameObject searchBox)
    {
        for (int i = 0; i < boxes.Count; ++i)
        {
            if (boxes[i] == (searchBox))
                return i;
        }

        return -1;
    }
    
    public GameObject GetTriggerFromIndex(int i)
    {
        return triggers[i];
    }

    public void ResetStep()
    {
        // A completed algorithm will not be reset.
        // An algorithm without pivot is not currently active (not being solved by the player)
        if (isCompleted || !hasPivot)
            return;

        // Propagate reset to recursive algorithms if they exist.
        // If both leftSide and rightSide exists there is no way to know which one is currently active,
        // so ResetStep() is called on both, and internal logic will stop the reset on the 
        // inactive algorithm (the one not currently being solved by the player).
        if (leftSideAlgorithm != null)
            leftSideAlgorithm.ResetStep();
        if (rightSideAlgorithm != null)
            rightSideAlgorithm.ResetStep();

        // Do not reset when recursive algorithms exists (ref. code immediately above)
        if (startRecursion)
            return;

        // Reset boxes list
        boxes = new List<GameObject>(boxesOnInit);

        // Move boxes back to correct positions
        for (int i = left; i <= right; ++i)
        {
            boxes[i].GetComponent<R_BoxScript>().Move(triggers[i].transform.position);
        }

        // Reset left and right indexes
        leftIndex = left;
        if (hasPivot)
            pivotIndex = leftIndex++;
        rightIndex = right;

        // Reset flags
        leftSideFound = false;
        rightSideFound = false;
        endIteration = false;

        InitializeStep();
    }
    
}
