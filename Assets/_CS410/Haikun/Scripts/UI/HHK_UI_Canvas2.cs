using UnityEngine;
using System.Collections;

public class HHK_UI_Canvas2 : MonoBehaviour {

	public HHK_UI_Panel_Base uiPanel;

	// Use this for initialization
	void Start () 
	{
		uiPanel.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		HHK_Role_Tags role = other.GetComponent<HHK_Role_Tags>();
		if (role)
		{
			HHK_Role_Tags.TAG[] tags= {HHK_Role_Tags.TAG.Role_Player};
			if (role.Is_Belong_Teams_Either(tags))
			{
				uiPanel.gameObject.SetActive(true);
			}
		}
		

	}
	
	void OnTriggerExit(Collider other) 
	{
		HHK_Role_Tags role = other.GetComponent<HHK_Role_Tags>();
		if (role)
		{
			HHK_Role_Tags.TAG[] tags= {HHK_Role_Tags.TAG.Role_Player};
			if (role.Is_Belong_Teams_Either(tags))
			{
				uiPanel.gameObject.SetActive(false);
			}
		}
	}
}
