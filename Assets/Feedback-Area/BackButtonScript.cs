using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;
using UnityEngine.SceneManagement;

public class BackButtonScript : StandaloneButton {
    public string scene;
    public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);
        SceneManager.LoadScene(scene);
    }

	protected override void Update() 
	{
        base.Update();
    }

}
