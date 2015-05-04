using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System;  
using System.Net;
using System.Net.Mail;
using System.Net.Security;  
using System.Security.Cryptography.X509Certificates;


public class HHK_UI_CS_Dept : HHK_UI_Panel_Base {

	public InputField emailAddress;

	float walkspeed = 0.0f;

	// Use this for initialization
	void Awake () 
	{
		// disable it self
		gameObject.SetActive(false);
	}

	void OnEnable()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player") as GameObject;
		FirstPersonController fpc = go.GetComponent<FirstPersonController>();
		if (fpc)
		{
			// normal mode
			walkspeed = fpc.m_WalkSpeed;
			fpc.m_WalkSpeed = 0;
		}

		OVRPlayerController ovr = go.GetComponent<OVRPlayerController>();
		if (ovr)
		{
			// VR mode
			ovr.enabled = false;
		}

		// enable functional
		Collider[] colls = gameObject.GetComponentsInChildren<Collider>();
		foreach(Collider col in colls)
		{
			col.enabled = true;
		}

		// in case in the VR mode, user lost the focus.
		emailAddress.text = "";
		emailAddress.interactable = true;
		emailAddress.ActivateInputField();


	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player") as GameObject;
		OVRPlayerController ovr = go.GetComponent<OVRPlayerController>();
		if (ovr)
		{
			// VR mode
			ovr.enabled = false;
			// in case in the VR mode, user lost the focus.
			emailAddress.ActivateInputField();
		}

	}

	// *** event *** // 

	public void BT_Send()
	{
		// send a email here.
		if (emailAddress.text != "")
			StartCoroutine(SendEmail());

	}

	// override the base function
	override public void BT_Cancel()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player") as GameObject;
		FirstPersonController fpc = go.GetComponent<FirstPersonController>();
		if (fpc)
		{
			// normal mode
			fpc.m_WalkSpeed = walkspeed;
		}
		OVRPlayerController ovr = go.GetComponent<OVRPlayerController>();
		if (ovr)
		{
			// VR mode
			ovr.enabled = true;
		}
		emailAddress.text = "";
		gameObject.SetActive(false);
	}

	// send email
	IEnumerator SendEmail()
	{
		emailAddress.interactable = false;

		// active the mission1
		MessageSystem.Send(MessageSystem.Mission1,emailAddress.text);

		// Send email
//		HHK_EmailSystem.Get().Send(emailAddress.text,
//									"Digital Walkers say hello to you!",
//		                           ("Hello, this message is sent from CS Department of Virtual UMass Boston.\n" +
//		                           			"Also, you active a mission, please go to room S-03-075 to check it out. " +
//		                           			"Good Luck!" +
//		                           			"-- Digital Walkers --"));


		//yield return new WaitForSeconds(1f);

		MailMessage mail = new MailMessage();  	
		mail.From = new MailAddress("umbcs410dw@gmail.com");  
		mail.To.Add(emailAddress.text);  

		// emailAddress.text = "Sending...";


		mail.Subject = "Digital Walkers say hello to you!";  
		mail.Body = "Hello, this message is sent from CS Department of Virtual UMass Boston.\n" +
			"Also, you active a mission, please go to room S-03-075 to check it out. " +
			"Good Luck!" +
			"-- Digital Walkers --";  
		// mail.Attachments.Add(new Attachment("Screen.png"));  
		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");  
		smtpServer.Port = 587;  
		smtpServer.Credentials 
			= new System.Net.NetworkCredential("umbcs410dw@gmail.com", "Ab123456.") as ICredentialsByHost;  
		smtpServer.EnableSsl = true;  
		ServicePointManager.ServerCertificateValidationCallback =  
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)  
		{ return true; };  
		smtpServer.Send(mail); 
		emailAddress.text = "";
		// yield return new WaitForSeconds(3f);

		// emailAddress.interactable = false;
		// emailAddress.text = "Sending...";

		GameObject go = GameObject.FindGameObjectWithTag("Player") as GameObject;
		FirstPersonController fpc = go.GetComponent<FirstPersonController>();
		if (fpc)
		{
			// normal mode
			fpc.m_WalkSpeed = walkspeed;
		}
		OVRPlayerController ovr = go.GetComponent<OVRPlayerController>();
		if (ovr)
		{
			// VR mode
			ovr.enabled = true;
		}
		
		// disable functional
		Collider[] colls = gameObject.GetComponentsInChildren<Collider>();
		foreach(Collider col in colls)
		{
			col.enabled = false;
		}

		yield return new WaitForSeconds(3f);

		emailAddress.text = "";
		emailAddress.interactable = true;
		gameObject.SetActive(false);

		
		yield return new WaitForSeconds(0.1f);
	}

}
