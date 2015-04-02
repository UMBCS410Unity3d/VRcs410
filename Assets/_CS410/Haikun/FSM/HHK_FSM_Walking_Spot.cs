using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Walking_Spot : HHK_FSM_Action
{
	public FsmEvent inSpot; 
	public FsmEvent lookingSpot;
	public HHK_FSM_Walking_NPC_Variables.MOVETYPE moveType;

	float timeout;
	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// *** move to walking spot
		// set the destination
		npc.agent.SetDestination (npc.spot.transform.position);
		switch (moveType) {
		case HHK_FSM_Walking_NPC_Variables.MOVETYPE.Run:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.Run);
			npc.agent.speed = npc.speed_run;
			break;
		case HHK_FSM_Walking_NPC_Variables.MOVETYPE.Walk:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.Walk);
			npc.agent.speed = npc.speed_walk;
			break;
		default:
			// play animation.
			npc.role_controller.CrossFade (HHK_Role_Controller.Idle);
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

		// if not ready and I am not in this spot, switch to the in spot state
		if (!npc.spot.Is_Ready())
		{
			if (!npc.spot.Is_In_This_Place(Owner.GetComponent<HHK_Role_Tags> ()))
			{
				Fsm.Event(lookingSpot);
				return;
			}
		}
		
		// if at the right position, switch to the in spot state
		if ((npc.agent.destination - npc.transform.position).magnitude <= npc.destance_for_stop_limit)
		{
			Fsm.Event(inSpot);
			return;
			
		}

	}

	// Code that runs when exiting the state.
	public override void OnExit ()
	{
		
	}


}
