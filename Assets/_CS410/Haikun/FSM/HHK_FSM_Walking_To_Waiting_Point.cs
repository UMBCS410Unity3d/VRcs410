using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Walking_To_Waiting_Point : HHK_FSM_Action
{
	public FsmEvent inWaitingLine;

	public HHK_FSM_Walking_NPC_Variables.MOVETYPE moveType;

	float timeout;
	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// get the index of the waiting line
		npc.index_of_waiting_line = npc.waiting_point.Get_My_Index (Owner.GetComponent<HHK_Role_Tags> ());
		// set the destination
		npc.agent.SetDestination (npc.waiting_point.Get_My_Position (Owner.GetComponent<HHK_Role_Tags> ()));
		switch (moveType) {
		case HHK_FSM_Walking_NPC_Variables.MOVETYPE.Run:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.AnimName.motion);
			npc.agent.speed = npc.speed_run;
			break;
		case HHK_FSM_Walking_NPC_Variables.MOVETYPE.Walk:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.AnimName.motion);
			npc.agent.speed = npc.speed_walk;
			break;
		default:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.AnimName.idle);
			npc.agent.speed = 0;
			break;
		}

		timeout = 0.5f;
	}

	// Code that runs every frame.
	public override void OnUpdate ()
	{
		timeout -= Time.deltaTime;
		if (timeout >= 0.0f) {
			return;
		}
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// if at the right position, switch to the in waiting line
		if ((npc.agent.destination - npc.transform.position).magnitude <= npc.destance_for_stop_limit)
		{
			Fsm.Event(inWaitingLine);
			return;
		}


	}

	// Code that runs when exiting the state.
	public override void OnExit ()
	{

	}


}
