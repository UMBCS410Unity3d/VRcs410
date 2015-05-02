using UnityEngine;
using System.Collections;
using System;  
using System.Net;  
using System.Net.Mail;  
using System.Net.Security;  
using System.Security.Cryptography.X509Certificates;
using System.Threading;


public class HHK_EmailSystem : MonoBehaviour 
{

	// singleton
	static  HHK_EmailSystem singleton = null;

	static string to, title, body;
	
	static public HHK_EmailSystem Get()
	{
		return singleton;
	}

	void Awake()
	{
		singleton = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Send(string to1, string title1, string body1)
	{
		to = to;
		title = title1;
		body = body1;

		ThreadStart entry = new ThreadStart(SendEmail);//求和方法被定义为工作线程入口  
		Thread workThread = new Thread(entry);  
		workThread.Start();  
	}

	void SendEmail()
	{
		Debug.Log("email sending...");

		//yield return new WaitForSeconds(1f);
		MailMessage mail = new MailMessage();  	
		mail.From = new MailAddress("umbcs410dw@gmail.com");  
		
		mail.Subject = title ; 
		mail.Body = body;  
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

		Debug.Log("email sent.");

	}
}
