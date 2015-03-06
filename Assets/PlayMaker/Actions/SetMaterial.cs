// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the material on a game object.")]
	public class SetMaterial : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		[RequiredField]
		public FsmMaterial material;

		public override void Reset()
		{
			gameObject = null;
			material = null;
			materialIndex = 0;
		}

		public override void OnEnter()
		{
			DoSetMaterial();
			
			Finish();
		}

		void DoSetMaterial()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			if (go.GetComponent<Renderer>() == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				go.GetComponent<Renderer>().material = material.Value;
			}
			else if (go.GetComponent<Renderer>().materials.Length > materialIndex.Value)
			{
				var materials = go.GetComponent<Renderer>().materials;
				materials[materialIndex.Value] = material.Value;
				go.GetComponent<Renderer>().materials = materials;
			}
		}
	}
}