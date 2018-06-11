using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_GameManager : MonoBehaviour {

    public GameObject Manager;
    
    public ESortingAlgorithm SortingAlgorithm;
    public uint NumberOfBoxes = 10;
    public uint MaxValueOnBoxes = 20;
    public EVisibilityOptions NumberVisibility = EVisibilityOptions.NeverVisible;
    public bool ShowTriggerColours;
    public bool ShowBoxHighlighting;
    
    public ToggleButton TriggerColourButton;
    public ToggleButton BoxHighlightButton;
    public GameObject LeftController;
    public GameObject RightController;

    private GameObject CurrentManager;

    void Load()
	{

    }
    // Use this for initialization
    void Start () {
        InitNewManager();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitNewManager()
    {
        //if(CurrentManager != null) return;

        

        //TODO: These currently override saved states.
/*         SortStateVariables.SortingAlgorithm = SortStateVariables.SortingAlgorithm == ESortingAlgorithm.None ? SortingAlgorithm : SortStateVariables.SortingAlgorithm;
        SortStateVariables.NumberOfBoxes = (int)NumberOfBoxes;
        SortStateVariables.BoxValueMax = (int)MaxValueOnBoxes;
        SortStateVariables.NumberVisibility = NumberVisibility;
        SortStateVariables.ShowTriggerColours = ShowTriggerColours;
        SortStateVariables.EnableHighlighting = ShowBoxHighlighting;
 */
        TriggerColourButton.State = (SortStateVariables.ShowTriggerColours ? EToggle.On : EToggle.Off);
        TriggerColourButton.SetButtonsState(SortStateVariables.ShowTriggerColours ? EToggle.On : EToggle.Off);
        BoxHighlightButton.State = (SortStateVariables.EnableHighlighting ? EToggle.On : EToggle.Off);
        BoxHighlightButton.SetButtonsState(SortStateVariables.EnableHighlighting ? EToggle.On : EToggle.Off);
        //TODO: Set NumberVisibilityPanel state
        //TODO: Set NumberOfBoxes Panel state


        GameObject manager = Instantiate(Manager);

        R_BoxResizer boxResizerScript = manager.GetComponent<R_BoxResizer>();
        boxResizerScript.leftController = LeftController;
        boxResizerScript.rightController = RightController;

        R_Manager managerScript = manager.GetComponent<R_Manager>(); 
        managerScript.GameManager = this;
        
        //managerScript.Restart();
        //CurrentManager = manager;
    }
}
