// Haikun Huang

/*
 * way point level 1 also ref to the big area, such as a lobby of a class room,
 * way point level 2 also ref to the small area, such as a chair or a desket, or a spot.
 * waiting point also ref to the waiting line of the big area.
 * 
 * There means we can know who in the big area and who in the small area.
*/

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class HHK_Way_Point_Level_2 : MonoBehaviour {

	// name of this place
	public string place_name;

	// list of the Charactors, who are staying in this area currently.
	HHK_Role_Tags charactor;

	// the charactor will use this place for a while
	public float mixTime, maxTime;
	public float force_to_leave_time_limit_scale = 2.0f;
	float force_to_leave_time_limit; // just for fix the bugs;

	// next place the AI should go
	public HHK_Way_Point_Level_1 next_place;

	public bool change_color_for_test = false;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (charactor)
		{
			force_to_leave_time_limit -= Time.deltaTime;
			if (force_to_leave_time_limit <= 0.0f)
			{
				// force left
				Leave_From_This_Place(charactor);
			}
		}
	}

	// check is the charactor in this place
	public bool Is_In_This_Place(HHK_Role_Tags role)
	{
		return (charactor == role);
	}
	
	
	// when a charactor have a purpose to go to this place, 
	// it will be added to the list of Charactors
	bool Join_To_This_Place(HHK_Role_Tags role)
	{
		if (charactor)
			return false;

		// change color for test
		if (change_color_for_test)
			role.GetComponent<Renderer>().material.color = Color.yellow;

		// set the time limit;
		force_to_leave_time_limit = maxTime * force_to_leave_time_limit_scale;

		charactor = role;
		return true;
	}

	public bool Join_From_The_Waiting_Line(HHK_Role_Tags role)
	{
		return Join_To_This_Place(role);
	}
	
	// when a charactor leave, it will be removed from the list of Charactors
	bool Leave_From_This_Place(HHK_Role_Tags role)
	{
		if (charactor == role)
		{
			// change color for test
			if (change_color_for_test)
				role.GetComponent<Renderer>().material.color = Color.blue;

			charactor = null;
			return true;
		}
		return false;
	}

	// Is ready for use?
	public bool Is_Ready()
	{
		if (charactor)
			return false;
		return true;
	}

	void OnTriggerEnter(Collider other)
	{
		HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
		if (rt)
		{
			// join to this place
			Join_To_This_Place(rt);
		}
	}

	void OnTriggerStay(Collider other)
	{
		// just for player 
		HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
		if (rt)
		{
			if (rt.Is_Belong_Teams(new HHK_Role_Tags.TAG[]{HHK_Role_Tags.TAG.Role_Player}))
			{
				// set the time limit;
				force_to_leave_time_limit = maxTime * force_to_leave_time_limit_scale;

				// join to this place
				Join_To_This_Place(rt);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
		if (rt)
		{
			// leave from this place
			Leave_From_This_Place(rt);
		}
	}
}
