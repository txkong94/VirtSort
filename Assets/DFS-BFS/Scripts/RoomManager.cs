using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRTK;

public class RoomManager : MonoBehaviour {

    public int Value = 0; // TODO Make private
    public GameObject DoorObject;

    [Range(0, 5)]
    public int DoorsOnNorthWall = 1;
    private GameObject[] NorthDoors;

    [Range(0, 5)]
    public int DoorsOnSouthhWall = 1;
    private GameObject[] SouthDoors;

    [Range(0, 5)]
    public int DoorsOnWestWall = 1;
    private GameObject[] WestDoors;

    [Range(0, 5)]
    public int DoorsOnEastWall = 1;
    private GameObject[] EastDoors;

    public bool HasParentRoom = true;
    public Transform TeleportDestinationLocation;

    private GameObject PlayerPosition;
    private Transform ParentDoorTransform;

    private GameObject ParentDoor;
    private GameObject[] ChildDoors;

    private float CHILD_DOOR_SPACE = 0.1f;

    // Use this for initialization
    void Start ()
    {
        // Init door arrays
        ChildDoors = new GameObject[DoorsOnNorthWall];
        // Set teleport location of DoorObject
        DoorObject.GetComponentInChildren<VRTK_DestinationPoint>().destinationLocation = TeleportDestinationLocation;

        InitObjectPositions();
        SpawnObjects();
    }

    private void InitObjectPositions()
    {
        // Find and set PlayerLocation from an object (if present)
        ParentDoorTransform = GameObject.Find("SouthWall").transform.Find("Origin");
        ParentDoorTransform.SetGlobalScale(Vector3.one);
    }

    private void SpawnObjects()
    {
        float DoorWidth = DoorObject.transform.lossyScale.z;

        // TODO Spawn Player (?)

        // Spawn parent door
        if(HasParentRoom)
        {
            ParentDoor = Instantiate(DoorObject, ParentDoorTransform);
            Door door = ParentDoor.GetComponent<Door>();
            if (door)
                door.DoorText = "PARENT";
            // TODO? Change light colour?
        }

        // TODO If NumberOfDoors > 0: Spawn child doors
    }

    // Update is called once per frame
    void Update () {
		
	}
}
