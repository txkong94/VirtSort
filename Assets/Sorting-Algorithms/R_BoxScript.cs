using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRTK;
using System;

public class R_BoxScript : MonoBehaviour {

    private VRTK_InteractableObject interact;
    private Vector3 scale;

    public bool IsSnapping = false;

    private bool isHighlightEnabled = false;

    public GameObject NormalMesh;
    public GameObject HighlightMesh;

    private EVisibilityOptions visibility;
    public EVisibilityOptions Visibility
    {
        get { return visibility; }

        set
        {
            visibility = value;
            switch((int)visibility)
            {
                case (int)EVisibilityOptions.AlwaysVisible:
                    SetNumberVisibility(true);
                    break;
                case (int)EVisibilityOptions.VisibleOnPickup:
                    SetNumberVisibility(false);
                    break;
                case (int)EVisibilityOptions.NeverVisible:
                    SetNumberVisibility(false);
                    break;
                default:
                    SetNumberVisibility(true);
                    break;
            }
        }
    }

    private Quaternion baseRotation;
    private Vector3 snapPosition;
    public Vector3 SnapPosition
    {
        get { return snapPosition; }
        set { snapPosition = value; }
    }

    private void SetNumberVisibility(bool visible)
    {
        string valueText = visible ? Value.ToString() : "";

        foreach (TextMeshPro tmPro in GetComponentsInChildren<TextMeshPro>())
        {
            tmPro.text = valueText;
        }
    }

    private int value;
    public int Value
    {
        set
        {
            this.value = value;
            if (visibility == EVisibilityOptions.NeverVisible || (visibility == EVisibilityOptions.VisibleOnPickup && !IsGrabbed))
                return;
            foreach(TextMeshPro tmPro in GetComponentsInChildren<TextMeshPro>())
            {
                tmPro.text = value.ToString();
            }
        }

        get { return value; }
    }
    
    private bool isGrabbable = true;
    public bool IsGrabbable
    {
        set
        {
            isGrabbable = value;
            SetHighlight();
            interact.isGrabbable = isGrabbable;
        }

        get { return isGrabbable; }
    }



    public bool IsGrabbed
    {
        get
        {
            return interact.IsGrabbed();
        }
    }

    public bool IsHighlightEnabled
    {
        get
        {
            return isHighlightEnabled;
        }

        set
        {
            isHighlightEnabled = value;
            SetHighlight();
        }
    }

    private List<Vector3> spawnPositionList;
    public void RegisterSpawnPosition(Vector3 pos)
    {
        if (!spawnPositionList.Contains(pos))
            spawnPositionList.Add(pos);
    }
    public void DeregisterSpawnPosition(Vector3 pos)
    {
        spawnPositionList.Remove(pos);
    }

    void Awake()
    {
        interact = GetComponent<VRTK_InteractableObject>();
        spawnPositionList = new List<Vector3>();
    }
	// Use this for initialization
	void Start () {
        SnapPosition = Vector3.zero;
        baseRotation = transform.localRotation;
        
        scale = gameObject.transform.localScale;
        interact.InteractableObjectGrabbed += OnGrabbed;
        interact.InteractableObjectUngrabbed += OnUngrabbed;

    }
	
	// Update is called once per frame
	void Update () {
            
	}

    void OnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if(Visibility == EVisibilityOptions.VisibleOnPickup)
        {
            SetNumberVisibility(true);
        }
    }

    void OnUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (Visibility == EVisibilityOptions.VisibleOnPickup)
        {
            SetNumberVisibility(false);
        }

        Snap();
    }

    public void Snap()
    {
        if (!IsSnapping || spawnPositionList.Count == 0) { return; }

        snapPosition = spawnPositionList[0] + new Vector3(.0f, gameObject.transform.localScale.y / 2.0f, .0f);
        transform.position = snapPosition;
        transform.localRotation = baseRotation;
    }

    public void Move(Vector3 pos)
    {
        transform.position = pos + new Vector3(.0f, gameObject.transform.localScale.y / 2.0f, .0f);
        transform.localRotation = baseRotation;
    }

    private void SetHighlight()
    {
        if (IsHighlightEnabled && isGrabbable)
        {
            HighlightMesh.SetActive(true);
            NormalMesh.SetActive(false);
        }
        else
        {
            HighlightMesh.SetActive(false);
            NormalMesh.SetActive(true);
        }
    }
}
