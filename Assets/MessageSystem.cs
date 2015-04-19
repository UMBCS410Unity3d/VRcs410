//Haikun Huang

/**
 * 
 *
 * 
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour 
{
	// message quene format
	class Message
	{
		public string name;
		public object value;
		public float time;
		
		public Message (string n, object v)
		{
			name = n;
			value = v;
			time = 0.0f;
		}
	}

	// singleton
	static MessageSystem singleton = null;

	static public MessageSystem Get()
	{
		return singleton;
    }

	public float time_out = 2.0f;

	public bool debugMode = false;

	Dictionary<string, Queue<Message>> msgMap = new Dictionary<string, Queue<Message>>();
	
	void Awake()
	{
		singleton = this;
	}

	void LateUpdate()
	{
		// Debug.Log("waiting messages: "+ msgMap.Count);

		// add up the time
		foreach(string key in msgMap.Keys)
		{
			foreach(Message m in msgMap[key])
			{
				m.time += Time.deltaTime;
				// if time out
				if (m.time>=time_out)
				{
					// dequeue this event and debug it
					msgMap[key].Dequeue();
					if (debugMode)
					{
						Debug.LogError("Message: [" + key +"] time out!");
					}
					break;
				}
			}
		}

	}

	static public void Send(string eventName, object value = null)
	{
		if (!singleton.msgMap.ContainsKey(eventName))
			singleton.msgMap[eventName] = new Queue<Message>();

		singleton.msgMap[eventName].Enqueue(new Message(eventName,value));

	}

	static public bool Listen(string eventName)
	{
		if (singleton.msgMap.ContainsKey(eventName))
		{
			if (singleton.msgMap[eventName].Count > 0)
				return true;
			else
			{
				singleton.msgMap.Remove(eventName);
			}
		}
		return false;
	}

	// get next message's value
	static public object Next(string eventName)
	{
//		if (!Listen(eventName))
//			return null;

		return singleton.msgMap[eventName].Dequeue().value;

	}

	static public string[] All_Event()
	{
		List<string> ret = new List<string>();
		foreach (string key in singleton.msgMap.Keys)
		{
			ret.Add(key);
		}
		return ret.ToArray();
	}

}
