// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Adds a Component to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Component on exiting the state.")]
	public class AddComponent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to add the Component to.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
        [Title("Component Type"), Tooltip("The type of Component to add to the Game Object.")]
		public FsmString component;

        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(Component))]
        [Tooltip("Store the component in an Object variable. E.g., to use with Set Property.")]
	    public FsmObject storeComponent;

		[Tooltip("Remove the Component when this State is exited.")]
		public FsmBool removeOnExit;

		Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			component = null;
		    storeComponent = null;
		}

		public override void OnEnter()
		{
			DoAddComponent();
			
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		void DoAddComponent()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			addedComponent = go.AddComponent(GetType(component.Value));
		    storeComponent.Value = addedComponent;

			if (addedComponent == null)
			{
				LogError("Can't add component: " + component.Value);
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