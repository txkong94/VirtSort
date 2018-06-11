using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHighlightButtonScript : ToggleButtonScript {

	public static event ToggleButtonEventHandler BoxHighlightButtonEvent;

	// Use this for initialization
	public override void Broadcast()
    {
        if (BoxHighlightButtonEvent == null)
            return;
        if (ToggleState == EToggle.On)
            BoxHighlightButtonEvent(true);
        else
            BoxHighlightButtonEvent(false);
    }
}
