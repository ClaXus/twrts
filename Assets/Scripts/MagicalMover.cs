using UnityEngine;
using System.Collections;

public class MagicalMover : MonoBehaviour {
	public float moveSpeed;

	private Transform myTransform;
	private Spell currentSpell;
	private int ccLuck;
	private float ratioForce;

	void Start () {
		myTransform = transform;
	}

	private bool canGo=false;
	
	private Vector3 direction;

	void Update(){
		if (!canGo)
			return;
		transform.position += direction * Time.deltaTime * moveSpeed;
	}

	public void CanGo(Vector3 forward, Spell me, int cc, int force){
		currentSpell = me;
		canGo = true;
		direction = forward;
		System.Random rd = new System.Random ();
		rd.NextDouble ();
		ccLuck =  (int)	(rd.NextDouble () * (float)cc);
		if (ccLuck == 0)
			ccLuck = 1;
		ratioForce = 1f + force*0.1f;
		StartCoroutine (afterTime(me.cooldown));
		//direction.y -= 0.1f;
	}

	IEnumerator afterTime(float time){
		yield return new WaitForSeconds (time);
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		Debug.LogWarning ("ONTRIGGER : " + other.name);
		if(other.name.Contains("Enemy")){
			int value = (int)(((float)((SpellDirected)currentSpell).value)*ratioForce*ccLuck);
			Debug.LogWarning("Degats : " + value);
			other.gameObject.GetComponent<Enemy>().dropHP(value);
			gameObject.SetActive(false);
		}
	}

}
