using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The on/off buttons of a ToggleButton prefab should contain a script that inherits from this class.
// The Broadcast() method should broadcast a custom ToggleButtonEventHandler event with appropriate value(on/true OR off/false)
public abstract class ToggleButtonScript : MonoBehaviour {

    public delegate void ToggleButtonEventHandler(bool isOn);
    public static event ToggleButtonEventHandler DefaultToggleButtonEvent;

    [HideInInspector]
    public EToggle ToggleState = EToggle.On;

    public virtual void Broadcast()
    {
        DefaultToggleButtonEvent((ToggleState == EToggle.On) ? true : false);
    }
}
