using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BoxResizer : MonoBehaviour {

    public GameObject leftController;
    public GameObject rightController;

    private VRTK_InteractGrab leftInteract;
    private VRTK_InteractGrab rightInteract;
    private bool hasVRTK_InteractGrab = false;

	// Use this for initialization
	void Start () {
        leftInteract = leftController.GetComponent<VRTK_InteractGrab>();
        rightInteract = rightController.GetComponent<VRTK_InteractGrab>();

        // TODO (Maybe?) Have separate method for each event (left and right), 
        // and store the local scale at pickup, scale accordingly (on comparison and on drop), 
        // instead of hardcoding numbers.
        leftInteract.ControllerStartUngrabInteractableObject += new ObjectInteractEventHandler(ActOnUngrab);
        rightInteract.ControllerStartUngrabInteractableObject += new ObjectInteractEventHandler(ActOnUngrab);
    }
	
	// Update is called once per frame
	void Update () {
        // TODO If you pick up two boxes, then drop one, the other should be scaled to normal size

		if(leftInteract && rightInteract)
        {
            GameObject leftObject = leftInteract.GetGrabbedObject();
            GameObject rightObject = rightInteract.GetGrabbedObject();
            if (!(leftObject && rightObject)) { return; }

            SortBoxScript leftBox = leftObject.GetComponent<SortBoxScript>();
            SortBoxScript rightBox = rightObject.GetComponent<SortBoxScript>();
            if (!(leftBox && rightBox)) { return; }

            if (leftBox.value < rightBox.value)
            {
                leftObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                //rightObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (leftBox.value > rightBox.value)
            {
                //leftObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                rightObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
	}

    private void ActOnUngrab(object sender, ObjectInteractEventArgs e)
    {
        VRTK_InteractGrab interactObject = (VRTK_InteractGrab)sender;
        if(!interactObject) { return; }

        interactObject.GetGrabbedObject().transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
    }
}
