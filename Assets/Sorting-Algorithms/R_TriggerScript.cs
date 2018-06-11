using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETriggerState
{
    pivot,
    inactive,
    incorrect,
    intermediate,
    correct
}

public class R_TriggerScript : MonoBehaviour {
    // Materials for the different states of the trigger

    public Material blueMaterial;
    public Material inactiveMaterial;
    public Material incorrectMaterial;
    public Material intermediateMaterial;
    public Material correctMaterial;

    // -Uses HashSet instead of List to avoid duplicated-
    private HashSet<GameObject> boxesInTrigger = new HashSet<GameObject>();

    private bool isShowingTriggerColour = true;
    private Material currentMaterial;
    public bool SetHasTriggerColour(bool set)
    {
        isShowingTriggerColour = set;

        if(isShowingTriggerColour)
            GetComponent<Renderer>().material = currentMaterial;
        else
            GetComponent<Renderer>().material = inactiveMaterial;

        return isShowingTriggerColour;
    }

    // The state of the trigger is reflected in its colour - incorrect/red, correct/yellow or final/green
    private ETriggerState state;
    public ETriggerState State
    {
        get { return state; }
        set
        {
            state = value;
            // TODO Set appropriate material.
            switch((int)state)
            {

                case (int)ETriggerState.inactive:
                    currentMaterial = inactiveMaterial;
                    break;
                case (int)ETriggerState.pivot:
                    currentMaterial = blueMaterial;
                    break;
                case (int)ETriggerState.incorrect:
                    currentMaterial = incorrectMaterial;
                    break;
                case (int)ETriggerState.intermediate:
                    currentMaterial = intermediateMaterial;
                    break;
                case (int)ETriggerState.correct:
                    currentMaterial = correctMaterial;
                    break;
                default:
                    currentMaterial = incorrectMaterial;
                    break;
            }
            if(isShowingTriggerColour)
                GetComponent<Renderer>().material = currentMaterial;
        }
    }

    private bool isLocked = false;
    public bool IsLocked
    {
        set {
            isLocked = value;
            foreach (GameObject box in boxesInTrigger)
            {
                if (isLocked)
                {
                    box.GetComponent<R_BoxScript>().IsGrabbable = false;
                    box.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    box.GetComponent<R_BoxScript>().IsGrabbable = true;
                    box.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
        get { return isLocked; }
    }

    public bool IsEmpty()
    {
        if (boxesInTrigger.Count > 0)
            return false;
        else
            return true;
    }

    public bool ContainsBox(GameObject box)
    {
        if(boxesInTrigger.Contains(box))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SortBox"))
        {
            boxesInTrigger.Add(other.gameObject);
            IsLocked = isLocked;
            State = state;
            R_BoxScript box = other.GetComponent<R_BoxScript>();
            box.RegisterSpawnPosition(gameObject.transform.position);
            if (!box.IsGrabbed)
                box.Snap();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SortBox"))
        {
            boxesInTrigger.Remove(other.gameObject);
            other.GetComponent<R_BoxScript>().DeregisterSpawnPosition(gameObject.transform.position);
        }

        if (IsEmpty() && State != ETriggerState.correct)
            currentMaterial = incorrectMaterial;
            if(isShowingTriggerColour)
                GetComponent<Renderer>().material = currentMaterial;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
