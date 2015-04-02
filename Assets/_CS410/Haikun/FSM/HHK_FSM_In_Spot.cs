using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_In_Spot : HHK_FSM_Action
{
	public FsmEvent afterSpot;

	public float time_to_leave;

	float timeout;
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();
	
		// set the time for using
		time_to_leave = Random.Range(npc.spot.mixTime, npc.spot.maxTime);
		// stop
		// agent.Stop();
		// play animation via spot
		npc.role_controller.CrossFade(npc.spot.actionName);

		timeout = 0.5f;
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		timeout -= Time.deltaTime;
		if (timeout >= 0.0f) {
			return;
		}

		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();

		// rotataion
		npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, npc.spot.transform.rotation, 
		                                     npc.agent.angularSpeed * Time.deltaTime);

		// count down
		time_to_leave -= Time.deltaTime;
		if (time_to_leave <= 0.0f)
		{
			Fsm.Event(afterSpot);
			return;
		}
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{

	}


}
