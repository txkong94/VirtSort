using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;

public class ChangeAmountScript : RadioButtonScript {

    public delegate void AmountEventHandler(int amount);
    public static event AmountEventHandler AmountButtonEvent;

    //TODO: Make this shit work.
    public int boxAmount;

    // Use this for initialization
    void Start () {
        GetComponentInChildren<TextMeshPro>().text = boxAmount.ToString();
    }
    /*
    public override void StartUsing(VRTK_InteractUse usingObject)
	{
        base.StartUsing(usingObject);
        AmountButtonEvent(boxAmount);
    }
    */
	protected override void Update() 
	{
        base.Update();
    }

    public override void Broadcast()
    {
        AmountButtonEvent(boxAmount);
    }

}
