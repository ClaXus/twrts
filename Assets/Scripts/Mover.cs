using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mover : NetworkBehaviour {
	[SyncVar]
	private Vector3 syncPos;
	[SyncVar]
	private Quaternion syncRot;



	public Collider collider;

	public Transform cameraHolder;
	public Rigidbody rigidbody;
	public float speed=10f;

	public Transform myTransform;
	
	private Vector3 lastPos;
	private Quaternion lastRot;
	//private float lerpRate = 10;
	private float posThreshold = 0.5f;
	private float rotThreshold = 5;

	
	private float x = 0f;
	private float y = 0f;

	private float rateSync = 10f;

	private float mouseX;
	private float mouseY;
	private float mouseZ;
	public float turnSpeed = 4.0f;
	private Vector3 offset;

	Vector3 zoomVector;

	public float maxZoom=0f;
	public float minZoom=-10f;

	public Transform theCamera;
	
	private float movementSpeed = 5.0f;
	private float rotationSpeed = 5.0f;
	private float currentRotation = 1.0f;

	private float distance = 4f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		InputMovement();
		InputRotation ();	
		if ( Input.GetKeyDown(KeyCode.W)){
			currentRotation += 180.0f;
		}
		if (Input.GetMouseButton (1)) {
			float inputX = Input.GetAxis( "Horizontal" );
			float inputY = Input.GetAxis( "Vertical" );
			float inputR = Mathf.Clamp( Input.GetAxis( "Mouse X" ), -1.0f, 1.0f );
			// get current position and rotation, then do calculations
			// position
			Vector3 moveVectorX = myTransform.forward * inputY;
			Vector3 moveVectorY = myTransform.right * inputX;
			Vector3 moveVector = ( moveVectorX + moveVectorY ).normalized * movementSpeed * Time.deltaTime;
			// rotation
			currentRotation = ClampAngle( currentRotation + ( inputR * rotationSpeed) );
			Quaternion rotationAngle = Quaternion.Euler( 0.0f, currentRotation, 0.0f );
			
			// update Character position and rotation
			myTransform.position = myTransform.position + moveVector;
			myTransform.rotation = rotationAngle;
			
			// update Camera position and rotation
			theCamera.position = cameraHolder.position;
			theCamera.rotation = cameraHolder.rotation;
		}

		if (IsGrounded()) {		
			if(Input.GetAxis("Mouse ScrollWheel") < 0){
				if(distance<15f){					
					Vector3 angles = myTransform.eulerAngles;
					
					x = angles.y;
					y = angles.x;
					
					x = x + (float) (Input.GetAxis("Mouse X") * 1f * 0.02);
					y = y + (float) (Input.GetAxis("Mouse Y") * 1f * 0.02);
					
					Quaternion rotation = Quaternion.Euler(0, x, 0);
					distance += 1;
					Vector3 newPosition = myTransform.position + rotation * -Vector3.forward * distance;
					newPosition.y= myTransform.position.y + 2f*distance*0.3f;
					theCamera.transform.position = newPosition;

					AjustRotationOfCamera(1f);
				}     
			}           
			
			if(Input.GetKeyDown(KeyCode.Space)){
				rigidbody.AddForce(Vector3.up *300f);
			}
			
			
			if(Input.GetAxis("Mouse ScrollWheel") > 0){
				if(distance >5f){
					Vector3 angles = myTransform.eulerAngles;
					
					x = angles.y;
					y = angles.x;
					
					x = x + (float) (Input.GetAxis("Mouse X") * 1f * 0.02);
					y = y + (float) (Input.GetAxis("Mouse Y") * 1f * 0.02);
					
					Quaternion rotation = Quaternion.Euler(0, x, 0);
					distance -= 1;
					
					Vector3 newPosition = myTransform.position + rotation * -Vector3.forward * distance;

					newPosition.y= myTransform.position.y + 2f*distance*0.3f;
					theCamera.transform.position = newPosition;
					AjustRotationOfCamera(-1f);
				}
			}
		}


	}
	//private bool ajustCamera = false;
	private float totalRotate=0f;
	private float minRotation = 20f;
	private float maxRotation = 28f;

	void AjustRotationOfCamera(float toAddNumber){
		if (theCamera.transform.eulerAngles.x>minRotation+0.1f && toAddNumber<-0.1f) {
			theCamera.transform.Rotate(toAddNumber,0,0);
		}
		else{
			theCamera.transform.Rotate(toAddNumber,0,0);
		}
		if (theCamera.transform.eulerAngles.x > maxRotation - 0.1f) {
			theCamera.transform.Rotate (-1, 0, 0);
			Debug.LogWarning("TOOUP");
			totalRotate -= 1;
		} else if (totalRotate <-0.1f) {
			theCamera.transform.Rotate (1, 0, 0);
			totalRotate += 1;
		
		}
	}
	
	bool IsGrounded() {
		return Physics.Raycast(myTransform.position, -(Vector3.up), collider.bounds.extents.y + 0.1f);
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


	private int simultaneousMovement=0;
	void InputMovement(){
		Vector3 vectorMove = Vector3.zero;
		if (Input.GetKey (KeyCode.Z)) {
			Vector3 g = theCamera.transform.forward;
			g.y=0;
			vectorMove+=g;
			//rigidbody.MovePosition (rigidbody.position + theCamera.transform.forward * speed * Time.deltaTime);
			simultaneousMovement+=1;
		}
		if (Input.GetKey (KeyCode.S)) {
			Vector3 g = theCamera.transform.forward;
			g.y=0;
			vectorMove-=g;
			//rigidbody.MovePosition (rigidbody.position - theCamera.transform.forward * speed * Time.deltaTime);
			simultaneousMovement +=1;
		}
		if (Input.GetKey (KeyCode.D) && simultaneousMovement < 2) {
			
			vectorMove+=theCamera.transform.right;
			//rigidbody.MovePosition (rigidbody.position + theCamera.transform.right * speed * Time.deltaTime);
			simultaneousMovement +=1;
		}
		if (Input.GetKey (KeyCode.Q) && simultaneousMovement < 2) {
			vectorMove-=theCamera.transform.right;
			//rigidbody.MovePosition (rigidbody.position - theCamera.transform.right * speed * Time.deltaTime);
			simultaneousMovement +=1;
		}
		if(simultaneousMovement>=1)
			rigidbody.MovePosition (rigidbody.position + vectorMove.normalized * speed * Time.deltaTime);
		simultaneousMovement = 0;
	}	

	void InputRotation(){

	}

	float ClampAngle (float theAngle){
		if ( theAngle < -360.0f )
		{
			theAngle += 360.0f;
		}
		else if ( theAngle > 360.0f )
		{
			theAngle -= 360.0f;
		}
		
		return theAngle;
	}
}
