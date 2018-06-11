using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberVisibilityButtonScript : RadioButtonScript {

    public EVisibilityOptions VisibilityOption = EVisibilityOptions.NeverVisible;

    public delegate void NumberVisibilityEventHandler(EVisibilityOptions option);
    public static event NumberVisibilityEventHandler NumberVisibilityButtonEvent;

    public override void Broadcast()
    {
        NumberVisibilityButtonEvent(VisibilityOption);
    }
}
