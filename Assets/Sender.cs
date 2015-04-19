using UnityEngine;
using System.Collections;

public class Sender : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		MessageSystem.Send("int", 1);
		MessageSystem.Send("int", 2);
		MessageSystem.Send("int", 3);

		MessageSystem.Send("float", 1.1);

		MessageSystem.Send("missing handler", "No!");

		foreach (string key in MessageSystem.All_Event())
		{
			Debug.Log("Event: " + key);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// listen
		while(MessageSystem.Listen("int"))
		{
			Debug.Log("deal with int: " + MessageSystem.Next("int"));
		}

		while(MessageSystem.Listen("float"))
		{
			Debug.Log("deal with float: " + MessageSystem.Next("float"));
		}
	}
}
