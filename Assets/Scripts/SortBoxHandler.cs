using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortBoxHandler : MonoBehaviour {

    private BoxCollider trigger;
    private bool active;
    private GameObject objectInside;

	// Use this for initialization
	void Start () {
        trigger = GetComponent<BoxCollider>();
        active = false;
        objectInside = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //VRTK.VRTK_InteractableObject obj = other.gameObject.GetComponent<VRTK.VRTK_InteractableObject>();
        //if (obj && !active) obj.isGrabbable = false;
        if(!objectInside)
        {
            objectInside = other.gameObject;
            VRTK.VRTK_InteractableObject obj = other.gameObject.GetComponent<VRTK.VRTK_InteractableObject>();
            if (obj && !active) obj.isGrabbable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //VRTK.VRTK_InteractableObject obj = other.gameObject.GetComponent<VRTK.VRTK_InteractableObject>();
        //if (obj) obj.isGrabbable = true;
        objectInside = null;
    }

    public void SetActive(bool active)
    {
        this.active = active;
        if(objectInside)
        {
            VRTK.VRTK_InteractableObject obj = objectInside.GetComponent<VRTK.VRTK_InteractableObject>();
            if (obj) obj.isGrabbable = true;
            Debug.Log("Set Active");
        }
    }
}
