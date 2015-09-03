using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : NetworkManager {
	[SerializeField]
	public Transform[] spawnPoints;

	public NetworkConnection serverConnection;

	public List<NetworkIdentity> observablesGameObjects; 

	public short connectionNumber=0;
	private Vector3 spawnPosition;

	public enum GameState{
		PlacementState,
		ActionState,
		PauseState
	}
	
	private int currentState;
	
	int TimeToPlace;

	private Player currentPlayer;

	// Use this for initialization
	void Start () {
		Debug.LogWarning("Start GM");
		currentState = (int) GameState.PlacementState;
		List<GameObject> InGameEnnemies = new List<GameObject> ();
	}	

	void Update(){


	}

	public void AddPlayerForAGame(Vector3 spawningPosition){
		this.spawnPosition = spawningPosition;
		ClientScene.AddPlayer (serverConnection, connectionNumber);
	}

	public void AddObservables(NetworkIdentity nI){
		observablesGameObjects.Add (nI);
	}

	public void RefreshObservables(){
		foreach(NetworkIdentity nI in observablesGameObjects){
			nI.RebuildObservers(true);
		}
	
	}

	public override void OnStartServer() {
		Debug.LogWarning("Server started");
			
	}

	public override void OnStartClient (NetworkClient client)
	{
		Debug.LogWarning ("START CLIENT");

	}

	public override void OnClientConnect(NetworkConnection conn) {
		Debug.LogWarning ("OnClientConnect");
		serverConnection = conn;
		//ClientScene.AddPlayer (serverConnection, connectionNumber);
	}



	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){
		Debug.LogWarning("Add Player");
		GameObject player = (GameObject)Instantiate(playerPrefab, spawnPosition/*spawnPoints[connectionNumber].position*/, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		RefreshObservables()
		connectionNumber += 1;
	}

	public short getConnectionNumber(){
		return connectionNumber;
	}
}
