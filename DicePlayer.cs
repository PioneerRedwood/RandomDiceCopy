using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicePlayer : MonoBehaviour
{
	[Serializable] public enum PlayType
	{
		Self,
		AI,
		Other
	}

	public PlayType playType;

	[SerializeField] Text spText;
	[SerializeField] Text creationDieCostText;

	// SP
	public int SP
	{
		get;
		private set;
	}
	
	public void AddSP(int sp)
	{
		SP += sp;

		UpdateUI();
	}

	private void UpdateUI()
	{
		spText.text = SP.ToString();
		creationDieCostText.text = CreationDieCost.ToString();
	}

	public int CreationDieCost
	{
		get;
		private set;
	}

	public bool BuyDie()
	{
		if(SP >= CreationDieCost)
		{
			SP -= CreationDieCost;

			CreationDieCost += 10;

			UpdateUI();

			return true;
		}
		else
		{
			return false;
		}
	}

	private List<Die> dice;
	public List<Die> OwnedDice
	{
		get
		{
			return dice;
		}
	}

	public void AddDie(Die die)
	{
		dice.Add(die);
	}

	public void Initialize(PlayType type)
	{
		playType = type;

		dice = new List<Die>();
	}

	public BoardManager boardManager;

	void Start()
	{
		SP = 100;
		CreationDieCost = 10;
		
		UpdateUI();

		// TODO: 아래는 추후에 수정 필요함
		Initialize(playType);

		boardManager.Initialize(this);

		if (playType == PlayType.Self)
		{
			// 앞에 5개 가져가기
			for (int i = 0; i < GamePrefabManager.Instance.Dice.Count / 2; ++i)
			{
				dice.Add(GamePrefabManager.Instance.Dice[i]);
			}
		}
		else
		{
			// 뒤에 5개 가져가기
			for (int i = 5; i < GamePrefabManager.Instance.Dice.Count; ++i)
			{
				dice.Add(GamePrefabManager.Instance.Dice[i]);
			}
		}
	}

	// HP


}
