// haikun huang
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Animator))]
public class HHK_Door : MonoBehaviour 
{

	Animator anim;

	public HHK_Role_Tags.TAG[] allowed;

	List<HHK_Role_Tags> roles; // how many role still in this area?

	public bool opened = false;

	public string openName = "open";
	public string closeName = "close";


	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
		roles = new List<HHK_Role_Tags>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// close door
		if (roles.Count <=0 && opened)
		{
			anim.CrossFade(closeName, 0.1f);
			opened = false;
		}

		// open door
		if (roles.Count > 0 && !opened)
		{
			anim.CrossFade(openName, 0.1f);
			opened = true;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		HHK_Role_Tags role = other.GetComponent<HHK_Role_Tags>();
		if (role)
		{
			if (role.Is_Belong_Teams_Either(allowed))
			{
				// in to this area
				roles.Add(role);
			}
		}
	}

	void OnTriggerExit(Collider other) 
	{
		HHK_Role_Tags role = other.GetComponent<HHK_Role_Tags>();
		if (role)
		{
			if (role.Is_Belong_Teams_Either(allowed))
			{
				// get out this area
				roles.Remove(role);
			}
			
		}
	}

}
