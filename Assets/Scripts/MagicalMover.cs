using UnityEngine;
using System.Collections;

public class MagicalMover : MonoBehaviour {
	public float moveSpeed;

	private Transform myTransform;
	private Spell currentSpell;

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

	public void CanGo(Vector3 forward, Spell me){
		currentSpell = me;
		canGo = true;
		direction = forward;
		StartCoroutine (afterTime(me.cooldown));
		//direction.y -= 0.1f;
	}

	IEnumerator afterTime(float time){
		yield return new WaitForSeconds (time);
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		Debug.LogWarning (other.name);
		if(other.name.Contains("Enemy")){
			//Destroy(other.gameObject);
			Debug.LogWarning("Degats : " + ((SpellDirected)currentSpell).value);
		}
	}

}
