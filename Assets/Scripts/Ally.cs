using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ally : NetworkBehaviour {

	[SyncVar] 
	private Vector3 syncPos;
	[SyncVar]
	private Quaternion syncRot;

	private Transform myTransform;
	
	private Vector3 lastPos;
	private Quaternion lastRot;
	//private float lerpRate = 10;
	private float posThreshold = 0.5f;
	private float rotThreshold = 5;
	
	public float moveSpeed = 10.0f;
	public Vector3 velocity;
	private float rateSync = 10f;

	void Start () {
		myTransform = transform;
	}

	void FixedUpdate(){
		if (!base.isServer)
			return;
		// transform bullet on the server
		int i = Random.Range (2, 18);
		if(i%2==0)
			transform.position += transform.forward * Time.deltaTime * moveSpeed;
		else
			transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}
	/*
	protected void LerpMotion () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	protected void CmdProvideMotionToServer () {
		syncPos = myTransform.position;
		syncRot = myTransform.rotation;
	}
	
	[ClientCallback]
	protected void TransmitMotion(){
		if (!isServer)
			return;
		if (Vector3.Distance(myTransform.position, lastPos) > posThreshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold) {
			lastPos = myTransform.position;
			lastRot = myTransform.rotation;
			
			syncPos = myTransform.position;
			syncRot = myTransform.rotation;
		}

		if (isLocalPlayer) {
			CmdProvideMotionToServer ();
		}
	}
	*/

}
