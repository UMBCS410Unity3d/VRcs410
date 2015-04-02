using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HHK_Role_Tags))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HHK_Role_Controller))]
public class HHK_FSM_Walking_NPC_Variables : MonoBehaviour 
{
	// move type
	public enum MOVETYPE
	{
		Walk,
		Run
	}

	// action type
	public enum ACTIONTYPE
	{

	}


	// some speed
	public float speed_walk = 1.8f;
	public float speed_run = 3.5f;
	
	// some place
	public HHK_Way_Point_Level_1 place {get;set;}
	public HHK_Way_Point_Level_2 spot {get;set;}
	public HHK_Waiting_Point waiting_point {get;set;}
	
	public NavMeshAgent agent {get;set;}
	public HHK_Role_Tags role {get;set;}
	
	public float destance_for_stop_limit {get;set;}
	
	// public float time_to_leave  {get;set;}
	
	// waiting index of the line
	public int index_of_waiting_line {get;set;}

	//	waiting boring
	public float waittingBoringTime = 20.0f;
	
	// role controller
	public HHK_Role_Controller role_controller{get;set;}

	void Awake()
	{
		destance_for_stop_limit = 0.01f;


		agent = GetComponent<NavMeshAgent>();
		role = GetComponent<HHK_Role_Tags>();
		role_controller = GetComponent<HHK_Role_Controller>();
	}
	// Use this for initialization
	void Start () 
	{
		
		// idle at beginning
		role_controller.Play(HHK_Role_Controller.Idle);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
