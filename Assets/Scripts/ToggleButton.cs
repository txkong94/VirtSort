using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum EToggle
{
    On,
    Off
}

public class ToggleButton : VRTK_InteractableObject {
    [HideInInspector]
    public EToggle State = EToggle.On;

    public ToggleButtonScript OnButton;
    public ToggleButtonScript OffButton;

    protected override void Awake()
    {
        if (!OnButton || !OffButton)
            Debug.LogError("Children On and Off must contain a script inheriting from ToggleButtonScript!");
        OnButton.ToggleState = EToggle.On;
        OffButton.ToggleState = EToggle.Off;
    }

    private void Start()
    {
        SetButtonsState(State);
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
    {
        base.StartUsing(currentUsingObject);

        State = (State == EToggle.On) ? EToggle.Off : EToggle.On;

        SetButtonsState(State);
    }

    public void SetButtonsState(EToggle state)
    {
        if (state == EToggle.On)
        {
            OnButton.gameObject.SetActive(true);
            OffButton.gameObject.SetActive(false);
            OnButton.Broadcast();
        }
        else
        {
            OnButton.gameObject.SetActive(false);
            OffButton.gameObject.SetActive(true);
            OffButton.Broadcast();
        }
    }
}
