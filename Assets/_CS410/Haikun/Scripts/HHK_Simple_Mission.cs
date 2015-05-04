using UnityEngine;
using System.Collections;

public class HHK_Simple_Mission : MonoBehaviour {

	public string emailAddress{get;set;}
	bool mission1 = false;

	public int mission1_process{get;set;}

	// singleton
	static  HHK_Simple_Mission singleton = null;
	
	static public HHK_Simple_Mission Get()
	{
		return singleton;
	}

	void Awake()
	{
		singleton = this;
		mission1_process = 0;
		emailAddress = "";
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		// listen the mission1
		if (!mission1)
		{
			while(MessageSystem.Listen(MessageSystem.Mission1))
			{
				mission1 = true;
				emailAddress = MessageSystem.Next(MessageSystem.Mission1) as string;
				//Debug.Log(emailAddress);
			}
		}

		if (mission1)
		{
			while(MessageSystem.Listen(MessageSystem.NPC_Die))
			{
				MessageSystem.Next(MessageSystem.NPC_Die);
				mission1_process++;
			}
		}
	}
}
