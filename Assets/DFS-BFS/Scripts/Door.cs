using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRTK;

public class Door : MonoBehaviour {

    public GameObject TeleportDestinationLocation { get; set; }

    private TextMeshPro sign;
    public string DoorText
    {
        get
        {
            if (!sign)
                return "";

            return sign.text;
        }
        set
        {
            if(sign)
            {
                sign.SetText(value);
            }
        }
    }
    

    private void Awake()
    {
        sign = GetComponentInChildren<TextMeshPro>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
