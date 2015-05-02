using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;  
using System.Net;  
using System.Net.Mail;  
using System.Net.Security;  
using System.Security.Cryptography.X509Certificates;

public class HHK_UI_S3_075 : HHK_UI_Panel_Base {

	public Text process;
	public Button button;
	public Text buttonText;
	public int missionCount = 3;
	bool mission_done = false;

	// Use this for initialization
	void Start () 
	{
		button.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// update botton
		if (HHK_Simple_Mission.Get().mission1_process < missionCount)
		{
			button.gameObject.SetActive(false);
		}
		else
		{
			button.gameObject.SetActive(true);
		}
		process.text = "Process : " + HHK_Simple_Mission.Get().mission1_process + " / " + missionCount;

	}

	public void BT_OK()
	{
		if (HHK_Simple_Mission.Get().mission1_process >= missionCount 
		    && !mission_done)
		{
			mission_done = true;
			StartCoroutine(SendEmail());
		}
	}


	// send email
	IEnumerator SendEmail()
	{
		//yield return new WaitForSeconds(1f);
		//Debug.Log(HHK_Simple_Mission.Get().emailAddress);
//		MailMessage mail = new MailMessage();  	
//		mail.From = new MailAddress("umbcs410dw@gmail.com");  
//		mail.To.Add(HHK_Simple_Mission.Get().emailAddress);  
//
//		
//		yield return new WaitForSeconds(0.1f);
//		
//		mail.Subject = "CS 410 project";  
//		mail.Body = "Hey, you passed CS 410.\n" +
//				"-- Digital Walkers --";  
//		// mail.Attachments.Add(new Attachment("Screen.png"));  
//		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");  
//		smtpServer.Port = 587;  
//		smtpServer.Credentials 
//			= new System.Net.NetworkCredential("umbcs410dw@gmail.com", "Ab123456.") as ICredentialsByHost;  
//		smtpServer.EnableSsl = true;  
//		ServicePointManager.ServerCertificateValidationCallback =  
//			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)  
//		{ return true; };  
//		smtpServer.Send(mail); 
		//emailAddress.text = "";
		//yield return new WaitForSeconds(3f);


		HHK_EmailSystem.Get().Send(HHK_Simple_Mission.Get().emailAddress,
		                           "CS 410 project",
		                           ("Hey, you passed CS 410.\n" +
		 				"-- Digital Walkers --"));


	
		button.interactable = false;
		buttonText.text = "Completed.";
		yield return null;
	}



}
