// Haikun Huang
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HHK_Role_Tags))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HHK_Role_Controller))]
public class HHK_Test_Walking_NPC_2 : MonoBehaviour {
	
	public enum STATE
	{
		Looking_WP_1,
		Looking_WP_2,
		Looking_WP_Waiting_Point,
		Walking_to_WP_1,
		Walking_to_WP_2,
		Walking_to_Waiting_Point,
		In_Spot,
		In_Waiting_Line,
		None,
	};
	
	// current state
	public STATE state = STATE.Looking_WP_1;

	// some speed
	float speed_walk = 1.8f;
	public float speed_run = 3.5f;

	// some place
	HHK_Way_Point_Level_1 place;
	HHK_Way_Point_Level_2 spot;
	HHK_Waiting_Point waiting_point;
	
	NavMeshAgent agent;
	HHK_Role_Tags role;
	
	float speed_for_stop_limit = 0.05f;
	
	float time_to_leave = 0.0f;
	
	// waiting index of the line
	int index_of_waiting_line = 0;

	// role controller
	HHK_Role_Controller role_controller;
	
	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
		role = GetComponent<HHK_Role_Tags>();
		role_controller = GetComponent<HHK_Role_Controller>();
		// idle at beginning
		role_controller.Play_Animation(HHK_Role_Controller.Idle);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// select a action to do, base on the current state
		switch(state)
		{
		case STATE.Looking_WP_1:
			Looking_For_WP_1();
			break;
		case STATE.Looking_WP_2:
			Looking_For_WP_2();
			break;
		case STATE.Looking_WP_Waiting_Point:
			break;
		case STATE.Walking_to_WP_1:
			Walking_to_WP_1();
			break;
		case STATE.Walking_to_WP_2:
			Walking_to_WP_2();
			break;
		case STATE.Walking_to_Waiting_Point:
			Walking_to_Waiting_Point();
			break;
		case STATE.In_Spot:
			In_Spot();
			break;
		case STATE.In_Waiting_Line:
			In_the_Waiting_Line();
			break;
		default:
			break;
		}
	}
	
	// looking for a wp 1, and than switch to walking wp 1state
	void Looking_For_WP_1()
	{
		// find all the wp1
		HHK_Way_Point_Level_1[] wp1s = FindObjectsOfType<HHK_Way_Point_Level_1>();
		place = wp1s[Random.Range(0,wp1s.Length)];
		
		// switch to walking
		state = STATE.Walking_to_WP_1;
		// set the destination
		agent.SetDestination(place.transform.position);
		// play animation.
		role_controller.Play_Animation(HHK_Role_Controller.Run);
		agent.speed = speed_run;

	}
	
	// looking any available wp 2, and switch to the walking wp 2 state
	// if not, switch to the looking for waiting point state
	void Looking_For_WP_2()
	{
		// if the place is null, return to looking for wp 1 state
		if (!place)
		{
			state = STATE.Looking_WP_1;
			return;
		}
		
		spot = place.Any_Available_Spot();
		
		// find a available spot
		if (spot)
		{
			// switch to walking
			state = STATE.Walking_to_WP_2;
			// set the destination
			agent.SetDestination(spot.transform.position);
			// play animation.
			role_controller.Play_Animation(HHK_Role_Controller.Walk);
			agent.speed = speed_walk;
		}
		// go to the waiting point
		// if there is no any waiting point, go back to the looking for wp 1 state
		else
		{
			if (place.waittingPoint)
			{
				// get the waiting point;
				waiting_point = place.waittingPoint;
				// go to the waitting line
				state = STATE.Walking_to_Waiting_Point;
				// join the line, and get the index of the line,
				// otherwise, looking for the next wp 1
				if (!waiting_point.Join_To_This_Place(role))
				{
					state = STATE.Looking_WP_1;
					return;
				}
				index_of_waiting_line = waiting_point.Get_My_Index(role);
				// set the destination
				agent.SetDestination(waiting_point.Get_My_Position(role));
				// play animation.
				role_controller.Play_Animation(HHK_Role_Controller.Walk);
				agent.speed = speed_walk;
			}
			// switch to walking
			else
			{
				state = STATE.Looking_WP_1;
			}
		}
		
	}
	
	// walking to wp 1
	// if reach the place, switch to the looking for wp 2 state
	void Walking_to_WP_1()
	{
		// if the place is null, return to looking for wp 1 state
		if (!place)
		{
			state = STATE.Looking_WP_1;
			return;
		}
		
		// check, if reach the place, switch to looking for wp 2 state
		// and stop
		if (place.Is_In_This_Place(role))
		{
			// switch to looking wp 2
			state = STATE.Looking_WP_2;
			// stop
			// agent.Stop();
			// play animation.

		}
	}
	
	// walking to wp 2
	// keep cheaking this spot, if not ready, just return to looking for wp 2 state
	// once reach the spot, do the action for using, and switch to the in spot state
	void Walking_to_WP_2()
	{
		if (!spot)
		{
			state = STATE.Looking_WP_2;
			// stop
			// agent.Stop();
			// play animation.
			return;
		}
		
		// if not ready and I am not in this spot, witch to the in spot state
		if (!spot.Is_Ready())
		{
			if (!spot.Is_In_This_Place(role))
			{
				state = STATE.Looking_WP_2;
				// stop
				// agent.Stop();
				// play animation.
				return;
			}
		}
		
		// if at the right position, switch to the in spot state
		if (agent.desiredVelocity.magnitude <= speed_for_stop_limit)
		{
			state = STATE.In_Spot;
			// set the time for using
			time_to_leave = Random.Range(spot.mixTime, spot.maxTime);
			// stop
			// agent.Stop();
			// play animation.
			role_controller.Play_Animation(HHK_Role_Controller.Idle);

		}
	}
	
	// in spot
	// leave when the time out, switch to the looking for wp 1
	void In_Spot()
	{

		// if not ready and I am not in this spot, witch to the in spot state
		if (!spot.Is_Ready())
		{
			if (!spot.Is_In_This_Place(role))
			{
				state = STATE.Looking_WP_2;
				// stop
				agent.Stop();
				// play animation.
				return;
			}
		}

		// rotataion
		transform.rotation = Quaternion.Lerp(transform.rotation, spot.transform.rotation, 
		                                     agent.angularSpeed * Time.deltaTime);
		
		// count down
		time_to_leave -= Time.deltaTime;
		if (time_to_leave <= 0.0f)
		{
			// if has a next place need to go
			if (spot.next_place)
			{
				place = spot.next_place;
				state = STATE.Walking_to_WP_1;
				// set the destination
				agent.SetDestination(place.transform.position);
				// play animation.
				role_controller.Play_Animation(HHK_Role_Controller.Run);
				agent.speed = speed_run;
			}
			else
			{
				state = STATE.Looking_WP_1;
				// play animation.
			}
		}
	}
	
	// walking to the waiting point
	// keep checking the new waiting index of the line
	// keep checking any availabe spot
	void Walking_to_Waiting_Point()
	{
		if (!waiting_point)
		{
			state = STATE.Looking_WP_1;
			return;
		}
		
		// if at the right position, switch to the in spot state
		if (agent.desiredVelocity.magnitude <= speed_for_stop_limit)
		{
			state = STATE.In_Waiting_Line;
			// stop
			// agent.Stop();
			// play animation.
			role_controller.Play_Animation(HHK_Role_Controller.Idle);
		}
		
		// keep checking the new waiting index of the line, move to the new position
		if (index_of_waiting_line != waiting_point.Get_My_Index(role))
		{
			// go to the waitting line
			state = STATE.Walking_to_Waiting_Point;
			// join the line, and get the index of the line
			index_of_waiting_line = waiting_point.Get_My_Index(role);
			// set the destination
			agent.SetDestination(waiting_point.Get_My_Position(role));
			// play animation.
			role_controller.Play_Animation(HHK_Role_Controller.Walk);
			agent.speed = speed_walk;
		}
		
		// keep checking any availabe spot
		if(index_of_waiting_line == 0)
		{
			spot = place.Any_Available_Spot();
			if (spot)
			{
				// switch to walking to wp 2
				state = STATE.Walking_to_WP_2;
				// leave this place
				waiting_point.Leave_From_This_Place(role);
				// join the wp2
				spot.Join_From_The_Waiting_Line(role);
				// set the destination
				agent.SetDestination(spot.transform.position);
				// play animation
				role_controller.Play_Animation(HHK_Role_Controller.Walk);
				agent.speed = speed_walk;
			}
		}
	}
	
	// in the waiting line
	// keep checking the new waiting index of the line
	// keep checking any availabe spot
	void In_the_Waiting_Line()
	{

		if (!waiting_point)
		{
			state = STATE.Looking_WP_1;
			return;
		}

		// rotataion
		transform.rotation = Quaternion.Lerp(transform.rotation, waiting_point.transform.rotation, 
		                                     agent.angularSpeed * Time.deltaTime);
		
		// keep checking the new waiting index of the line, move to the new position
		if (index_of_waiting_line != waiting_point.Get_My_Index(role))
		{
			// go to the new waiting position
			state = STATE.Walking_to_Waiting_Point;
			// join the line, and get the index of the line
			index_of_waiting_line = waiting_point.Get_My_Index(role);
			// set the destination
			agent.SetDestination(waiting_point.Get_My_Position(role));
			// play animation.
			role_controller.Play_Animation(HHK_Role_Controller.Walk);
			agent.speed = speed_walk;
			
		}
		
		// keep checking any availabe spot
		if(index_of_waiting_line == 0)
		{
			spot = place.Any_Available_Spot();
			if (spot)
			{
				// switch to walking to wp 2
				state = STATE.Walking_to_WP_2;
				// leave this place
				waiting_point.Leave_From_This_Place(role);
				// join the wp2
				spot.Join_From_The_Waiting_Line(role);
				// set the destination
				agent.SetDestination(spot.transform.position);
				// play animation
				role_controller.Play_Animation(HHK_Role_Controller.Walk);
				agent.speed = speed_walk;
			}
		}
	}
	
}
