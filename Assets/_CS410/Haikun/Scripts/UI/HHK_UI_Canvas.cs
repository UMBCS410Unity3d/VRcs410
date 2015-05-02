using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class HHK_UI_Canvas : MonoBehaviour {

	public HHK_UI_Panel_Base uiPanel;
	
	Collider coll;
	bool backOnLine = true;
	float current = 0.0f;

	public float interval = 2.0f;



	// Use this for initialization
	void Awake () 
	{
		coll = gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (uiPanel.gameObject.activeSelf)
		{
			coll.enabled = false;
		}
		else
		{
			if (!backOnLine)
			{
				current = interval;
				backOnLine = true;
			}
		}

		if (backOnLine)
		{
			current -= Time.deltaTime;
			if (current <= 0.0f)
			{
				coll.enabled = true;
			}
		}

		// force to leave
		if (uiPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Tab))
		{
			uiPanel.BT_Cancel();
		}


	}

	// active the UI
	void OnTriggerEnter(Collider other) 
	{
	
		// leap motion
		RigidFinger finger = other.GetComponentInParent<RigidFinger>();
		if (finger)
		{
			uiPanel.gameObject.SetActive(true);
			backOnLine = false;
		}
	}
}
