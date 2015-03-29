// Haikun Huang

// this is a manager of entry points, it is responsibility is organize all 
// the entry points in currently level.
// and store the currently start point which was assigned by pervious level.
// every level only has one and only one HHK_Entry_Points_Manager, 
// and required a player object in the level.


using UnityEngine;
using System.Collections;

public class HHK_Entry_Points_Manager : MonoBehaviour 
{
	// was assigned by pervious level
	public static int which_start_point = 0;
	// once the player exit the area of the entry point, that means he ready for shift to next level.
	// this var is prevent the shifting function go into a dead end.
	public static bool ready_to_go = false;
	// this var is user to fixed the problem of the default character which is provided by Unity3D
	public float offset_y = 1.2f;

	// a array of entry points.
	public HHK_Entry_Point[] entry_points;
	
	// Use this for initialization
	void Start () 
	{
		// find the player object
		HHK_Role_Tags.TAG[] tags = {HHK_Role_Tags.TAG.Role_Player};
		HHK_Role_Tags[] players = HHK_Role_Tags.Find_Objects_Belong_Teams(tags);

		// set the player to the start point;
		players[0].transform.position = entry_points[which_start_point].transform.position;
		// fixed the position.
		players[0].transform.position = new Vector3(players[0].transform.position.x,
		                                            players[0].transform.position.y + offset_y,
		                                            players[0].transform.position.z);
		players[0].transform.rotation = entry_points[which_start_point].transform.rotation;

		ready_to_go = false; // reset
	}


}
