﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class InformationLoader : MonoBehaviour {

	[SerializeField]
	private PlayerInformations informations;

	public PlayerInformations Informations {
		get { return informations; }
	}

	void Awake() {
		LoadPlayerInformations();
	}

	public void LoadPlayerInformations() {
		string filePath = Application.persistentDataPath + "info.bin";
		if (File.Exists(filePath)) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fS = File.OpenRead(filePath);
			
			informations = (PlayerInformations)formatter.Deserialize(fS);
			
			fS.Close();
		}
		else {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fS = File.Create(filePath);
			
			formatter.Serialize(fS, informations);
			
			fS.Close();
			
			LoadPlayerInformations();
		}
	}

	public void SavePlayerInformations(string playerPseudo, int playerMoney, int playerPoints, int[] playerStyle) {
		informations.PlayerPseudo = playerPseudo;
		informations.Money = playerMoney;
		informations.Style = playerStyle;
		informations.Points = playerPoints;
		
		string filePath = Application.persistentDataPath + "info.bin";
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream fS = File.Create(filePath);
		
		formatter.Serialize(fS, informations);
		fS.Close();
		
	}


}
