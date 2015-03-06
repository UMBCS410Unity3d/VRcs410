// Haikun Huang
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * way point level 1 also ref to the big area, such as a lobby of a class room,
 * way point level 2 also ref to the small area, such as a chair or a desket, or a spot.
 * waiting point also ref to the waiting line of the big area.
 * 
 * There means we can know who in the big area and who in the small area.
*/


public class HHK_Waiting_Point : MonoBehaviour 
{
	// name of this place
	public string place_name;

	// list of the Charactors, who are staying in this area currently.
	List<HHK_Role_Tags> charactors;

	// maximun in the line
	public int max_len_of_line = 5;

	// interval of each charactors
	public float interval = 2.0f;

	public bool change_color_for_test = false;

	void Awake()
	{
		charactors = new List<HHK_Role_Tags>();
	}
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// check is the charactor in this place
	public bool Is_In_This_Place(HHK_Role_Tags role)
	{
		return charactors.Contains(role);
	}

	// when a charactor have a purpose to go to this place, 
	// it will be added to the list of Charactors
	public bool Join_To_This_Place(HHK_Role_Tags role)
	{
		if (charactors.Count < max_len_of_line)
		{
			charactors.Add(role);
			// change color for test
			if (change_color_for_test)
				role.GetComponent<Renderer>().material.color = Color.red;
			return true;
		}
		return false;
	}
	
	// when a charactor leave, it will be removed from the list of Charactors
	public void Leave_From_This_Place(HHK_Role_Tags role)
	{
		charactors.Remove(role);
		// change color for test
		if (change_color_for_test)
			role.GetComponent<Renderer>().material.color = Color.blue;
	}

	// get who in this place
	public HHK_Role_Tags[] Get_Who_In_This_Place()
	{
		return charactors.ToArray();
	}

	// get the position of the currently, for the input charactors
	public Vector3 Get_My_Position(HHK_Role_Tags role)
	{
		Vector3 ret = gameObject.transform.position;

		// get the index of the list for the input charactor
		int i = Get_My_Index(role);
		ret += (-gameObject.transform.forward) * interval * i;

		return ret;
	}

	// get the index of the line
	public int Get_My_Index(HHK_Role_Tags role)
	{
		return charactors.IndexOf(role);
	}
}
