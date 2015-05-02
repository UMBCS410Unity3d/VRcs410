#pragma strict
@script ExecuteInEditMode

var isActive : boolean = false;
var isSelected : boolean = false;
var myVertIndices = new int[0];

function Activate()
{
	isActive = true;
}

function AddVertIndex(vertIndex : int)
{
	var tempIndices = new Array();
	tempIndices = myVertIndices;
	tempIndices.Add(vertIndex);
	myVertIndices = tempIndices;
}

function UpdateAttachedVerts(theMesh : Mesh) 
{	
	var meshVerts = theMesh.vertices;
	for(theVertIndex in myVertIndices)
	{
		meshVerts[theVertIndex] = transform.parent.InverseTransformPoint(transform.position);
	}
	theMesh.vertices = meshVerts;
}

#if UNITY_EDITOR
@DrawGizmo(GizmoType.NotSelected|GizmoType.Pickable)
static function RenderOffGizmo(me : QuickEdit_VertHandleScript, gizType : GizmoType)
{
		Gizmos.DrawIcon(me.transform.position, "ProCore/VertOff.tga", false);
}

@DrawGizmo(GizmoType.Selected|GizmoType.Pickable)
static function RenderOnGizmo(me : QuickEdit_VertHandleScript, gizType : GizmoType)
{
		Gizmos.DrawIcon(me.transform.position, "ProCore/VertOn.tga", false);
}
#endif