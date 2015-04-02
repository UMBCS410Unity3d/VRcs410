// Haikun Huang

// Animation controller

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class HHK_Role_Controller : MonoBehaviour 
{
	static public string Run = "motion";
	static public string Walk = "motion";
	static public string Idle = "idle";
	static public string Left_Strafe = "left strafe";
	static public string Right_Strafe = "rithe strafe";
	static public string Left_Turn = "left turn";
	static public string Right_Turn = "right turn";
	static public string Stand_To_Sit = "stand to sit";
	static public string Sitting = "sitting";
	static public string Sit_To_Stand = "sit to stand";
	static public string Boring = "boring";
	static public string Boring1 = "boring 1";

	// sound
	public AudioClip footSound;

	Animator anim;
	NavMeshAgent agent;

	public AudioSource audioFoot; // must be assigned!

	float fadeTime = 0.05f;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
	}

	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		// update the speed to agent
		anim.SetFloat("speed",agent.velocity.magnitude);
	}

	// Play animation
	public void Play(string animName)
	{
		// if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
			// anim.CrossFade(animName,fadeTime);
			anim.Play(animName);
	}

	// Play animation
	public void CrossFade(string animName)
	{
		// if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
			anim.CrossFade(animName,fadeTime);

	}

	public bool Is_Current_State(string animName)
	{
		return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
	}

	public void Play_Foot_Sound()
	{
		audioFoot.PlayOneShot(footSound);
	}

	public void Play_Boring()
	{
		int action = Random.Range(0,3);
		switch (action)
		{
		case 0:
			anim.CrossFade(Boring1,fadeTime);
			break;
		default:
			anim.CrossFade(Boring,fadeTime);
			break;
		}
	}

}
