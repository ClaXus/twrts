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
	
	private float rateSync = 10f;
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){		
		TransmitMotion();
		LerpMotion();
	}
	
	protected void LerpMotion () {
		if (!isLocalPlayer) {
			//Debug.LogWarning ("LerpPosition");
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * rateSync);
		}
	}
	
	[Command]
	protected void CmdProvideMotionToServer () {
		//Debug.LogWarning ("CmdPosition");
		syncPos = myTransform.position;
		syncRot = myTransform.rotation;
	}
	
	[ClientCallback]
	protected void TransmitMotion(){
		if (!isServer)
			return;
		if (Vector3.Distance(myTransform.position, lastPos) > posThreshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold)
		{
			lastPos = myTransform.position;
			lastRot = myTransform.rotation;
			
			syncPos = myTransform.position;
			syncRot = myTransform.rotation;
		}
		if (isLocalPlayer) {
			CmdProvideMotionToServer ();
		}
	}


}
