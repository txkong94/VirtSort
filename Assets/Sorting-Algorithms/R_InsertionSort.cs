using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class R_InsertionSort : R_ISortingAlgorithm {

    private List<GameObject> triggers;
    private List<GameObject> boxes;
    private List<GameObject> currentState = new List<GameObject>();
    private List<GameObject> boxesOnIterationStart;

    private int maxIteration; // Total iterations needed to complete insertion sort.
    private bool isRecursive; // Flag for using a recursive implementation of insertion sort.

    //
    // Internal algorithm variables
    //

    private GameObject trigger1;
    private GameObject trigger2;
    private R_TriggerScript trigger1script;
    private R_TriggerScript trigger2script;
    private GameObject box1;
    private GameObject box2;
    private R_BoxScript box1script;
    private R_BoxScript box2script;
    // current index to be compared (with index-1). Iteration is done if index < 1
    private int index; 
	// current iteration. 
	private int iteration;
    // Is set to false at the start of every iteration. Set to true when a swap is performed.
    // An iteration is finished when hasSwapInStep is false.
    bool hasSwapInStep;
    // Set to True each step when the boxes at position 'index' and 'index-1' are picked up at the same time
    bool boxesChecked;
    // Set to True when the algorithm has been completed.
    bool isCompleted = false;
    // Set to True after Completed() has run, so that it isn't run indefinitely.
    bool completed = false;

    // Variables related to recursive implementation
    R_InsertionSort childAlgorithm;
    bool enableHighlighting;

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

    public R_InsertionSort(ref List<GameObject> triggers, List<GameObject> boxes, bool isRecursive = true)
    {
        this.triggers = triggers;
        this.boxes = boxes;
		currentState.AddRange(this.boxes);
        boxesOnIterationStart = new List<GameObject>(currentState);
        this.isRecursive = isRecursive;
        maxIteration = triggers.Count;
        
        if(this.isRecursive)
            InitializeRecursive();
        else 
            InitializeNonRecursive();
    }

    private void InitializeRecursive()
    {
        List<int> listToSort = new List<int>();

        foreach(GameObject box in boxes)
        {
            listToSort.Add(box.GetComponent<R_BoxScript>().Value);
        }
        listToSort.RemoveAt(listToSort.Count - 1);
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        gameManager.GetComponent<R_GameManager>().InitNewManager();
    }

    private void InitializeNonRecursive()
    {
		iteration = 1;
		index = iteration;
		triggers[0].GetComponent<R_TriggerScript>().State = ETriggerState.intermediate;
        InitializeStep();
    }

    public void Update()
    {
        if (box1script.IsGrabbed && box2script.IsGrabbed)
        {
            boxesChecked = true;
        }

        if(boxesChecked && !box1script.IsGrabbed && !box2script.IsGrabbed && IsCorrectlyPositioned())
        {
            EndStep();
            if(isCompleted)
                Completed();
            else
                InitializeStep(index);
        }

        return;
    }

    private void Completed()
    {
        if(completed) return;
        Debug.Log("Algorithm is finished");
        foreach (GameObject trigger in triggers)
        {
            var t = trigger.GetComponent<R_TriggerScript>();
            t.IsLocked = true;
            t.State = ETriggerState.correct;
            t.SetHasTriggerColour(true);
        }
        foreach (GameObject box in boxes)
        {
            box.GetComponent<R_BoxScript>().Visibility = EVisibilityOptions.AlwaysVisible;
        }
        completed = true;
    }

    private void EndStep() 
    {
        if(completed) return;
        trigger1script.IsLocked = true;
        trigger2script.IsLocked = true;

        trigger1script.State = ETriggerState.pivot;
        trigger2script.State = ETriggerState.intermediate;
        
        //Makes sure the internal list of boxes is consistent with each step the player does.
        if(hasSwapInStep)
            SwapBoxesInList(index - 1, index);
        
        --index;
        if(!hasSwapInStep || index < 1) // Start new iteration
        {
            boxesOnIterationStart = new List<GameObject>(currentState);
            NewIteration();
        }
    }

    private void NewIteration()
    {
        if(iteration == maxIteration - 1) 
        {
            isCompleted = true;
            return;
        }

        for (int i = 0; i < triggers.Count; i++)
        {
            if (i <= iteration)
                triggers[i].GetComponent<R_TriggerScript>().State = ETriggerState.intermediate;
            else
                triggers[i].GetComponent<R_TriggerScript>().State = ETriggerState.inactive;
        }
        iteration++;

        index = iteration;
        Debug.Log("Starts iteration: " + iteration);
    }

    private bool IsCorrectlyPositioned()
    {
		if(hasSwapInStep)
			return trigger1script.ContainsBox(box2) && trigger2script.ContainsBox(box1);
		else
			return trigger1script.ContainsBox(box1) && trigger2script.ContainsBox(box2);
    }

	private void SwapBoxesInList(int index1, int index2)
	{
		GameObject temp = currentState[index2];
		currentState[index2] = currentState[index1];
		currentState[index1] = temp;
	}

    public void InitializeStep(int index = 1)
    {
        trigger1 = triggers[index - 1];
        trigger2 = triggers[index];
        trigger1script = trigger1.GetComponent<R_TriggerScript>();
        trigger2script = trigger2.GetComponent<R_TriggerScript>();
        box1 = currentState[index - 1];
        box2 = currentState[index];
        box1script = box1.GetComponent<R_BoxScript>();
        box2script = box2.GetComponent<R_BoxScript>();

        int count = 0;
        foreach (GameObject trigger in triggers)
        {
            trigger.GetComponent<R_TriggerScript>().IsLocked = true;
            if(count > iteration)
                triggers[count].GetComponent<R_TriggerScript>().State = ETriggerState.inactive;
            count++;
        }
        trigger1script.IsLocked = false;
        trigger2script.IsLocked = false;
        trigger2script.State = ETriggerState.pivot;
        boxesChecked = false;
		hasSwapInStep = false;

		if(box1script.Value > box2script.Value)
			hasSwapInStep = true;

        Debug.Log("hasSwapInStep: " + hasSwapInStep);

    }

    // Returns the index of the search box, or -1 if the box is not found.
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

    // TODO
    public void ResetStep()
    {
        if (isCompleted)
            return;

        currentState = new List<GameObject>(boxesOnIterationStart);

        // Move boxes back to correct positions
        for (int i = 0; i < currentState.Count; ++i)
        {
            currentState[i].GetComponent<R_BoxScript>().Move(triggers[i].transform.position);
        }

        index = iteration;

        InitializeStep(index);

        // Set correct trigger states (/colours)
        for (int i = 0; i < iteration; ++i)
        {
            triggers[i].GetComponent<R_TriggerScript>().State = ETriggerState.intermediate;
        }
        trigger2.GetComponent<R_TriggerScript>().State = ETriggerState.pivot;
    }
    
}
