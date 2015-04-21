using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_In_Waiting_Line : HHK_FSM_Action
{
	public FsmEvent walkingToWaitingPoint;
	public FsmEvent walkingToSpot;
	public FsmEvent boring;
	public float waitingTime;

	Vector3 newDir;

	float timeout;

	// Code that runs on entering the state.
	public override void OnEnter ()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		waitingTime = Random.Range (0, npc.waittingBoringTime);

		// ramdomly direction
		newDir = Random.insideUnitSphere;
		newDir.y = 0;
		newDir.Normalize();

		// play animation
		npc.role_controller.CrossFade (HHK_Role_Controller.AnimName.idle);

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

		// rotataion
		//npc.transform.forward = Vector3.Lerp(npc.transform.forward, newDir, 
		//                                     npc.agent.angularSpeed * Time.deltaTime);
		// npc.transform.rotation = Quaternion.Lerp (npc.transform.rotation, npc.waiting_point.transform.rotation, 
		//                                     npc.agent.angularSpeed * Time.deltaTime);

		waitingTime -= Time.deltaTime;
		if (waitingTime <= 0.0f) {
			Fsm.Event (boring);
		}

		// keep checking any availabe spot
		if (npc.index_of_waiting_line == 0) {
			npc.spot = npc.place.Any_Available_Spot ();
			if (npc.spot) {
				// leave this please
				npc.waiting_point.Leave_From_This_Place (Owner.GetComponent<HHK_Role_Tags> ());
				// join the spot first
				npc.spot.Join_From_The_Waiting_Line (Owner.GetComponent<HHK_Role_Tags> ());
				// to spot
				Fsm.Event (walkingToSpot);
				return;
			}
		}

		// keep checking the new waiting index of the line, move to the new position
		if (npc.index_of_waiting_line != npc.waiting_point.Get_My_Index (Owner.GetComponent<HHK_Role_Tags> ())) {

			// waiting ,until the prev moved
			HHK_Role_Tags prevTag 
				= npc.waiting_point.At_Index(npc.waiting_point.Get_My_Index (Owner.GetComponent<HHK_Role_Tags> ()) - 1);
							
			// I am the first one
			if (prevTag == null) {
				// go to the waitting line
				Fsm.Event (walkingToWaitingPoint);
				return;
			}

			HHK_FSM_Walking_NPC_Variables prev = prevTag.GetComponent<HHK_FSM_Walking_NPC_Variables> ();
			// prev moved
			if (prev.index_of_waiting_line == prev.waiting_point.Get_My_Index (prev.GetComponent<HHK_Role_Tags> ())) {
				// go to the waitting line
				Fsm.Event (walkingToWaitingPoint);
				return;
			}

		}
	}

	// Code that runs when exiting the state.
	public override void OnExit ()
	{

	}


}
