using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortBoxTriggerScript : MonoBehaviour {

    public int correctValue;
    public Material correctMaterial;
	public Material intermediateMaterial;
    public Material incorrectMaterial;

    bool set = false; // TODO Remove?
    bool doneInIteration = false;
    private GameObject objectInTrigger = null;
    private GameObject lastObjectInTrigger = null;

    public bool isCorrect = false;
    public bool isFinal = false; // Set to true when this is the final correct value of the sort.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SortBox"))
        {
            objectInTrigger = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("SortBox")) 
		{
			if(other.GetComponent<SortBoxScript>().value == correctValue) 
			{
                if(isFinal && doneInIteration) GetComponent<Renderer>().material = correctMaterial;
				else if(doneInIteration) GetComponent<Renderer>().material = intermediateMaterial;
                else GetComponent<Renderer>().material = incorrectMaterial;
                isCorrect = true;
            }
			else
			{
				GetComponent<Renderer>().material = incorrectMaterial;
				isCorrect = false;
			}

            // Should just be in OnTriggerEnter, but it may not be called when the app starts (boxes start inside trigger?)
            if (!set)
            {
                objectInTrigger = other.gameObject;
                set = true;
            }
            lastObjectInTrigger = other.gameObject;
        }

        
    }

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("SortBox")) 
		{
			GetComponent<Renderer>().material = incorrectMaterial;
            isCorrect = false;

            objectInTrigger = null;
        }
	}

    public GameObject GetObjectInTrigger()
    {
        if(objectInTrigger) { return objectInTrigger; }
        return null;
    }
    public GameObject GetLastObjectInTrigger()
    {
        if (lastObjectInTrigger) { return lastObjectInTrigger; }

        return null;
    }

    public bool HasObject()
    {
        if(objectInTrigger != null) { return true; }

        return false;
    }

    public bool DoneInIteration
    {
        set { doneInIteration = value; }
    }
}
