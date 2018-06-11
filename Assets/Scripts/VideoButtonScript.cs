using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRTK;

public class VideoButtonScript : StandaloneButton {

    public delegate void VideoEventHandler();
    public static event VideoEventHandler VideoButtonEvent;


    // Use this for initialization
    void Start () {

	}
	
    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        VideoButtonEvent();
    }
}
