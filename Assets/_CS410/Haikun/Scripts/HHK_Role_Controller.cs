// Haikun Huang

// Animation controller

using UnityEngine;
using System.Collections;


public class HHK_Role_Controller : MonoBehaviour 
{
	static public string Run = "run";
	static public string Walk = "walk";
	static public string Idle = "idle";
	static public string Left_Strafe = "left strafe";
	static public string Right_Strafe = "rithe strafe";
	static public string Left_Turn = "left turn";
	static public string Right_Turn = "right turn";


	Animator anim;

	float fadeTime = 0.1f;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	// Play animation
	public void Play_Animation(string animName)
	{
		anim.CrossFade(animName,fadeTime);
	}


}
