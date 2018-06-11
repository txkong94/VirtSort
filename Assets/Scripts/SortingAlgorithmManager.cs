using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface ISortingAlgorithm
{
    //Should get next sorting step state.
    bool Next();
    //Should get previous sorting step state.
    int[] Previous();
    //Should get current sorting step state.
    int[] Current();
    //Returns through if the algorithm is solved
    bool IsSolved();
    //Whatever array to actually sort.
    int[] ArrayToSort { get; set; }

    //Sort the whole thing.
    void Solve();

    //Instruction of current step
    string GetInstruction();

    int CurrentStep();
}

public class SortingAlgorithmManager : MonoBehaviour
{

    
    public int boxesToSort; //How many boxes to sort. Ideally not more than, like, 10 or something.
    public GameObject sortBox;
    public bool isNumbersVisible;
    public GameObject sortBoxTrigger;
    public GameObject boxSpawnPoint;
    public GameObject instructionWall;

    SortBoxTriggerScript triggerLeft = null;
    SortBoxTriggerScript triggerRight = null;
    SortBoxScript boxLeft = null;
    SortBoxScript boxRight = null;

    private ISortingAlgorithm sortingAlgorithm; //The algorithm to use for sorting. Probably should be public so it can be changed.
    
    private GameObject[] sortBoxes; //Holds the boxes that can be sorted.
    private GameObject[] sortBoxTriggers;

    int leftIndex = 0;
    int rightIndex = 1;

    bool bothPickedUp = false;

    private bool isShowingGeneralInstructions = false;
    bool solved = false;

    bool tableExist = false;
    private bool reloadCoroutineIsRunning = false;

    void Start()
    {
        reloadTable();
    }


    IEnumerator createTriggersAndBoxes() 
    {
        solved = false;
        GetComponent<ResizeTable>().updateTableSize(boxesToSort);
        sortBoxes = new GameObject[boxesToSort];
        sortBoxTriggers = new GameObject[boxesToSort];
        sortingAlgorithm = new BubbleSort();
        sortingAlgorithm.ArrayToSort = createRandomArray();
        int j = 0;
        foreach(int i in sortingAlgorithm.ArrayToSort) 
        {
            GameObject box = Instantiate(sortBox, boxSpawnPoint.transform);
            SortBoxScript sortBoxScript = box.GetComponent<SortBoxScript>();
            if (sortBoxScript)
            {
                sortBoxScript.SetNumbersVisible(isNumbersVisible);
                sortBoxScript.SetGrabbable(j == leftIndex || j == rightIndex);
            }

            GameObject boxTrigger = Instantiate(sortBoxTrigger, boxSpawnPoint.transform);
            boxTrigger.transform.localPosition += new Vector3(0, 0, j * boxTrigger.transform.localScale.z * 1.1f);
            //boxTrigger.GetComponent<SortBoxTriggerScript>().correctValue = i;
            //Debug.Log("Does this run on restart?");
            box.transform.localPosition += new Vector3(0, 0, j * boxTrigger.transform.localScale.z * 1.1f);

            boxTrigger.GetComponent<SortBoxHandler>().SetActive(true);
                        
            //TODO: This stuff should probably be placed on SortBoxScript instead.
            box.GetComponent<SortBoxScript>().value = i;
            foreach(TextMeshPro tmPro in box.GetComponentsInChildren<TextMeshPro>()) 
            {
                tmPro.text = i.ToString();
            }
            sortBoxes[j] = box;
            sortBoxTriggers[j] = boxTrigger;
            j++;
            yield return null;
        }

        triggerLeft = sortBoxTriggers[leftIndex].GetComponent<SortBoxTriggerScript>();
        triggerRight = sortBoxTriggers[rightIndex].GetComponent<SortBoxTriggerScript>();
        if (triggerLeft && triggerRight)
        {
            boxLeft = triggerLeft.GetObjectInTrigger().GetComponent<SortBoxScript>();
            boxRight = triggerRight.GetObjectInTrigger().GetComponent<SortBoxScript>();
        }

        //sortBoxTriggers[j - sortingAlgorithm.CurrentStep() - 1].GetComponent<SortBoxTriggerScript>().isFinal = true;
        updateTriggers();
        if(!isShowingGeneralInstructions) instructionWall.GetComponentInChildren<TextMeshPro>().text = sortingAlgorithm.GetInstruction();
        tableExist = true;
    }

    IEnumerator destroyTriggersAndBoxes()
    {
        //TODO: Delete all and reset stuff.
        if (tableExist)
        {
            tableExist = false;
            for (int i = 0; i < sortBoxes.Length; i++)
            {
                Destroy(sortBoxes[i]);
                Destroy(sortBoxTriggers[i]);
                yield return null;
            }
        }
    }



    void FixedUpdate()
    {
        if (tableExist && !solved && isCorrectStep())
        {
            //Debug.Log("FixedUpdate inside If");
            sortingAlgorithm.Next();
            solved = sortingAlgorithm.IsSolved();
            updateTriggers();
        }
    }

    private void updateTriggers() 
    {
        int j = 0;
        foreach(GameObject trigger in sortBoxTriggers)
        {
            // Debug.Log("Update trigger " + j);
            SortBoxTriggerScript sbts = trigger.GetComponent<SortBoxTriggerScript>();
            sbts.correctValue = sortingAlgorithm.ArrayToSort[j];

            if (solved) 
            {
                sortBoxTriggers[j].GetComponent<SortBoxTriggerScript>().isFinal = true;
                Debug.Log("Solved " + j);
            } 
            else if (j == sortBoxTriggers.Length - sortingAlgorithm.CurrentStep()) sortBoxTriggers[j].GetComponent<SortBoxTriggerScript>().isFinal = true;
            j++;

            if (sbts.isFinal) sbts.DoneInIteration = true;
            else sbts.DoneInIteration = false;
        }
        if(!isShowingGeneralInstructions) instructionWall.GetComponentInChildren<TextMeshPro>().text = sortingAlgorithm.GetInstruction();
    }

    private bool isCorrectStep()
    {
        for (int i = sortBoxTriggers.Length - 1; i >= 0; i--) {
            if (!sortBoxTriggers[i].GetComponent<SortBoxTriggerScript>().isCorrect) return false;
        }

        //Debug.Log("isCorrectStep() is true");
        return true;
    }

    private int[] createRandomArray()
    {
        int[] array = new int[boxesToSort];
        for (int i = 0; i < boxesToSort; i++)
        {
            array[i] = UnityEngine.Random.Range(1, 21);
        }

        return array;
    }

    public void reloadTable(int amount = -1)
    {
        StartCoroutine(reloadCoroutine(amount));
    }

    IEnumerator reloadCoroutine(int amount = -1) 
    {
        if(!reloadCoroutineIsRunning) 
        {
            reloadCoroutineIsRunning = true;
            yield return destroyTriggersAndBoxes();
            if(amount != -1) boxesToSort = amount;
            yield return createTriggersAndBoxes();
            reloadCoroutineIsRunning = false;
        }
    }

    void Update()
    {
        if(Input.GetButtonUp("Instruction")) 
		{
            showGeneralInstructions();
            Debug.Log("Pressing Button");
        }

        if(Input.GetButtonUp("Restart")) 
        {
            reloadTable(boxesToSort);
        }

        // TODO Handle with events instead of checking every frame
        if (triggerLeft && triggerRight && boxLeft && boxRight)
        {
            if(boxLeft && boxRight && boxLeft.IsGrabbed() && boxRight.IsGrabbed() && !triggerLeft.HasObject() && !triggerRight.HasObject())
            {
                bothPickedUp = true;
            }

            if(bothPickedUp && (triggerLeft.isCorrect || triggerRight.isFinal) && triggerLeft.HasObject() && triggerRight.HasObject())
            {
                bothPickedUp = false;
                triggerLeft.DoneInIteration = true;
                if (triggerRight.isFinal) triggerRight.DoneInIteration = true;
                boxLeft.SetGrabbable(false);
                boxRight.SetGrabbable(false);
                ++leftIndex; // TODO Create method in algorithm to get next index (this only works with bubble sort)
                ++rightIndex; // TODO Create method in algorithm to get next index (this only works with bubble sort)
                if (triggerRight.isFinal || rightIndex >= sortBoxTriggers.Length)
                {
                    leftIndex = 0; // TODO Create method in algorithm to get next index (this only works with bubble sort)
                    rightIndex = 1; // TODO Create method in algorithm to get next index (this only works with bubble sort)
                }

                triggerLeft = sortBoxTriggers[leftIndex].GetComponent<SortBoxTriggerScript>();
                triggerRight = sortBoxTriggers[rightIndex].GetComponent<SortBoxTriggerScript>();
                if(triggerLeft && triggerRight)
                {
                    boxLeft = triggerLeft.GetLastObjectInTrigger().GetComponent<SortBoxScript>();
                    boxRight = triggerRight.GetLastObjectInTrigger().GetComponent<SortBoxScript>();
                }
                else
                {
                    boxLeft = null;
                    boxRight = null;
                    Debug.LogError("A BOX WAS NOT FOUND!");
                }
                if(boxLeft && boxRight)
                {
                    boxLeft.SetGrabbable(true);
                    boxRight.SetGrabbable(true);
                }
            }
        }
    }

    private void showGeneralInstructions()
    {
        string s =  "Move your hands into a block and press and hold the trigger button to grab. \n";
               s += "Release the trigger button to release the block. \n ";
               s += "Swap the order of the blocks on the table according to the algorithm shown. \n";
               s += "Yellow indicates that the block is placed correctly for this step. \n";
               s += "Green indicates that the block is placed correctly for the end result.";
        if (isShowingGeneralInstructions)
        {
            instructionWall.GetComponentInChildren<TextMeshPro>().text = sortingAlgorithm.GetInstruction();
            isShowingGeneralInstructions = false;
        }
        else
        {
            instructionWall.GetComponentInChildren<TextMeshPro>().text = s;
            isShowingGeneralInstructions = true;
        }
    }
}