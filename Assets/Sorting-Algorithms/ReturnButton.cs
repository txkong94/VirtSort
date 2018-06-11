using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ReturnButton : StandaloneButton {

    public delegate void ReturnEventHandler();
    public static event ReturnEventHandler ReturnButtonEvent;

    public override void StartUsing(VRTK_InteractUse usingObject) {
        base.StartUsing(usingObject);
        ReturnButtonEvent();
    }
}
