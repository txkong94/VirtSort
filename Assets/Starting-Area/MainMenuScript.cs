using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//TODO: Separate to file?
public static class SortStateVariables 
{
    public static ESortingAlgorithm SortingAlgorithm = ESortingAlgorithm.None;
    public static int NumberOfBoxes = 10;
    public static int BoxValueMax = 20;
    public static bool EnableHighlighting = true;
    public static bool ShowTriggerColours = true;
    public static EVisibilityOptions NumberVisibility = EVisibilityOptions.NeverVisible;
}

public class MainMenuScript : MonoBehaviour {

    private GameObject panel;
    private List<GameObject> buttons;
    public GameObject button;

    // Use this for initialization
    void Start () {
        panel = getChildByName("Panel");
        buttons = new List<GameObject>();
        setupButton("BubbleSort", BubbleSortButton);
        setupButton("Optimized BubbleSort", OptimizedBubbleSortButton);
        setupButton("InsertionSort", InsertionSortButton);
        setupButton("QuickSort", QuickSortButton);
        setupButton("DFS", DFSButton);
    }

	void setupButton(string text, GenericButtonScript.DoSomething methodToRun)
	{
		GameObject newButton = Instantiate(button);
        newButton.SetActive(true);
        newButton.transform.localPosition += new Vector3(-1f*buttons.Count, 0, 0); // TODO: Make table and button pos/size dynamic
        GenericButtonScript buttonScript = newButton.GetComponent<GenericButtonScript>();
        buttonScript.ButtonText = text;
        buttonScript.doSomething += methodToRun;
        buttons.Add(newButton);
    }

	void InsertionSortButton()
	{
        SortStateVariables.SortingAlgorithm = ESortingAlgorithm.InsertionSort;
        SceneManager.LoadScene("R_Main");
	}

	void BubbleSortButton()
	{
        SortStateVariables.SortingAlgorithm = ESortingAlgorithm.BubbleSort;
        SceneManager.LoadScene("R_Main");
	}
	void OptimizedBubbleSortButton()
	{
        SortStateVariables.SortingAlgorithm = ESortingAlgorithm.BubbleSortOptimized;
        SceneManager.LoadScene("R_Main");
	}

	void QuickSortButton()
	{
        SortStateVariables.SortingAlgorithm = ESortingAlgorithm.QuickSort;
        SceneManager.LoadScene("R_Main");
	}

	void DFSButton()
	{	
		SceneManager.LoadScene("DFS");
	}

	GameObject getChildByName(string name)
	{
		foreach(Transform child in transform)
		{
			if(child.name == name)
                return child.gameObject;
        }
        return null;
    }

    void Update()
    {
        if (Input.GetButtonUp("BubbleSortOptimized"))
            OptimizedBubbleSortButton();
    }
}
