using UnityEngine;
using System.Collections;

public class HHK_PM_TEST : MonoBehaviour {

	public PlayMakerFSM fsm;

	// Use this for initialization
	void Start () 
	{
		if (fsm == null)
		{
			fsm = GetComponent<PlayMakerFSM>();
		}
		fsm.SendEvent("GG");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
