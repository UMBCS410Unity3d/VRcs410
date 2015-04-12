using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Waiting_Boring : FsmStateAction
{
	public FsmEvent walkingToWaitingPoint;
	float timeout = 1.0f;

	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// play animatino
		npc.role_controller.Play_Boring ();

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
		if (npc.role_controller.Is_Current_State (HHK_Role_Controller.AnimName.idle)) {
			Fsm.Event (walkingToWaitingPoint);
			return;
		}
		
	}

	// Code that runs when exiting the state.
	public override void OnExit ()
	{
		
	}


}
