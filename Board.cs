using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	// 보드 간격을 수치화
	// Row가 행:Y, Col이 열:X
	static readonly float rowOffset = 0.73f;
	static readonly float colOffset = 0.75f;

	static readonly float[] RowDefinition = { 0.83f, 0.1f, -0.63f };
	static readonly float[] ColumnDefinition = { -1.5f, -0.75f, 0f, 0.75f, 1.5f };

	public Die[,] Dice;

	public DicePlayer OwnedPlayer { get; private set; }

	private void Awake()
	{
		Dice = new Die[3, 5];
	}

	#region Init board
	public void InitializeBoard(DicePlayer player)
	{
		OwnedPlayer = player;

		for (int row = 0; row < RowDefinition.Length; ++row)
		{
			for (int col = 0; col < ColumnDefinition.Length; ++col)
			{
				Die die = null;
				InstantiateDie(ref die, ColumnDefinition[col], RowDefinition[row]);
				Dice[row, col] = die;

				die.name = "[" + col.ToString() + ", " + row.ToString() + "]";
			}
		}
	}

	private void InstantiateDie(ref Die die, float xAxis, float yAxis)
	{
		die = Instantiate(GamePrefabManager.Instance.EmptyDie);

		die.transform.SetParent(gameObject.transform);

		die.SetOrigin(new Vector2(xAxis, yAxis));
	}
	#endregion

	public List<Vector2Int> AvailablePositions { get; private set; }

	private bool CheckRandomPlaceExists()
	{
		bool isExist = false;
		AvailablePositions = new List<Vector2Int>();

		for(int row = 0; row < 3; ++row)
		{
			for(int col = 0; col < 5; ++col)
			{
				if (!Dice[row, col].Initialized)
				{
					isExist = true;
					AvailablePositions.Add(new Vector2Int(row, col));
				}
			}
		}

		return isExist;
	}

	public void GenerateRandomDie()
	{
		if (!CheckRandomPlaceExists())
		{
			return;
		}

		if (AvailablePositions != null && AvailablePositions.Count > 0)
		{
			int randPosIndex = UnityEngine.Random.Range(0, AvailablePositions.Count);
			int randDieIndex = UnityEngine.Random.Range(0, OwnedPlayer.OwnedDice.Count);

			Vector2Int randomIndex = AvailablePositions[randPosIndex];
			Die randomDie = OwnedPlayer.OwnedDice[randDieIndex];

			Dice[randomIndex.x, randomIndex.y].OnAttached(randomDie, DieLevel.One);
		}
	}

	public Die CheckValidationOnBoard(Die src, Vector2 position)
	{
		Die die = null;

		// 2차 시도 범위 검사
		for (int row = 0; row < 3; ++row)
		{
			for (int col = 0; col < 5; ++col)
			{
				if (Dice[row, col].Initialized)
				{
					Vector2 diePos = Dice[row, col].transform.position;

					float leftX = diePos.x - (rowOffset / 2);
					float rightX = diePos.x + (rowOffset / 2);
					float lowY = diePos.y - (colOffset / 2);
					float highY = diePos.y + (colOffset / 2);
					
					//Debug.Log("X: " + leftX + ", " + rightX + " Y: " + lowY + ", " + highY);
					// X 범위
					if ((leftX < position.x) && (position.x < rightX)
							&& 
							(lowY < position.y) && (position.y < highY))
					{
						if(!(Dice[row, col].OriginCoordinate == src.OriginCoordinate))
						{
							die = Dice[row, col];
						}
					}
				}
			}
		}
		return die;
	}

	public void OnMerged(Die target)
	{
		for (int row = 0; row < 3; ++row)
		{
			for (int col = 0; col < 5; ++col)
			{
				Die temp = Dice[row, col];
				if (temp.Initialized)
				{
					if (temp == target)
					{
						int rand = UnityEngine.Random.Range(0, OwnedPlayer.OwnedDice.Count);
						Die die = OwnedPlayer.OwnedDice[rand];

						target.OnAttached(die, target.Level);

						target.LevelUp();

						return;
					}
				}
			}
		}
	}
}
