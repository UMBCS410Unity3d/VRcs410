#pragma strict

function Start () {

}

function Update () {
	transform.rotation.eulerAngles.y += 1 * Time.deltaTime;
}