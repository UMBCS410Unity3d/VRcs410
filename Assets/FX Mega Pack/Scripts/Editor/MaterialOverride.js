/****************************************
	Material Overide v1.0
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com
 	
 	    A tool that makes it possible to attach two materials to a shuriken particle system 
        Can also be used to change from two to one materials 	
        																																
*****************************************/

class MaterialOverride extends EditorWindow {
  
    
    var systemParent:ParticleSystem;
    var material0:Material;
    var material1:Material;
    var scaleMultiplier:float = 1;
    
    @MenuItem ("Unluck/Material Override")
    
    
    static function ShowWindow () {
        EditorWindow.GetWindow (MaterialOverride);
    }
      
    function OnGUI () {
    	if(Selection.activeTransform != null){
    	systemParent = Selection.activeTransform.GetComponent.<ParticleSystem>();
    		
    	}
    	EditorGUILayout.LabelField("Force 2 or 1 material(s) on shuriken particle system(s)");
    	EditorGUILayout.LabelField("(consider using 1 material for slower devices)");
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Get materials from shuriken particle system in scene");
       // systemParent = EditorGUILayout.ObjectField ("Particle System: ", systemParent, typeof (ParticleSystem), true) as ParticleSystem;
        if (GUILayout.Button("Get Materials"))
        {
            getMaterials();
        }
       
    	material0 = EditorGUILayout.ObjectField ("Material 0: ", material0, typeof (Material), true) as Material;
    	material1 = EditorGUILayout.ObjectField ("Material 1: ", material1, typeof (Material), true) as Material;
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Apply materials to selected shuriken particle system(s)");
    	if (GUILayout.Button("Set Materials"))
        {
            overideMaterials();
        }
    }
    
    function overideMaterials() {
    
			var newArray:Array = new Array();
			if(material0 != null){
				newArray.push(material0);
			}
			if(material1 != null){
				newArray.push(material1);
			}
			if(newArray.length > 0){
				for(var i:int=0; i < Selection.transforms.Length; i++){
				//	Debug.Log(i+""+Selection.transforms.Length);
					if(Selection.transforms[i].GetComponent(ParticleSystemRenderer)!=null)
					Selection.transforms[i].GetComponent(ParticleSystemRenderer).sharedMaterials= newArray;
				}
           		//systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials= newArray;
            }
    }
    
    function getMaterials() {
    	if(systemParent != null){
    		if(systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials[0] != null){
    			material0=systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials[0];
    		}
    		if(systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials[1] != null){
    			material1=systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials[1];
    		}
    	}
    }
}