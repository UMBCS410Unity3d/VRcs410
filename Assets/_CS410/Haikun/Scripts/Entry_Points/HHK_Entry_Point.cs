// Haikun Huang

// this componet is use to shift the levles.
// this componet should be attached to the Entry Points which must attached a Colloder.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class HHK_Entry_Point : MonoBehaviour 
{

	public string to_next_level;

	public int which_start_point;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other)
	{
		// only accept the player get into this trigger
		// and he is ready to go.
//		HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
//		if (rt)
//		{
//			HHK_Role_Tags.TAG[] tags = {HHK_Role_Tags.TAG.Role_Player};
//			if (rt.Is_Belong_Teams(tags) 
//			    && HHK_Entry_Points_Manager.ready_to_go)
//			{
//				// player get into this trigger
//				// shift to the next level and start at the given start point/entry point
//				HHK_Entry_Points_Manager.which_start_point = which_start_point;
//				Application.LoadLevel(to_next_level);
//			}
//
//		}
		// leap motion
		RigidFinger finger = other.GetComponentInParent<RigidFinger>();
		if (finger)
		{
			HHK_Entry_Points_Manager.which_start_point = which_start_point;
			Application.LoadLevel(to_next_level);
		}
	}

	void OnTriggerExit(Collider other)
	{
		// only accept the player get into this trigger
		HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
		if (rt)
		{
			HHK_Role_Tags.TAG[] tags = {HHK_Role_Tags.TAG.Role_Player};
			if (rt.Is_Belong_Teams(tags))
			{
				// player get into this trigger
				// ready to shift to next level
				HHK_Entry_Points_Manager.ready_to_go = true;
			}
			
		}
	}
}
