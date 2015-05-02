// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	public class AddScript : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to add the script to.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The Script to add to the GameObject.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString script;
		
		[Tooltip("Remove the script from the GameObject when this State is exited.")]
		public FsmBool removeOnExit;

		Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			script = null;
		}

		public override void OnEnter()
		{
			DoAddComponent(gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value);
			
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value)
            {
                if (addedComponent != null)
                {
                    Object.Destroy(addedComponent);
                }
            }
		}

		void DoAddComponent(GameObject go)
		{
			if (go == null) return;
			addedComponent =  go.AddComponent(GetType(script.Value));

			if (addedComponent == null)
			{
				LogError("Can't add script: " + script.Value);
			}
		}

        // Temporary Type Helper for 1.7.8
        // In Unity 4.x you could use AddComponent(string) leaving off namespace
        // In Unity 5.x you have to use AddComponent(type)
        // So old data is missing namespace info and will fail
        // This is fixed internally in 1.8.0, but we need this fix for 1.7.8
        // TODO: Remove this fix in 1.8.0
	    static Type GetType(string name)
        {
            var type = ReflectionUtils.GetGlobalType(name);
            if (type != null) return type;

            type = ReflectionUtils.GetGlobalType("UnityEngine." + name);
            if (type != null) return type;

            type = ReflectionUtils.GetGlobalType("HutongGames.PlayMaker." + name);

            return type;
        }

	}
}