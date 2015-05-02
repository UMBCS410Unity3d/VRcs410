// Haikun Huang
using UnityEngine;
using System.Collections;

public class HHK_Main_Menu : MonoBehaviour 
{
	public string next_level = "S-03";
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	// ***************** button funtcion ***************** //
	public void Normal_Cam()
	{
		HHK_Camera_Manager.b_normal_cam = true;
		Application.LoadLevel(next_level);
	}

	public void VR_Cam()
	{
		HHK_Camera_Manager.b_normal_cam = false;
		Application.LoadLevel(next_level);
	}
}
