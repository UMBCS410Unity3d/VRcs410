using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_After_Spot : HHK_FSM_Action
{
	public FsmEvent lookingPlace;
	public FsmEvent walkingToPlace;

	float timeout = .5f;
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// play animation via spot
		npc.role_controller.CrossFade(npc.spot.actionNameExit);
		timeout = .5f;
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		timeout -= Time.deltaTime;
		if (timeout >= 0.0f) {
			return;
		}

		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		if (npc.role_controller.Is_Current_State(HHK_Role_Controller.Idle))
		{
			// if has a next place need to go
			if (npc.spot.next_place)
			{
				npc.place = npc.spot.next_place;
				Fsm.Event(walkingToPlace);
				return;
			}
			else
			{
				Fsm.Event(lookingPlace);
				return;
			}
		}
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
//		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();
//		// leave this place
//		if (npc.spot)
//			npc.spot.Leave_From_This_Place(Owner.GetComponent<HHK_Role_Tags> ());
	}


}
