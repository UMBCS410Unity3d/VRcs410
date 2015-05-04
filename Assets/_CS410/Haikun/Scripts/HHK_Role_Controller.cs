// Haikun Huang

// Animation controller

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class HHK_Role_Controller : MonoBehaviour 
{
//	static public string Run = "motion";
//	static public string Walk = "motion";
//	static public string Idle = "idle";
//	static public string Left_Strafe = "left_strafe";
//	static public string Right_Strafe = "rithe_strafe";
//	static public string Left_Turn = "left_turn";
//	static public string Right_Turn = "right_turn";
//	static public string Stand_To_Sit = "stand_to_sit";
//	static public string Sitting = "sitting";
//	static public string Sit_To_Stand = "sit_to_stand";
//	static public string Boring = "boring";
//	static public string Boring1 = "boring_1";
//	static public string Stand_To_Sit_Type = "stand_to_sit_type_sit";
//	static public string Sit_Type_To_Stand = "sit_type_sit_to_stand";

	// enum -> string
	public enum AnimName
	{
		motion,
		idle,
		left_strafe,
		rithe_strafe,
		left_turn,
		right_turn,
		stand_to_sit,
		sit_to_stand,
		boring,
		boring_1,
		stand_to_sit_type,
		sit_type_to_stand,
		collect_paper_from_print,
		die
	}

	// sound
	public AudioClip footSound;
	public AudioClip dieSound;

	Animator anim;
	NavMeshAgent agent;

	AudioSource audio; // must be assigned!
	

	float fadeTime = 0.05f;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		audio = GetComponent<AudioSource>();
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

	public void Play(AnimName animName)
	{
		// if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
		// anim.CrossFade(animName,fadeTime);
		anim.Play(animName.ToString());
	}

	// Play animation
	public void CrossFade(string animName)
	{
		// if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
			anim.CrossFade(animName,fadeTime);

	}

	public void CrossFade(AnimName animName)
	{
		// if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
		anim.CrossFade(animName.ToString(),fadeTime);
		
	}

	public bool Is_Current_State(string animName)
	{
		return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
	}

	public bool Is_Current_State(AnimName animName)
	{
		return anim.GetCurrentAnimatorStateInfo(0).IsName(animName.ToString());
	}

	public void Play_Foot_Sound()
	{
		audio.PlayOneShot(footSound);
	}

	public void Play_Boring()
	{
		int action = Random.Range(0,3);
		switch (action)
		{
		case 0:
			anim.CrossFade(AnimName.boring_1.ToString(),fadeTime);
			break;
		default:
			anim.CrossFade(AnimName.boring.ToString(),fadeTime);
			break;
		}
	}

	public void Play_Die_Sound()
	{
		audio.outputAudioMixerGroup = null;
		audio.PlayOneShot(dieSound);
	}

}
