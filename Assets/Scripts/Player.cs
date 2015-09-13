﻿using UnityEngine;
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
	public Mover moverScript;
	
	[SerializeField]
	GameObject gameObject;

	[SerializeField]
	public Camera playerCamera;

	[SerializeField]
	public GameObject[] spellAnimation;

	public int playerCC=2;
	public int playerF=2;
	public int playerRes=1;
	public int playerVit=1;
	
	//public Player gm;

	private Button currentButton;
	private int currentIndex;
	private FastGameScene FgS;
	private int nbPhysicAttacks=0;
	private Vector3 spawnPosition;

	int[] stats;

	void Start () {
		if (isLocalPlayer) {
			moverScript.enabled = true;
		}


		//InformationLoader iLoader = new InformationLoader ();
		//PlayerInformations pI = iLoader.LoadPlayerInformations ();
		//if(pI!=null)
		//	stats = pI.Style;
		FgS = FindObjectOfType(typeof(FastGameScene)) as FastGameScene;
		if (FgS != null) {
			FgS.initializeButtons (ref spellButtons, this);
			FgS.getInstantiateSpells(this);
		} else {
			Initiation init = FindObjectOfType(typeof(Initiation)) as Initiation;
			init.initializeButtons (ref spellButtons, this);
			init.getInstantiateSpells(this);
		}
	}

	void Update () {
		if (!isLocalPlayer)
			return;
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentIndex = 0;
			currentButton = spellButtons [0];
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
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			currentIndex = 8;
			currentButton = spellButtons [5];
			moverScript.StartCoroutine (b1Timer ());
			nbPhysicAttacks += 1;
		}
	}

	public void btnClicked(Button b){
		currentButton = b;
		currentIndex = Array.IndexOf (spellButtons, currentButton);
		if (currentIndex >= 4)
			currentIndex += 3;
		moverScript.StartCoroutine(b1Timer());
	}
	
	public IEnumerator b1Timer(){
		Debug.LogWarning ("b1Timer");
		yield return StartCoroutine( changeAspect() );
	}

	public IEnumerator changeAspect(){
		if (currentButton.interactable) {
			Button veryCurrentButton = currentButton;
			veryCurrentButton.interactable = false;
			ColorBlock cb = veryCurrentButton.colors;
			cb.normalColor = cb.disabledColor;
			cb.highlightedColor = cb.disabledColor;
			veryCurrentButton.colors = cb; 
			yield return new WaitForSeconds (associateSpells[currentIndex].castTime);
			yield return StartCoroutine( launchSpell() );
			yield return new WaitForSeconds (associateSpells[currentIndex].cooldown-0.2f);
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
			veryCurrentButton.interactable = true;
			veryCurrentButton.colors = cb;

		}
	}
	
	public IEnumerator launchSpell(){
		if (currentButton.gameObject.activeSelf) {
			int realIndex;
			if(currentIndex>1){
				if(currentIndex>7)
					realIndex = 1;
				else {
					realIndex = 2;
				}
			}
			else{
				realIndex = 0;
			}
			GameObject currentAnimation = spellAnimation[realIndex];

			currentAnimation.transform.position = moverScript.gameObject.transform.position + moverScript.gameObject.transform.forward*2;
			
			currentAnimation.gameObject.SetActive(true);

			currentAnimation.GetComponent<MagicalMover>().CanGo(moverScript.gameObject.transform.forward, associateSpells[currentIndex], playerCC, playerF);
		}
		yield return null;
	}

	public void setSpawnPosition(Vector3 spawnPosition){
		this.spawnPosition = spawnPosition;
	}

	public Vector3 getSpawnPosition(){
		return this.spawnPosition;
	}

	private Initiation init;

	void OnTriggerEnter(Collider other) {

		//Debug.LogWarning (other.name);
		if(other.name.Equals("Spotlight")){
			init = (FindObjectOfType(typeof(Initiation)) as Initiation);
			init.StepAfterLight();
		}
	}

	public void updateStats(int []stats){
		playerCC	=stats[0];
		playerF		=stats[1];
		playerRes	=stats[2];
		playerVit	=stats[3];
		moverScript.changeSpeed (playerVit);
	}

}
