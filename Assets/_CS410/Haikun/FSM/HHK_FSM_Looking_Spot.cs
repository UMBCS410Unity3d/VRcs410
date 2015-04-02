using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Looking_Spot : HHK_FSM_Action
{
	public FsmEvent walkingToWaitingPoint;
	public FsmEvent walkingToSpot;
	public FsmEvent lookingPlace;

	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();
		npc.spot = npc.place.Any_Available_Spot ();
		
		// find a available spot
		if (npc.spot) {
			// join the spot first
			npc.spot.Join_From_The_Waiting_Line(Owner.GetComponent<HHK_Role_Tags> ());
			Fsm.Event (walkingToSpot);

		}
		// go to the waiting point
		// if there is no any waiting point, go back to the looking for wp 1 state
		else {
			if (npc.place.waittingPoint) {
				// get the waiting point;
				npc.waiting_point = npc.place.waittingPoint;
				// join the line, and get the index of the line,
				// otherwise, looking for the next wp 1
				if (!npc.waiting_point.Join_To_This_Place (Owner.GetComponent<HHK_Role_Tags> ())) {
					Fsm.Event (lookingPlace);
					return;
				}
				Fsm.Event (walkingToWaitingPoint);
				return;

			}
			// switch to walking
			else {
				Fsm.Event (lookingPlace);
				return;
			}
		}
	}

//	// Code that runs every frame.
//	public override void OnUpdate()
//	{
//		
//	}
//
//	// Code that runs when exiting the state.
//	public override void OnExit()
//	{
//		
//	}


}
