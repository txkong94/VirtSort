using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRTK;

public class FeedbackButtonScript : StandaloneButton {
    public string feedbackValue;

    // Use this for initialization
    void Start () {
		GetComponentInChildren<TextMeshPro>().text = feedbackValue.ToString();
	}
	
	// Update is called once per frame
    public delegate void FeedbackEventHandler(string value);
    public static event FeedbackEventHandler FeedbackButtonEvent;


    public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);
        FeedbackButtonEvent(feedbackValue);
    }

	protected override void Update() 
	{
        base.Update();
    }
}
