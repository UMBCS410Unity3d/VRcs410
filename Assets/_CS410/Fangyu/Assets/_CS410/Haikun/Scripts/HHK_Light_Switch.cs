using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(HHK_Role_Tags))]
public class HHK_Light_Switch : MonoBehaviour 
{
	// light
	public Light target;

	// which key is used to interact with the light
	public KeyCode actionKey = KeyCode.Mouse0; 

	// allow the light blink randomly.
	public bool blink = false;

	float default_intensity = 1.0f;

	// Use this for initialization
	void Start () 
	{
		if (target)
		{
			// setup default_intensity
			target.intensity = default_intensity;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// blinking
		if (target && target.enabled
		    && blink)
		{

			target.intensity = Random.Range(0.0f, default_intensity);
		}
	}

	void OnTriggerStay(Collider other)
	{

		// when the player click the key, then turn on/off the light
		if (Input.GetKeyDown(actionKey))
		{
			HHK_Role_Tags rt = other.gameObject.GetComponent<HHK_Role_Tags>();
			if (rt)
			{
				// player get stay this trigger
				HHK_Role_Tags.TAG[] tags = {HHK_Role_Tags.TAG.Role_Player};
				if (rt.Is_Belong_Teams(tags))
				{
					if (target)
					{
						target.enabled = !target.enabled;
					}
				}
				
			}
		}
	}

}
