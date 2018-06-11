using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTable : MonoBehaviour {

    private SortingAlgorithmManager sortingAlgorithmManager;
    private int numOfBoxes = 1;

    // The mesh for the top plate should be the size to fit one box.
    public GameObject top;
    // The right leg mesh is positioned correctly after resizing the top
    public GameObject rightLeg;

    private Vector3 oldScale;
    private Vector3 oldRightLegPos;
    private Vector3 oldTopPos;

    // Use this for initialization
    void Awake()
    {
        oldScale = top.transform.localScale;
        oldTopPos = top.transform.localPosition;
        oldRightLegPos = rightLeg.transform.localPosition;
    }

    public void updateTableSize(int size) 
    {
        sortingAlgorithmManager = GetComponent<SortingAlgorithmManager>();
        numOfBoxes = size;
        top.transform.localPosition = oldTopPos;
        rightLeg.transform.localPosition = oldRightLegPos;
        // Resize top plate according to number of boxes to be placed.

        Vector3 newScale = new Vector3(oldScale.x, oldScale.y, oldScale.z * numOfBoxes);
        top.transform.localScale = newScale;
        top.transform.Translate(0, 0, (newScale.z - oldScale.z) / 2);

        // Move the right leg according to the top plate sizing
        rightLeg.transform.Translate(0, 0, newScale.z - oldScale.z);
    }
}
