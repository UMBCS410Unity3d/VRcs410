using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Die : HHK_FSM_Action
{

	// Code that runs on entering the state.
	public override void OnEnter()
	{
	
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables> ();
	
		// play animation via spot
		npc.role_controller.CrossFade(HHK_Role_Controller.AnimName.die);

		HHK_Role_Tags tag = Owner.GetComponent<HHK_Role_Tags>();

		// force to leave the area
		if (npc.waiting_point)
		{
			npc.waiting_point.Leave_From_This_Place(tag);
		}
		if (npc.spot)
		{
			npc.spot.Leave_From_This_Place(tag);
		}

		npc.agent.Stop();

		// send a message
		MessageSystem.Send(MessageSystem.NPC_Die, npc);
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}


}
