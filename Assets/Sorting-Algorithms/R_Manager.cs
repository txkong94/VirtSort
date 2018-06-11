using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public interface R_ISortingAlgorithm
{
    void Update();

    // Get the index/position of a box according to the current state. Should return -1 if box is not found.
    int GetBoxIndex(GameObject searchBox);

    // Return the GameObject of the trigger at the given index/position
    GameObject GetTriggerFromIndex(int i);

    // Resets the algorithm to the beginning of the current step (iteration), and places the boxes accordingly.
    void ResetStep();

}

public enum ESortingAlgorithm
{
    BubbleSort,
    BubbleSortOptimized,
    InsertionSort,
    InsertionSortRecursive,
    QuickSort,
    None
}

public enum EVisibilityOptions
{
    AlwaysVisible,
    VisibleOnPickup,
    NeverVisible
}

public class R_Manager : MonoBehaviour {
    [HideInInspector]
    public R_GameManager GameManager;

    private int NumberOfBoxes;
    private int BoxValueMax;
    //public bool VisibleNumbers;
    private GameObject TablePrefab;

    // Instructional videos. I dno how to do this through the scripts idk.
    public VideoClip InsertionSortVideo;
    public VideoClip BubbleSortVideo;
    public VideoClip QuickSortVideo;

    private GameObject SortBox;
    private GameObject SortBoxTrigger;

    private ESortingAlgorithm SortingAlgorithm;
    private R_ISortingAlgorithm algorithm;
    private EVisibilityOptions NumberVisibility = EVisibilityOptions.NeverVisible;
    public bool EnableHighlighting = false;
    public bool BoxSnapping;
    private bool ShowTriggerColours = true;

    public R_ISortingAlgorithm Algorithm { get { return algorithm; } }
    private List<GameObject> boxes = new List<GameObject>();
    private List<GameObject> triggers = new List<GameObject>();
    public GameObject Table 
    {
        get;
        private set;
    }

    private void Awake()
    {
        // Set up event handling
        RegisterEvents();

        EnableHighlighting = SortStateVariables.EnableHighlighting;
        ShowTriggerColours = SortStateVariables.ShowTriggerColours;
        NumberOfBoxes = SortStateVariables.NumberOfBoxes;
        BoxValueMax = SortStateVariables.BoxValueMax;
        EnableHighlighting = SortStateVariables.EnableHighlighting;
        SortingAlgorithm = SortStateVariables.SortingAlgorithm;

    }

    private void LoadState()
    {
        EnableHighlighting = SortStateVariables.EnableHighlighting;
        ShowTriggerColours = SortStateVariables.ShowTriggerColours;
        NumberOfBoxes = SortStateVariables.NumberOfBoxes;
        BoxValueMax = SortStateVariables.BoxValueMax;
        EnableHighlighting = SortStateVariables.EnableHighlighting;
        SortingAlgorithm = SortStateVariables.SortingAlgorithm;
        NumberVisibility = SortStateVariables.NumberVisibility;
    }

    private void SaveState()
    {
        SortStateVariables.EnableHighlighting = EnableHighlighting;
        SortStateVariables.ShowTriggerColours = ShowTriggerColours;
        //SortStateVariables.NumberOfBoxes = NumberOfBoxes;
        SortStateVariables.BoxValueMax = BoxValueMax;
        SortStateVariables.EnableHighlighting = EnableHighlighting;
        SortStateVariables.SortingAlgorithm = SortingAlgorithm;
        SortStateVariables.NumberVisibility = NumberVisibility;
    }

    
    private void RegisterEvents()
    {
        NumberVisibilityButtonScript.NumberVisibilityButtonEvent += HandleToggleVisibility;
        RestartButtonScript.RestartButtonEvent += HandleRestart;
        ResetStepButtonScript.ResetButtonEvent += HandleReset;
        ChangeAmountScript.AmountButtonEvent += HandleChangeAmount;
        TriggerColoursButtonScript.TriggerColourButtonEvent += HandleTriggerColour;
        BoxHighlightButtonScript.BoxHighlightButtonEvent += HandleHighlighting;
        ReturnButton.ReturnButtonEvent += HandleReturn;
    }


    private void DeregisterEvents()
    {
        NumberVisibilityButtonScript.NumberVisibilityButtonEvent -= HandleToggleVisibility;
        RestartButtonScript.RestartButtonEvent -= HandleRestart;
        ResetStepButtonScript.ResetButtonEvent -= HandleReset;
        ChangeAmountScript.AmountButtonEvent -= HandleChangeAmount;
        TriggerColoursButtonScript.TriggerColourButtonEvent -= HandleTriggerColour;
        BoxHighlightButtonScript.BoxHighlightButtonEvent -= HandleHighlighting;
        ReturnButton.ReturnButtonEvent -= HandleReturn;
    }
    void OnDestroy()
    {
        //Deregister events, otherwise causes MissingReferenceError due to calling non-existent subscriber.
        DeregisterEvents();
        SaveState();
    }
    // Use this for initialization
    void Start () {
        TablePrefab = Resources.Load("Table", typeof(GameObject)) as GameObject;
        SortBox = Resources.Load("R_SortBox", typeof(GameObject)) as GameObject;
        SortBoxTrigger = Resources.Load("R_SortBoxTrigger", typeof(GameObject)) as GameObject;
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        algorithm.Update();

        // WORKING??
        if (Input.GetButtonUp("Restart"))
            SceneManager.LoadScene("Start");
            //Initialize(NumberOfBoxes);

    }

    public void Initialize(int? numberOfBoxes = null)
    {
        numberOfBoxes = numberOfBoxes == null ? NumberOfBoxes : numberOfBoxes;
        Initialization((int) numberOfBoxes);
    }

    public void Initialize(List<int> listToSort)
    {
        Initialization(listToSort.Count, listToSort);
    }

    private void Initialization(int numberOfBoxes, List<int> listToSort = null)
    {
        NumberOfBoxes = numberOfBoxes;

        if(Table == null)
            Table = Instantiate(TablePrefab);
        // Resize table to appropriate size
        Table.GetComponent<ResizeTable>().updateTableSize(NumberOfBoxes);

        // Initialize boxes and triggers
        InitializeBoxesAndTriggers(NumberOfBoxes, listToSort);


        // Initialize the chosen algorithm
        switch ((int)SortingAlgorithm)
        {
            case (int)ESortingAlgorithm.BubbleSort:
                algorithm = new R_BubbleSort(ref triggers, ref boxes, false);
                break;
            case (int)ESortingAlgorithm.BubbleSortOptimized:
                algorithm = new R_BubbleSort(ref triggers, ref boxes);
                break;
            case (int)ESortingAlgorithm.InsertionSort:
                algorithm = new R_InsertionSort(ref triggers, boxes, false);
                break; // temp
            case (int)ESortingAlgorithm.InsertionSortRecursive:
                algorithm = new R_InsertionSort(ref triggers, boxes, true);
                break;
            case (int)ESortingAlgorithm.QuickSort:
                algorithm = new R_QuickSort(ref triggers, ref boxes, isRoot: true);
                break;
            default:
                algorithm = new R_BubbleSort(ref triggers, ref boxes);
                break;
        }
        SetHighlighting(EnableHighlighting);
    }

    public void Restart(int numOfBoxes = -1)
    {
        if(numOfBoxes > 0)
        {
            NumberOfBoxes = numOfBoxes;
        }

        Initialize(NumberOfBoxes);
        
    }

    public void ResetStep()
    {
        algorithm.ResetStep();
    }

    public void SetNumberVisibility(EVisibilityOptions opt)
    {
        
        foreach (GameObject box in boxes)
        {
            
            box.GetComponent<R_BoxScript>().Visibility = opt;
        }
        NumberVisibility = opt; 
    }

    public void SetHasTriggerColours(bool set)
    {
        
        foreach (GameObject trigger in triggers)
        {
                trigger.GetComponent<R_TriggerScript>().SetHasTriggerColour(set);
        }
        ShowTriggerColours = set;
    }

    private void InitializeBoxesAndTriggers(int numOfBoxes, List<int> listToSort = null)
    {
        foreach (GameObject box in boxes)
            Destroy(box);

        foreach (GameObject trigger in triggers)
            Destroy(trigger);

        // Clear values in lists in case they are already filled (e.g. when resetting)
        boxes.Clear();
        triggers.Clear();

        // Initialize boxes and triggers
        for (int i = 0; i < numOfBoxes; ++i)
        {
            // Create box with a random value
            GameObject box = Instantiate(SortBox, Table.transform.Find("BoxSpawn"));
            
            R_BoxScript boxScript = box.GetComponent<R_BoxScript>();
            if(listToSort == null)
                boxScript.Value = UnityEngine.Random.Range(1, BoxValueMax+1);
            else
                boxScript.Value = listToSort[i];
            boxScript.Visibility = NumberVisibility;
            boxScript.IsSnapping = BoxSnapping;
            

            // Create trigger
            GameObject trigger = Instantiate(SortBoxTrigger, Table.transform.Find("BoxSpawn"));
            trigger.GetComponent<R_TriggerScript>().SetHasTriggerColour(ShowTriggerColours);

            // Move trigger and the associated box to the correct position on the table
            // They are moved from the base position (BoxSpawn GameObject) based on the triggers' size
            trigger.transform.localPosition += new Vector3(0, 0, i * trigger.transform.localScale.z * 1.1f);
            box.transform.localPosition += new Vector3(0, 0, i * trigger.transform.localScale.z * 1.1f);
            // Add box and trigger to respective lists
            boxes.Add(box);
            triggers.Add(trigger);
            //SetHighlighting(EnableHighlighting);
        }
    }

    //
    // Handle button events
    //

    void HandleToggleVisibility(EVisibilityOptions option)
    {
        SetNumberVisibility(option);
    }

    void HandleRestart()
    {
        Restart();
    }

    void HandleReset()
    {
        ResetStep();
    }

    void HandleChangeAmount(int amount)
    {
        Restart(amount);
    }

    void HandleTriggerColour(bool hasColour)
    {
        SetHasTriggerColours(hasColour);
    }

    void HandleHighlighting(bool enabled)
    {
        SetHighlighting(enabled);
        EnableHighlighting = enabled;
    }

    public void SetHighlighting(bool enabled)
    {
        
        foreach(GameObject box in boxes)
        {
            box.GetComponent<R_BoxScript>().IsHighlightEnabled = enabled;
        }
    }

    void HandleReturn()
    {
        SceneManager.LoadScene("Start");
    }
}
