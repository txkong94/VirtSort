using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

public class SortBoxScript : MonoBehaviour, IComparable 
{
    public int value;

    public GameObject currentIterationTrigger;

    private VRTK_InteractableObject interactableObject;

    private bool isNumbersVisible = true;

    private bool grabbed = false;

    void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
    }

    void Start()
    {
        if(!isNumbersVisible)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        
    }

    public void SetNumbersVisible(bool visible)
    {
        isNumbersVisible = visible;
    }

    public void SetGrabbable(bool active)
    {
        if (interactableObject)
        {
            interactableObject.isGrabbable = active;
        }
    }

    public bool IsGrabbable()
    {
        if(interactableObject)
        {
            return interactableObject.isGrabbable;
        }

        return false;
    }

    public bool IsGrabbed()
    {
        if(interactableObject && interactableObject.GetGrabbingObject())
        {
            return true;
        }
        return false;
    }

    public int CompareTo(object obj)
    {
        return value.CompareTo(obj);
    }

    IEnumerator moveToTrigger() 
    {
        //TODO: Should move this sortBox back to corresponding sortBoxTrigger of current iteration.
        yield return null;
    }
}
