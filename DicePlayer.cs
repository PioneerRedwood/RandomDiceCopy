using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePlayer : MonoBehaviour
{
	[Serializable]
	public enum PlayType
	{
		Self,
		AI,
		Other
	}

	public PlayType playType;

	// TODO: ��� public���� �ٲ�
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
		// TODO: �Ʒ��� ���Ŀ� ���� �ʿ���
		Initialize(playType);

		boardManager.Initialize(this);

		if (playType == PlayType.Self)
		{
			// �տ� 5�� ��������
			for (int i = 0; i < GamePrefabManager.Instance.Dice.Count / 2; ++i)
			{
				dice.Add(GamePrefabManager.Instance.Dice[i]);
			}
		}
		else
		{
			// �ڿ� 5�� ��������
			for (int i = 5; i < GamePrefabManager.Instance.Dice.Count; ++i)
			{
				dice.Add(GamePrefabManager.Instance.Dice[i]);
			}
		}
	}
}
