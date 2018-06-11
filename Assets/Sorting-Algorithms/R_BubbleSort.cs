using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class R_BubbleSort : R_ISortingAlgorithm
{

    private List<GameObject> triggers;
    private List<GameObject> boxes;
    private List<GameObject> boxesOnIterationStart;

    private int maxIndex; // FOR OPTIMIZED BUBBLE SORT - Keeps track of the max index to be compared.
    private bool isOptimized; // Decides whether to use the optimized version of bubble sort or not

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
    // current index to be compared (with i-1)
    private int index = 1;
    // Is set to false at the start of every iteration. Set to true when a swap is performed.
    // Algorithm is finished when the bool is still false at the end of an iteration.
    bool hasSwapInIteration = false;
    // Set to True each step when the boxes at position 'index' and 'index-1' are picked up at the same time
    bool boxesChecked = false;
    // Set to True when the algorithm is completed
    bool isCompleted = false;

    public R_BubbleSort(ref List<GameObject> triggers, ref List<GameObject> boxes, bool isOptimized = true)
    {
        this.triggers = triggers;
        this.boxes = boxes;
        boxesOnIterationStart = new List<GameObject>(this.boxes);
        this.isOptimized = isOptimized;
        maxIndex = triggers.Count;

        InitializeNewStep();
    }

    public void Update()
    {
        if (box1script.IsGrabbed && box2script.IsGrabbed)
        {
            boxesChecked = true;
        }

        if (boxesChecked && !box1script.IsGrabbed && !box2script.IsGrabbed && CheckCorrectPlacement())
        {
            EndStep();
            InitializeNewStep();
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
        if (isCompleted)
            return;

        // Move boxes back to correct positions
        for (int i = 0; i < boxesOnIterationStart.Count; ++i)
        {
            boxes[i] = boxesOnIterationStart[i];
            boxes[i].GetComponent<R_BoxScript>().Move(triggers[i].transform.position);
        }

        index = 1;

        InitializeNewStep();
    }

    private void InitializeNewStep()
    {
        // lock all triggers and reset trigger state to incorrect
        foreach (GameObject trigger in triggers)
        {
            R_TriggerScript t = trigger.GetComponent<R_TriggerScript>();
            t.IsLocked = true;
            if (index == 1 && t.State != ETriggerState.correct)
            {
                t.State = ETriggerState.incorrect;
            }
        }

        if(isCompleted)
        {
            Completed();

            return;
        }

        trigger1 = triggers[index - 1];
        trigger2 = triggers[index];
        trigger1script = trigger1.GetComponent<R_TriggerScript>();
        trigger2script = trigger2.GetComponent<R_TriggerScript>();
        box1 = boxes[index - 1];
        box2 = boxes[index];
        box1script = box1.GetComponent<R_BoxScript>();
        box2script = box2.GetComponent<R_BoxScript>();

        
        trigger1script.IsLocked = false;
        trigger2script.IsLocked = false;
        boxesChecked = false;
    }

    private bool CheckCorrectPlacement()
    {
        if (box1script.Value <= box2script.Value && trigger1script.ContainsBox(box1) && trigger2script.ContainsBox(box2))
            return true;
        if (box1script.Value > box2script.Value && trigger1script.ContainsBox(box2) && trigger2script.ContainsBox(box1))
        {
            // Reflect the swapped boxes in the boxes list
            boxes[index] = box1;
            boxes[index - 1] = box2;

            hasSwapInIteration = true; //

            return true;
        }

        return false;
    }

    private void EndStep()
    {
        if (isCompleted)
            return;

        trigger1script.State = ETriggerState.intermediate;

        ++index;
        if (index >= maxIndex) // Start new iteration
        {
            if (!hasSwapInIteration)
            {
                isCompleted = true;
            }

            index = 1;
            if (isOptimized)
            {
                trigger2script.State = ETriggerState.correct;
                --maxIndex;
                if(maxIndex == 1)
                {
                    isCompleted = true;
                }
            }

            hasSwapInIteration = false;
            boxesOnIterationStart = new List<GameObject>(boxes);
        }
    }

    private void Completed()
    {
        foreach (GameObject trigger in triggers)
        {
            R_TriggerScript t = trigger.GetComponent<R_TriggerScript>();
            t.IsLocked = true;
            t.State = ETriggerState.correct;
            t.SetHasTriggerColour(true);
        }

        foreach (GameObject box in boxes)
        {
            box.GetComponent<R_BoxScript>().Visibility = EVisibilityOptions.AlwaysVisible;
        }
    }

    public VideoClip instructionVideo()
    {
        return null;
    }

}
