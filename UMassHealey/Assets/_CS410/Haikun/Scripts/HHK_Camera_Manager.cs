// Haikun Huang
using UnityEngine;
using System.Collections;

public class HHK_Camera_Manager : MonoBehaviour 
{

	public GameObject normal_cam;

	public GameObject vr_cam;

	public static bool b_normal_cam = true;


	// Use this for initialization
	void Start () 
	{
		if (b_normal_cam)
		{
			normal_cam.SetActive(true);
			vr_cam.SetActive(false);
		}
		else
		{
			normal_cam.SetActive(false);
			vr_cam.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

}
