class QuickEdit_Base extends EditorWindow 
{	
	//external variables
	var vertHandlePrefab = (Resources.LoadAssetAtPath("Assets/ProCore/Shared/Prefabs/VertHandlePrefab.prefab", typeof(Object)));
	
	//states
	var editModeActive : boolean = false;
	var editingShared : boolean = false;
	
	//internally shared variables
	var sourceObject : GameObject;
	var vertHandles : Transform[];
	var newMeshName : String = "";
	var vertIndex : int = 0;
	var vertPositions : Vector3[];
	var sourceMesh : Mesh;
	var editMesh : Mesh;
	var backupMeshVertData : Vector3[];
	var backupMeshUVData : Vector2[];
	
	
	//selection variables
	var sel_Dirty : boolean = false;
	var storedSelection : GameObject;
	var storedLength : int;
	var vertHandleSelection : QuickEdit_VertHandleScript[];
	var activeObjectPrevTransform : TRS = new TRS(Vector3.zero, Quaternion.identity, Vector3.one);
	
	class TRS 
	{
		var translation : Vector3;
		var rotation : Quaternion;
		var scale : Vector3;

		function TRS(t : Vector3, r : Quaternion, s : Vector3)
		{
			translation = t;
			rotation = r;
			scale = s;
		}

		function Set(t : Transform)
		{
			translation = t.position;
			rotation = t.localRotation;
			scale = t.localScale;
		}

		function Equals(t : Transform)
		{
			return t.position == translation && t.localRotation == rotation && t.localScale == scale;
		}
	}

	//Update Loop
	function Update()
	{
		if(editModeActive)
		{
			//custom "OnSelectionChange"
			if(Selection.activeGameObject != storedSelection || Selection.gameObjects.length != storedLength)
			{
				storedSelection = Selection.activeGameObject;
				storedLength = Selection.gameObjects.length;
				FilterSelection();				
			}		
			//move verts
			if(Selection.activeTransform && !sel_Dirty)
			{
				if( !activeObjectPrevTransform.Equals(Selection.activeTransform) )
				{
					UpdateVertHandles();
					activeObjectPrevTransform.Set(Selection.activeTransform); 
				}			
			}
		}
	}
	//
	
	function EnterEditMode()
	{
		editModeActive = true;

		sourceObject = Selection.activeGameObject;
		sourceMesh = sourceObject.GetComponent(MeshFilter).sharedMesh;
		
		//Undo.RegisterUndo(sourceObject.GetComponent(MeshFilter), "Edit Mesh");
		
		//save the starting mesh data, so it can be cancelled
		backupMeshVertData = sourceMesh.vertices;
		backupMeshUVData = sourceMesh.uv;
		
		if(!editingShared) //copy the source mesh, save it, and make it the new used mesh
		{
			editMesh = new Mesh();
			
			editMesh.vertices = sourceMesh.vertices;
			editMesh.uv = sourceMesh.uv;			
			editMesh.uv2 = sourceMesh.uv2;			
			editMesh.triangles = sourceMesh.triangles;
			editMesh.normals = sourceMesh.normals;
			editMesh.tangents = sourceMesh.tangents;
			
			//get/set triangles for submeshes (yayy!)
			editMesh.subMeshCount = sourceMesh.subMeshCount;
			for(var t : int = 0;t<sourceMesh.subMeshCount;t++)
			{
				editMesh.SetTriangles(sourceMesh.GetTriangles(t), t);
			}
			//

			AssetDatabase.CreateAsset(editMesh, AssetDatabase.GenerateUniqueAssetPath("Assets/ProCore/QuickEdit/Meshes/"+newMeshName+".asset"));
			AssetDatabase.SaveAssets();

			sourceObject.GetComponent(MeshFilter).sharedMesh = editMesh;
		}		
		else //edit the source mesh directly
		{
			editMesh = sourceMesh;
		}

		vertPositions = editMesh.vertices;
		vertHandles = new Transform[0];
		vertIndex = 0;
		Selection.objects = new Array();
		vertHandleSelection = new QuickEdit_VertHandleScript[0];
		AssignVertHandles();
	}
	
	function CancelMeshEdit()
	{
		if(!editingShared)
		{
			sourceObject.GetComponent(MeshFilter).sharedMesh = sourceMesh;
		}
		else
		{
			sourceMesh.vertices = backupMeshVertData;
			sourceMesh.uv = backupMeshUVData;
		}
		
		for(theVertHandle in vertHandles)
		{
			if(theVertHandle)
			{
				DestroyImmediate(theVertHandle.gameObject);
			}
		}

		Selection.objects = new Array();
		Selection.activeObject = sourceObject;

		editModeActive = false;
	}
	
	function ExitEditMode()
	{
		for(theVertHandle in vertHandles)
		{
			if(theVertHandle)
			{
				DestroyImmediate(theVertHandle.gameObject);
			}
		}
		
		// //special addition for MT - this will copy the new mesh into the MeshCollider (if available) as well
		// if(sourceObject.GetComponent(MeshCollider) != null)
		// {
		// 	sourceObject.GetComponent(MeshCollider).sharedMesh = editMesh;
		// }
		
		RefreshColliders(sourceObject);
		
		Selection.objects = new Array();
		Selection.activeObject = sourceObject;
		editModeActive = false;
	}

	function RefreshColliders(obj : GameObject)
	{
		var msh : Mesh = obj.GetComponent(MeshFilter).sharedMesh;
		
		msh.RecalculateBounds();
		
		if(obj.GetComponent(Collider))
		{
			for(var c : Collider in obj.GetComponents(Collider))
			{
				var t : System.Type = c.GetType();

				if(t == typeof(BoxCollider))
				{
					c.center = msh.bounds.center;
					c.size = msh.bounds.size;
				} else
				if(t == typeof(SphereCollider))
				{
					c.center = msh.bounds.center;
					c.radius = Mathf.Max(Mathf.Max(msh.bounds.extents.x, msh.bounds.extents.y), msh.bounds.extents.z);
				} else
				if(t == typeof(CapsuleCollider))
				{
					c.center = msh.bounds.center;
					c.radius = Mathf.Maxx( msh.bounds.extents.x, msh.bounds.extents.z);
					c.height = msh.bounds.size.y;
				} else
				if(t == typeof(WheelCollider))
				{
					c.center = msh.bounds.center;
					c.radius = Mathf.Max(Mathf.Max(msh.bounds.extents.x, msh.bounds.extents.y), msh.bounds.extents.z);
				} else
				if(t == typeof(MeshCollider))
				{
					obj.GetComponent(MeshCollider).sharedMesh = null;
					obj.GetComponent(MeshCollider).sharedMesh = msh;
				} 
			}
		}
	}
	
	//check the pos of each vert, and assign to vert handle or make new vert handle as needed
	function AssignVertHandles()
	{		
		for(theVertPos in vertPositions)
		{			
			if(!VertHandleHere(theVertPos, vertIndex))
			{
				//create a new vert handle at this vert's pos and set it up
				var newVertHandle = new GameObject();//Instantiate(vertHandlePrefab, Vector3.zero, Quaternion.identity);
				newVertHandle.AddComponent(QuickEdit_VertHandleScript);
				newVertHandle.transform.parent = sourceObject.transform;
				newVertHandle.transform.localPosition = theVertPos;
				newVertHandle.GetComponent(QuickEdit_VertHandleScript).Activate();
				newVertHandle.GetComponent(QuickEdit_VertHandleScript).AddVertIndex(vertIndex);
				newVertHandle.name = "VertHandle_"+vertIndex;
					
				// don't do this 'cause DrawGizmos can't be called on hidden objects
				// newVertHandle.hideFlags = HideFlags.HideInHierarchy;

				//add the new vert handle to the vert handle array
				var tempHandles = new Array();
				tempHandles = vertHandles;
				tempHandles.Add(newVertHandle.transform);
				vertHandles = tempHandles;
			}
			vertIndex++;
		}
	}
	
	function VertHandleHere(theVertPos : Vector3, vertIndex : int) : boolean
	{
		for(theVertHandle in vertHandles)
		{
			if(theVertHandle.transform.localPosition == theVertPos)
			{
				theVertHandle.gameObject.GetComponent(QuickEdit_VertHandleScript).AddVertIndex(vertIndex);
				return true;
			}
		}
		return false;
	}
	
	function NameIsUnique(newMeshName : String) : boolean
	{
		if(AssetDatabase.LoadAssetAtPath("Assets/ProCore/QuickEdit/Meshes/"+newMeshName+".asset", typeof(Object)))
		{
			return false;
		}
		else
			return true;
	}
	
	function FilterSelection()
	{
		sel_Dirty = true;
		if(editModeActive)
		{
			for(var i : int = 0;i<vertHandleSelection.length;i++)
			{
				vertHandleSelection[i].isSelected = false;
			}
			
			var tempHandleSelection = new Array();
			for(var v=0;v<Selection.gameObjects.length;v++)
			{
				if(Selection.gameObjects[v].GetComponent(QuickEdit_VertHandleScript))
				{
					tempHandleSelection.Add(Selection.gameObjects[v].GetComponent(QuickEdit_VertHandleScript));
				}
			}
			vertHandleSelection = tempHandleSelection;
			
			if(Selection.activeTransform)
			{
				if(vertHandleSelection.length > 0)
				{
					var tempSel = new Array();
					for(tvh in vertHandleSelection)
					{
						tempSel.Add(tvh.gameObject);
					}
					Selection.objects = tempSel;
					
					activeObjectPrevTransform.Set(Selection.activeTransform);

					for(theVertHandle in vertHandleSelection)
					{
						theVertHandle.isSelected = true;
					}
					activeObjectCurrentPos = Selection.activeTransform.position;
				}
			}
			else
				Selection.objects = new Array();
		}
		sel_Dirty = false;
	}
	
	function UpdateVertHandles()
	{
		for(vertHandle in vertHandleSelection)
		{
			vertHandle.UpdateAttachedVerts(editMesh);
		}
	}
}
