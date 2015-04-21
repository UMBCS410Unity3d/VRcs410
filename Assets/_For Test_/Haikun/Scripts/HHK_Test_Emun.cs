using UnityEngine;
using System.Collections;

public class HHK_Test_Emun : MonoBehaviour {

	public enum E
	{
		A_Idle,
		B,
		C
	}
	E e = E.A_Idle;
	// Use this for initialization
	void Start () 
	{
		Debug.Log(e.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
