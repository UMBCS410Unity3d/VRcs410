#pragma strict
private var rotTarget:Quaternion;
var rotateEverySecond:float = 1;
private var lerpCounter:float;
private var rotCache:Quaternion;

function Start () {
	randomRot ();
	InvokeRepeating("randomRot", 0,rotateEverySecond);
}

function Update(){
	transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, lerpCounter*Time.deltaTime);
	lerpCounter++;
}

function randomRot () {
	 
	 rotTarget = Random.rotation;
	 rotCache = transform.rotation;
	 lerpCounter = 0;
}