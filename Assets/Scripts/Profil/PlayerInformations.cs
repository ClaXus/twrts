using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInformations {

	[SerializeField]
	private int currentPoints;
	public int Points
	{
		get { return currentPoints; }
		set { currentPoints = value; }
	}

	[SerializeField]
	private string playerPseudo; 
	public string PlayerPseudo
	{
		get { return playerPseudo; }
		set { playerPseudo = value; }
	}

	[SerializeField]
	private int currentMoney;
	public int Money
	{
		get { return currentMoney; }
		set { currentMoney = value; }
	}

	[SerializeField]
	int[] currentStyle;
	public int[] Style	{
		get { return currentStyle; }
		set { currentStyle = value; }
	}
}
