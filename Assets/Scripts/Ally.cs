using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ally : NetworkBehaviour {
	
	NavMeshAgent nMA;

	[SyncVar] 
	private Vector3 syncPos;
	[SyncVar]
	private Quaternion syncRot;

	[SerializeField]
	Material redMaterial;
	
	[SerializeField]
	Material baseMaterial;
	
	[SerializeField]
	int HP=1000;
	
	[SerializeField]
	TextMesh damagesInfo;
	
	[SerializeField]
	int BaseAttackForce = 33;
	
	[SerializeField]
	int SpecialAttackForce = 145;

	[SerializeField]
	MeshRenderer mR;
	
	
	public Transform target;

	private Transform myTransform;

	private GameObject targetGO;

	private Vector3 lastPos;
	private Quaternion lastRot;
	//private float lerpRate = 10;
	private float posThreshold = 0.5f;
	private float rotThreshold = 5;
	
	public float moveSpeed = 10.0f;
	public Vector3 velocity;
	private float rateSync = 10f;

	
	private System.Random rd = new System.Random ();
	
	void Start () {
		myTransform = transform;
		
		nMA = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Enemy").transform;
		if(isServer)
		{
			StartCoroutine(DoCheck());
		}
	}

	void FixedUpdate(){
		if(target==null && isServer)
		{
			StartCoroutine(DoCheck());
		}
	}

	IEnumerator DoCheck()
	{
		for(;;)
		{
			SearchForTarget();
			MoveToTarget();
			yield return new WaitForSeconds(0.2f);
		}
	}
	void SearchForTarget()
	{
		if(!isServer)
		{
			return;
		}
		
		if(target == null) {
			targetGO = GameObject.FindGameObjectWithTag("Enemy");
			if(targetGO)
				target = targetGO.transform;
		}
	}
	
	void MoveToTarget()
	{
		if(target != null && isServer)
		{
			SetNavDestination(target);
		}
	}
	
	void SetNavDestination(Transform dest) {
		nMA.SetDestination(dest.position);
	}

	public void dropHP(int damages){
		HP -= damages;
		damagesInfo.text = "" + damages;
		damagesInfo.gameObject.SetActive (true);
		StartCoroutine (takeDamages());
	}
	
	IEnumerator takeDamages(){
		mR.material = redMaterial;
		yield return new WaitForSeconds (0.3f);
		damagesInfo.gameObject.SetActive (false);
		mR.material = baseMaterial;
		if (HP < 0)
			Destroy (gameObject);
	}

	
	void OnTriggerEnter(Collider other) {
		Debug.LogWarning ("ONTRIGGER Ally : " + other.name);
		if(other.name.Contains("Enemy")){
			if(rd.NextDouble()>0.75){
				Debug.LogWarning("Dégats : " + SpecialAttackForce);
				other.gameObject.GetComponent<Enemy>().dropHP(SpecialAttackForce);
			}
			else{
				Debug.LogWarning("Dégats : " + BaseAttackForce);
				other.gameObject.GetComponent<Enemy>().dropHP(BaseAttackForce);
			}
			//gameObject.SetActive(false);
		}
	}
	void OnTriggerStay(Collider other) {
		Debug.LogWarning ("ONTRIGGER Ally : " + other.name);

		if(other.name.Contains("Enemy") && (rd.NextDouble()>0.75)){
			if(rd.NextDouble()>0.75){
				Debug.LogWarning("Dégats : " + SpecialAttackForce);
				other.gameObject.GetComponent<Enemy>().dropHP(SpecialAttackForce);
			}
			else{
				Debug.LogWarning("Dégats : " + BaseAttackForce);
				other.gameObject.GetComponent<Enemy>().dropHP(BaseAttackForce);
			}
		}    
	}

}
