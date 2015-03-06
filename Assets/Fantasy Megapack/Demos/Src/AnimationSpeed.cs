using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour {

	#pragma warning disable
	public string name;
	#pragma warning restore
	public float speed = 1;

	// Use this for initialization
	void Start () {
		GetComponent<Animation>()[name].speed = speed;
	}
}
