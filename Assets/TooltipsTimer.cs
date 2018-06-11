using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipsTimer : MonoBehaviour {

	public float Timer = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Timer -= Time.deltaTime;
		if(Timer < 0.0f)
		{
			transform.GetChild(0).gameObject.SetActive(false);
			this.enabled = false;

		}
	}
}
