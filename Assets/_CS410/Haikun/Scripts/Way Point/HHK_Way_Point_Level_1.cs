﻿// haikun huang

/*
 * way point level 1 also ref to the big area, such as a lobby of a class room,
 * way point level 2 also ref to the small area, such as a chair or a desket, or a spot.
 * waiting point also ref to the waiting line of the big area.
 * 
 * There means we can know who in the big area and who in the small area.
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class HHK_Way_Point_Level_1 : MonoBehaviour 
{
	// name of this place
	public string place_name;

	// list of the level 2 way point
	public HHK_Way_Point_Level_2[] wayPoints;

	// list of the waitting point
	public HHK_Waiting_Point waittingPoint;

	// secrete this location 
	public bool secreted = false;

	// list of the Charactors, who are staying in this area currently.
	List<HHK_Role_Tags> charactors;

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

	// find all the location
	public static HHK_Way_Point_Level_1[] FindAllPlace(bool include_secreted = false)
	{
		// find all the wp1
		HHK_Way_Point_Level_1[] wp1s = GameObject.FindObjectsOfType<HHK_Way_Point_Level_1>();

		// if included secreted, return all.
		if (include_secreted)
			return wp1s;

		// else just return un-secreted places.
		List<HHK_Way_Point_Level_1> list = new List<HHK_Way_Point_Level_1>();
		for(int i=0; i<wp1s.Length; i++)
		{
			if (!wp1s[i].secreted)
				list.Add(wp1s[i]);
		}
		return list.ToArray();
	}


	// check is the charactor in this place
	public bool Is_In_This_Place(HHK_Role_Tags role)
	{
		return charactors.Contains(role);
	}


	// when a charactor have a purpose to go to this place, 
	// it will be added to the list of Charactors
	void Join_To_This_Place(HHK_Role_Tags role)
	{
		charactors.Add(role);
	}
	
	// when a charactor leave, it will be removed from the list of Charactors
	void Leave_From_This_Place(HHK_Role_Tags role)
	{
		charactors.Remove(role);
	}

	// if and level 2 way point available, return it
	public HHK_Way_Point_Level_2 Any_Available_Spot()
	{
		List<HHK_Way_Point_Level_2> ret = new List<HHK_Way_Point_Level_2>();
		foreach(HHK_Way_Point_Level_2 spot in wayPoints)
		{
			if (spot.Is_Ready())
			{
				ret.Add(spot);
			}
		}

		// no any available spot
		if (ret.Count == 0)
			return null;

		// randomly chooes a spot
		return ret[Random.Range(0,ret.Count)];
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
