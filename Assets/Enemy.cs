using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour {

	NavMeshAgent nMA;

	private GameObject gO;

	[SerializeField]
	MeshRenderer mR;

	[SerializeField]
	Material redMaterial;

	[SerializeField]
	Material baseMaterial;

	[SerializeField]
	int HP=1000;

	[SerializeField]
	TextMesh damagesInfo;

	[SerializeField]
	int BaseAttackForce;

	[SerializeField]
	int SpecialAttackForce;

	Transform myTransform;

	public Transform target;

	string toAttackUnitTag="Totem";

	bool firstTime = true;

	void Start () {
		gO = gameObject;
		nMA = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag(toAttackUnitTag).transform;
		//nMA.CalculatePath (transform.position, nMA.path);
		myTransform = transform;
		
		if(isServer)
		{
			//Debug.LogWarning ("GoToPlayer0");
			StartCoroutine(DoCheck());
		}

		//nMA.destination = target.position;
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
			target = GameObject.FindGameObjectWithTag(toAttackUnitTag).transform;
			if(target==null && firstTime){ // Initiation Scene
				toAttackUnitTag = "Player";
			}
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
		//Debug.LogWarning ("GoToPlayer2");
		nMA.SetDestination(dest.position);
		nMA.Resume ();
	}

	void FixedUpdate () {
		//nMA.SetDestination(target.position);
		
		if(target==null && isServer)
		{
			//Debug.LogWarning("SearchPlayer");
			StartCoroutine(DoCheck());
		}
	}

	public void dropHP(int damages){
		HP -= damages;
		damagesInfo.text = "" + damages;
		damagesInfo.gameObject.SetActive (true);
		StartCoroutine (takeDamages());
	}

	IEnumerator takeDamages(){
		GameObject currentGo = gO;
		mR.material = redMaterial;
		yield return new WaitForSeconds (0.3f);
		damagesInfo.gameObject.SetActive (false);
		mR.material = baseMaterial;
		if (HP < 0)
			Destroy (gO);
	}

	void OnTriggerEnter(Collider other) {
		Debug.LogWarning ("ONTRIGGER Enemy : " + other.name);
		if(other.name.Contains(toAttackUnitTag)){

		}
	}

}
