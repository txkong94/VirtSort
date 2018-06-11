using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerColoursButtonScript : ToggleButtonScript {
    public static event ToggleButtonEventHandler TriggerColourButtonEvent;

    public override void Broadcast()
    {
        if (TriggerColourButtonEvent == null)
            return;
        if (ToggleState == EToggle.On)
            TriggerColourButtonEvent(true);
        else
            TriggerColourButtonEvent(false);
    }
}
