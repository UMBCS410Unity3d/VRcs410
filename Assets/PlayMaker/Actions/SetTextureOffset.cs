// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the Offset of a named texture in a Game Object's Material. Useful for scrolling texture effects.")]
	public class SetTextureOffset : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		[RequiredField]
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;
		[RequiredField]
		public FsmFloat offsetX;
		[RequiredField]
		public FsmFloat offsetY;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			namedTexture = "_MainTex";
			offsetX = 0;
			offsetY = 0;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetTextureOffset();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetTextureOffset();
		}

		void DoSetTextureOffset()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			if (go.GetComponent<Renderer>() == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			
			if (go.GetComponent<Renderer>().material == null)
			{
				LogError("Missing Material!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				go.GetComponent<Renderer>().material.SetTextureOffset(namedTexture.Value, new Vector2(offsetX.Value, offsetY.Value));
			}
			else if (go.GetComponent<Renderer>().materials.Length > materialIndex.Value)
			{
				var materials = go.GetComponent<Renderer>().materials;
				materials[materialIndex.Value].SetTextureOffset(namedTexture.Value, new Vector2(offsetX.Value, offsetY.Value));
				go.GetComponent<Renderer>().materials = materials;
			}
		}

	}
}