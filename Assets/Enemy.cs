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

	void Start () {
		gO = gameObject;
		nMA = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		//nMA.CalculatePath (transform.position, nMA.path);
		myTransform = transform;
		
		if(isServer)
		{
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
			target = GameObject.FindGameObjectWithTag("Player").transform;
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

	void Update () {
		//nMA.SetDestination(target.position);
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

}
