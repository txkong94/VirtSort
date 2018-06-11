using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;

public class GenericButtonScript : VRTK_InteractableObject {


    public string ButtonText 
	{
        get
		{
            return buttonText;
        }

		set 
		{
            GetComponentInChildren<TextMeshPro>().text = value;
            buttonText = value;
        }
    }

	private string buttonText;

    public delegate void DoSomething();
    public event DoSomething doSomething;

    // Use this for initialization
    void Start () {

    }


    public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);
        if(doSomething != null)
			doSomething();
    }

	public override void StopUsing(VRTK_InteractUse usingObject)
	{
		base.StopUsing(usingObject);
        if(doSomething != null)
			doSomething();
    }
	
	protected override void Update() 
	{
        base.Update();
    }
}
