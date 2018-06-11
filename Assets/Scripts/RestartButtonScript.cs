using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;

public class RestartButtonScript : StandaloneButton {
    public delegate void RestartEventHandler();
    public static event RestartEventHandler RestartButtonEvent;

    // Use this for initialization
    void Start () {

    }

    public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);
        RestartButtonEvent();
    }

	protected override void Update() 
	{
        base.Update();
    }

}
