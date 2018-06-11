using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public abstract class RadioButtonScript : VRTK_InteractableObject {

    public GameObject Selected;
    public GameObject Idle;

    public delegate void RadioButtonEventHandler();
    public static event RadioButtonEventHandler DefaultRadioButtonEvent;

    protected override void Awake()
    {
        //base.Awake();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
    {
        base.StartUsing(currentUsingObject);
        Broadcast();
        GetComponentInParent<RadioButtons>().SetSelected(this);
    }

    public virtual void Broadcast()
    {
        DefaultRadioButtonEvent();
    }

    public void SetSelected()
    {
        Selected.SetActive(true);
        Idle.SetActive(false);
    }

    public void SetIdle()
    {
        Selected.SetActive(false);
        Idle.SetActive(true);
    }
}
