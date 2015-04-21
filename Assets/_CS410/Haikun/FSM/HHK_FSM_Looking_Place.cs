using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("FSM")]
public class HHK_FSM_Looking_Place : HHK_FSM_Action
{
	public FsmEvent walkingToPlace;
	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		HHK_FSM_Walking_NPC_Variables npc = Owner.GetComponent<HHK_FSM_Walking_NPC_Variables>();

		// find all the wp1
		HHK_Way_Point_Level_1[] wp1s = HHK_Way_Point_Level_1.FindAllPlace();
		if (npc.place == null || wp1s.Length <= 1)
		{
			npc.place = wp1s[Random.Range(0,wp1s.Length)];
		}
		else
		{
			// no same place again
			HHK_Way_Point_Level_1 wp = wp1s[Random.Range(0,wp1s.Length)];
			while (wp == npc.place)
			{
				wp = wp1s[Random.Range(0,wp1s.Length)];
			}
			npc.place = wp;
		}
		// send event
		Fsm.Event(walkingToPlace);
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
