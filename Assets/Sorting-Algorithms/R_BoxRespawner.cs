using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_BoxRespawner : MonoBehaviour {

    public GameObject SpawnPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.gameObject;
        if(obj.CompareTag("SortBox"))
        {
            R_BoxScript script = obj.GetComponent<R_BoxScript>();
            R_Manager manager = FindObjectOfType<R_Manager>();

            // Find correct trigger to spawn to. If not found, spawn at default location (SpawnPoint).
            int i = manager.Algorithm.GetBoxIndex(script.gameObject);
            Vector3 spawn = SpawnPoint.transform.position;
            if (i >= 0 && manager.Algorithm.GetTriggerFromIndex(i).GetComponent<R_TriggerScript>().IsEmpty())
                spawn = manager.Algorithm.GetTriggerFromIndex(i).transform.position;

            script.Move(spawn);

            // Remove velocity and  to hinder box from falling off table again immediately
            Rigidbody boxrb = script.gameObject.GetComponent<Rigidbody>();
            if (boxrb)
            {
                boxrb.velocity = Vector3.zero;
                boxrb.rotation = Quaternion.identity;
            }
        }
    }
}
