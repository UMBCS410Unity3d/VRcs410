using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// *** this class just for associate VR mode to click the button ***

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Collider))]
public class HHK_UI_Bt_For_Finger : MonoBehaviour {
	
	Button bt;

	// Use this for initialization
	void Start () 
	{
		bt = gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other) 
	{
		// leap motion
		RigidFinger finger = other.GetComponentInParent<RigidFinger>();
		if (finger)
		{
			// call the button onclick event
			bt.onClick.Invoke();
		}
	}
}
