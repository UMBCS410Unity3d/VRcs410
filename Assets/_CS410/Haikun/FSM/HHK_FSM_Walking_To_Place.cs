using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Walking_To_Place : HHK_FSM_Action
{
	public FsmEvent lookingPlace;
	public FsmEvent lookingSpot;
	public HHK_FSM_Walking_NPC_Variables.MOVETYPE moveType;

	float timeout;

	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		if (!npc.place) {
			Fsm.Event (lookingPlace);
		} else {

			// set the destination
			npc.agent.SetDestination (npc.place.transform.position);
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
		
		// check, if reach the place, switch to looking for wp 2 state
		// and stop
		if (npc.place.Is_In_This_Place (Owner.GetComponent<HHK_Role_Tags> ())) {
			// send event looking spot
			Fsm.Event (lookingSpot);
		}

	}

	// Code that runs when exiting the state.
	public override void OnExit ()
	{
		
	}


}
