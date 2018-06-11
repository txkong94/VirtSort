using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;

public class ResetStepButtonScript : StandaloneButton
{
    public delegate void ResetEventHandler();
    public static event ResetEventHandler ResetButtonEvent;

    // Use this for initialization
    void Start()
    {
        
    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        ResetButtonEvent();
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        /*
        base.StopUsing(usingObject);
        manager.GetComponent<R_Manager>().ResetStep();
        */
    }

    protected override void Update()
    {
        base.Update();
    }

}
