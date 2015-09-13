using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Initiation : NetworkBehaviour {

	public enum GameState{
		MotionResponseState,
		KillEnemyState,
		PositionState
	}

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

	[SerializeField]
	Light mySpot; 

	[SerializeField]
	GameObject[] spawnPoints;
	
	[SerializeField]
	public GameObject[] instantiateSpells;

	[SerializeField]
	GameObject finalPanel;
	
	[SerializeField]
	Text winText;

	[SerializeField]
	GameObject []toDisableAfterWin;

	[SerializeField]
	GameObject []toEnableAfterWin;

	[SerializeField]
	Camera placementCamera;

	private int numeroStep=0;
	
	private float targetTime = 4f;
	
	private int TimeToPlace;
	
	private Player currentPlayer;
	
	private Vector3 spawnPosition;
	
	private int numberOfSpawns=0;
	
	private int[] stats;

	private int currentState;

	private GameManager gM;

	private string pseudoChoice;

	void Start () {
		panelButtons.gameObject.SetActive (true);
		messageText.text = "Lumière.";
		gM = (FindObjectOfType (typeof(GameManager)) as GameManager);
		gM.AddPlayerForAGame(new Vector3(0,0,0));
		stats = new int[4];
		// CC
		stats[0] = 2;
		// Force
		stats[1] = 2;
		// Resistance
		stats[2] = 1;
		// Vitalité
		stats[3] = 1;

		currentState = (int)GameState.MotionResponseState;
	}

	void Update () {
		if ((int)GameState.PositionState == currentState) {
				if (Input.GetMouseButtonDown (0)) {
				Debug.LogWarning("LeftClick");
				RaycastHit hit;
				if (Physics.Raycast (placementCamera.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)), out hit)) {
					
					Debug.LogWarning("RayCastOK");
							spawnPosition = hit.point;
							spawnPosition.y += 1;
							//gM.AddPlayerForAGame(spawnPosition);
								Cmd_SpawnRealAlly();
						
								Cmd_SpawnAlly();
								Cmd_SpawnAlly();
						
								currentPlayer.moverScript.enabled = true;
								currentPlayer.playerCamera.gameObject.SetActive(true);
								placementCamera.gameObject.SetActive(false);
								
								messageText.text = "Tracer.";
								explicationText.text = "De ton destin" + System.Environment.NewLine + "dépend" + System.Environment.NewLine + "l'issue du combat";
								
								currentState = (int)GameState.KillEnemyState;							
						}
				}
		}
		if ((int)GameState.KillEnemyState == currentState) {
			if(!FindObjectOfType(typeof(Enemy))){
				messageText.text = "Avenir.";
				explicationText.text = "Sois fier" + System.Environment.NewLine + "d'etre à présent" + System.Environment.NewLine + "un Revealers !";
				foreach (GameObject gO in toDisableAfterWin){
					gO.SetActive(false);
				}
				foreach (GameObject gO in toEnableAfterWin){
					gO.SetActive(true);
				}
				
				bool[] choices = new bool[4];

				InformationLoader iLoader = gM.GetComponent<InformationLoader>();
				if(iLoader.Informations.Choices!=null)
					choices = iLoader.Informations.Choices;
				choices[0] = true;
				iLoader.SavePlayerInformations(iLoader.Informations.PlayerPseudo, 100, 620, stats, choices);
				currentPlayer.moverScript.enabled = false;
			}
		}
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
			stats [2] += 1;
			LeftButtonText.text = "Projection";
			RightButtonText.text = "Distopie";
		} else if (numeroStep == 4) {
			stats [1] += 1;
			LeftButtonText.text = "Unaire";
			RightButtonText.text = "Binaire";
		} else {
			stats [0] += 1;

			LeftButton.gameObject.SetActive(false);
			RightButton.gameObject.SetActive(false);

			currentPlayer.updateStats(stats);

			currentPlayer.moverScript.enabled = false;
			currentPlayer.playerCamera.gameObject.SetActive(false);
			placementCamera.gameObject.SetActive(true);

			currentState = (int)GameState.PositionState;

			messageText.text = "Confier.";
			explicationText.text = "Les alliés" + System.Environment.NewLine + "sont" + System.Environment.NewLine + "la clef !";

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
			stats [0] += 1;
			LeftButtonText.text = "Projection";
			RightButtonText.text = "Distopie";
		} else if (numeroStep == 4) {
			stats [3] += 1;
			LeftButtonText.text = "Unaire";
			RightButtonText.text = "Binaire";
		} else {
			stats [1] += 1;

			LeftButton.gameObject.SetActive(false);
			RightButton.gameObject.SetActive(false);


			currentPlayer.moverScript.enabled = false;
			currentPlayer.playerCamera.gameObject.SetActive(false);
			placementCamera.gameObject.SetActive(true);
			
			currentState = (int)GameState.PositionState;
			
			messageText.text = "Confier.";
			explicationText.text = "Les alliés" + System.Environment.NewLine + "sont" + System.Environment.NewLine + "la clef !";

			currentPlayer.updateStats(stats);

			currentState = (int)GameState.PositionState;

		}
		numeroStep += 1;
	}

	public void initializeButtons(ref Button[] spellButtons, Player p){
		currentPlayer = p;

		currentPlayer.enabled = true;
		currentPlayer.playerCamera.gameObject.SetActive (true);
		currentPlayer.moverScript.enabled = true;
		for (int i=0;i<spellButtons.Length && i< buttonsPanelButtons.Length; i++) {
			//Debug.LogWarning ("Initialize Button " + i);
			AddListener(buttonsPanelButtons[i], "init");
			spellButtons[i] = buttonsPanelButtons[i];
		}
	}
	public void Cmd_SpawnAlly(){
		Debug.LogError ("CmdSpawn");
		myAllies[numberOfSpawns] = (GameObject) Instantiate(myAllies[numberOfSpawns], spawnPoints[numberOfSpawns].transform.position,  spawnPoints[numberOfSpawns].transform.rotation);
		NetworkServer.Spawn(myAllies[numberOfSpawns]);
		numberOfSpawns +=1;
	}

	public void Cmd_SpawnRealAlly(){
		Debug.LogError ("CmdSpawn");
		myAllies[numberOfSpawns] = (GameObject) Instantiate(myAllies[numberOfSpawns], spawnPosition,  Quaternion.identity);
		NetworkServer.Spawn(myAllies[numberOfSpawns]);
		numberOfSpawns +=1;
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

	public void getInstantiateSpells(Player p){
		p.spellAnimation = instantiateSpells;
	}

	public void backToMainMenu(){
		Application.LoadLevel("MenuScene");
		gM.StopHost();
	}
}
