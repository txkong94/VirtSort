using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButtons : MonoBehaviour {

    public RadioButtonScript[] Buttons;
    public uint DefaultOnIndex = 0;

    // Use this for initialization
    void Start () {
        if (DefaultOnIndex >= Buttons.Length)
        {
            Debug.LogError("DefualtOnIndex too big.");
            return;
        }

        if (Buttons.Length == 0)
            return;

        // TODO Set selected based on option in R_GameManager
        SetSelected(Buttons[DefaultOnIndex]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSelected(RadioButtonScript button)
    {
        foreach (RadioButtonScript btn in Buttons)
        {
            if (btn == button)
                btn.SetSelected();
            else
                btn.SetIdle();
        }
    }
}
