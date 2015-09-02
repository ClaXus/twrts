using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Player : NetworkBehaviour {

	[SerializeField]
	Button[] spellButtons;
	
	[SerializeField]
	Spell[] associateSpells;
	
	[SerializeField]
	JobPattern[] associateJobs;

	[SerializeField]
	Mover moverScript;
	
	[SerializeField]
	GameObject gameObject;
	
	[SerializeField]
	Player myPlayer;

	[SerializeField]
	public Camera playerCamera;

	private Button currentButton;
	private int currentIndex;
	private FastGameScene FgS;
	private int nbPhysicAttacks=0;
	private Vector3 spawnPosition;
	public Player gm;
	// Use this for initialization
	void Start () {

		if (isLocalPlayer) {
			moverScript.enabled = true;
		}		
		if (isServer)
			gm = this;
		
		myPlayer = this;
		FgS = FindObjectOfType(typeof(FastGameScene)) as FastGameScene;
		FgS.initializeButtons (ref spellButtons, this);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		//Debug.LogWarning ("Do Action !");
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentIndex = 0;
			currentButton = spellButtons [0];
			Debug.LogWarning ("Fire Ball !");
			moverScript.StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentIndex = 1;
			currentButton = spellButtons [1];
			moverScript.StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentIndex = 2;
			currentButton = spellButtons [0];
			moverScript.StartCoroutine (b1Timer ());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentIndex = 3;
			currentButton = spellButtons [3];
			moverScript.StartCoroutine (b1Timer ());
		}
		
		if (Input.GetKeyDown (KeyCode.A)) {
			currentIndex = 7;
			currentButton = spellButtons [4];
			moverScript.StartCoroutine (b1Timer ());
			nbPhysicAttacks += 1;
			if(nbPhysicAttacks>1){
				spellButtons [1].gameObject.SetActive(true);
				spellButtons[1].image.sprite = associateSpells[1].correspondingSprite;
			}
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			currentIndex = 8;
			currentButton = spellButtons [5];
			moverScript.StartCoroutine (b1Timer ());
		}
	}

	public void btnClicked(Button b){
		currentButton = b;
		currentIndex = Array.IndexOf (spellButtons, currentButton);
		if (currentIndex >= 4)
			currentIndex += 3;
		Debug.LogWarning ("CurrentIndex:" + currentIndex);
		moverScript.StartCoroutine(b1Timer());
	}
	
	public IEnumerator b1Timer(){
		Debug.LogWarning ("b1Timer");
		yield return StartCoroutine( changeAspect() );
	}
	
	public IEnumerator changeAspect(){
		
		Debug.LogWarning ("changeAspect");
		if (currentButton.gameObject.activeSelf) {
			Debug.LogWarning ("changeAspectOK");
			Button veryCurrentButton = currentButton;
			veryCurrentButton.interactable = false;
			ColorBlock cb = veryCurrentButton.colors;
			cb.normalColor = cb.disabledColor;
			cb.highlightedColor = cb.disabledColor;
			veryCurrentButton.colors = cb; 
			yield return new WaitForSeconds (associateSpells[currentIndex].castTime);
			//yield return StartCoroutine( launchSpell() );
			yield return new WaitForSeconds (associateSpells[currentIndex].cooldown);
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
			veryCurrentButton.interactable = true;
			veryCurrentButton.colors = cb;
		}
	}
	
	public IEnumerator launchSpell(){
		if (currentButton.gameObject.activeSelf) {
			Button veryCurrentButton = currentButton;
			veryCurrentButton.interactable = false;
			ColorBlock cb = veryCurrentButton.colors;
			cb.normalColor = cb.disabledColor;
			cb.highlightedColor = cb.disabledColor;
			veryCurrentButton.colors = cb; 
			yield return new WaitForSeconds (associateSpells[currentIndex].castTime);
			//yield return StartCoroutine( launchSpell() );
			yield return new WaitForSeconds (associateSpells[currentIndex].cooldown);
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
			veryCurrentButton.interactable = true;
			veryCurrentButton.colors = cb;
		}

		this.tag = "lauchingSpell";
		currentButton.tag = "launchingSpell";
	}

	public void setSpawnPosition(Vector3 spawnPosition){
		this.spawnPosition = spawnPosition;
	}

	public Vector3 getSpawnPosition(){
		return this.spawnPosition;
	}

}
