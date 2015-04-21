/****************************************
	ParticleSpawner.js v1.0
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com		
 	
 	GUI Buttons to view Particle Systems from list																							
*****************************************/
#pragma strict
import System.Collections.Generic;	//Used to sort particle system list
//Visible properties
var particles:GameObject[];			//Particle systems to add a button for each
var maxButtons:int = 10;			//Maximum buttons per page	
var showInfo:boolean;

//Hidden properties
private var page:int = 0;			//Current page
private var pages:int;				//Number of pages
private var currentPSInfo:String;	//Current particle info
private var currentPS:GameObject;

function Start(){
	//Sort particle system list alphabeticly
    particles.Sort(particles, function(g1,g2) String.Compare(g1.name, g2.name));
	//Calculate number of pages
	pages = Mathf.Ceil((particles.length -1 )/ maxButtons);
	//Debug.Log(pages);
}

function OnGUI () {
	//Time Scale Vertical Slider
	Time.timeScale = GUI.VerticalSlider (Rect (185, 50, 20, 150), Time.timeScale, 2.0, 0.0);
	//Field of view Vertical Slider
		//Camera.mainCamera.fieldOfView = GUI.VerticalSlider (Rect (225, 50, 20, 150), Camera.mainCamera.fieldOfView, 20.0, 100.0);
	//Check if there are more particle systems than max buttons (true adds "next" and "prev" buttons)
	if(particles.length > maxButtons){
		//Prev button
		if(GUI.Button(Rect(20,(maxButtons+1)*18,75,18),"Prev"))if(page > 0)page--;else page=pages;
		//Next button
		if(GUI.Button(Rect(95,(maxButtons+1)*18,75,18),"Next"))if(page < pages)page++;else page=0;
		//Page text
		GUI.Label (Rect(60,(maxButtons+2)*18,150,22), "Page" + (page+1) + " / " + (pages+1));
		
	}
	//Toggle button for info
	showInfo = GUI.Toggle (Rect(185, 20,75,25), showInfo, "Info");
	
	//System info
	if(showInfo)GUI.Label (Rect(250, 20,500,500), currentPSInfo);
	
	//Calculate how many buttons on current page (last page might have less)
	var pageButtonCount:int = particles.length - (page*maxButtons);
	//Debug.Log(pageButtonCount);
	if(pageButtonCount > maxButtons)pageButtonCount = maxButtons;
	
	//Adds buttons based on how many particle systems on page
	for(var i:int=0;i < pageButtonCount;i++){
		if(GUI.Button(Rect(20,i*18+18,150,18),particles[i+(page*maxButtons)].transform.name)){
			if(currentPS) Destroy(currentPS);
			var go:GameObject = Instantiate(particles[i+page*maxButtons]);
			currentPS = go;
			PlayPS(go.GetComponent(ParticleSystem), i + (page * maxButtons) +1);
			InfoPS(go.GetComponent(ParticleSystem), i + (page * maxButtons) +1);
		}
	}
}
//Play particle system (resets time scale)
function PlayPS (_ps:ParticleSystem, _nr:int){
		Time.timeScale = 1;
		_ps.Play();
}

function InfoPS (_ps:ParticleSystem, _nr:int){
		//Change current particle info text
		currentPSInfo = "System" + ": " + _nr + "/" + particles.length +"\n"+
		"Name: " + _ps.gameObject.name +"\n\n" +
		"Main PS Sub Particles: " + _ps.transform.childCount  +"\n" +
		"Main PS Materials: " + _ps.GetComponent.<Renderer>().materials.length +"\n" +
		"Main PS Shader: " + _ps.GetComponent.<Renderer>().material.shader.name;
		//If plasma(two materials)
		if(_ps.GetComponent.<Renderer>().materials.length >= 2)currentPSInfo = currentPSInfo + "\n\n *Plasma not mobile optimized*";
		//Usage Info
		currentPSInfo = currentPSInfo + "\n\n Use mouse wheel to zoom, click and hold to rotate";
		currentPSInfo = currentPSInfo.Replace("(Clone)", "");
}