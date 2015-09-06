using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Initiation : NetworkBehaviour {
	
	[SerializeField]
	Button[] buttonsPanelButtons;

	[SerializeField]
	Text messageText;

	[SerializeField]
	Text explicationText;
	
	[SerializeField]
	Button panelButtons;

	[SerializeField]
	GameObject[] myAllies;
	
	[SerializeField]
	Button LeftButton;

	[SerializeField]
	Button RightButton;

	[SerializeField]
	Text LeftButtonText;
	
	[SerializeField]
	Text RightButtonText;

	private int numeroStep=0;

	private float targetTime = 4f;
	private int currentState;
	
	private int TimeToPlace;
	
	private Player currentPlayer;

	private Vector3 spawnPosition;
	// Use this for initialization

	//private GameManager gM;

	private int numberOfSpawns=0;
	
	private int[] stats;

	[SerializeField]
	Light mySpot; 


	void Start () {
		panelButtons.gameObject.SetActive (true);
		messageText.text = "Lumière.";
		(FindObjectOfType(typeof(GameManager)) as GameManager).AddPlayerForAGame(new Vector3(0,0,0));
		stats = new int[4];
		// CC
		stats[0] = 2;
		// Force
		stats[1] = 2;
		// Resistance
		stats[2] = 1;
		// Vitalité
		stats[3] = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LeftButtonClick(){
		if (numeroStep == 1) {
			stats [0] += 1;
			LeftButtonText.text = "Début";
			RightButtonText.text = "Fin";
		} else if (numeroStep == 2) {
			stats [1] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else if (numeroStep == 3) {
			stats [numeroStep - 1] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else if (numeroStep == 4) {
			stats [numeroStep - 1] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else {
			LeftButton.gameObject.SetActive(false);
			RightButton.gameObject.SetActive(false);
			
			messageText.text = "Puissance.";
			explicationText.text = "Le combat" + System.Environment.NewLine + "est" + System.Environment.NewLine + "une nécessité";
		}
		numeroStep += 1;
	}

	public void RightButtonClick(){
		if (numeroStep == 1) {
			stats [3] += 1;
			LeftButtonText.text = "Début";
			RightButtonText.text = "Fin";
		} else if (numeroStep == 2) {
			stats [2] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else if (numeroStep == 3) {
			stats [numeroStep - 1] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else if (numeroStep == 4) {
			stats [numeroStep - 1] += 1;
			LeftButtonText.text = "Proximité";
			RightButtonText.text = "Distance";
		} else {
			LeftButton.gameObject.SetActive(false);
			RightButton.gameObject.SetActive(false);
			messageText.text = "Tracer.";
			explicationText.text = "Le combat" + System.Environment.NewLine + "est" + System.Environment.NewLine + "une nécessité";
		}
		numeroStep += 1;
	}

	public void initializeButtons(ref Button[] spellButtons, Player p){
		//spellButtonsFSG = spellButtons;
		currentPlayer = p;

		currentPlayer.enabled = true;
		currentPlayer.playerCamera.gameObject.SetActive (true);
		currentPlayer.moverScript.enabled = true;
		for (int i=0;i<spellButtons.Length && i< buttonsPanelButtons.Length; i++) {
			Debug.LogWarning ("Initialize Button " + i);
			AddListener(buttonsPanelButtons[i], "init");
			spellButtons[i] = buttonsPanelButtons[i];
		}
	}
	
	void AddListener(Button b, string value){
		Debug.LogWarning ("Add Listener" + b.ToString());
		b.onClick.AddListener(() => currentPlayer.btnClicked(b));
	}


	bool StepAfterLightOk = true;
	public void StepAfterLight(){
		if (StepAfterLightOk) {
			messageText.text = "Conscient.";
			explicationText.text = "Les choix guident" + System.Environment.NewLine + "la destinée" + System.Environment.NewLine + "des vrais héros";
			numeroStep += 1;
			StepAfterLightOk = false;
			LeftButton.enabled = true;
			LeftButton.gameObject.SetActive(true);
			RightButton.gameObject.SetActive(true);
			RightButton.enabled = true;
		}
	}

}
