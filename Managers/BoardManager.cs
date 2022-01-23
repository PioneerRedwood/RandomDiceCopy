using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	[SerializeField]
	private Board board;

	DicePlayer dicePlayer = null;

	public void Initialize(DicePlayer player)
	{
		dicePlayer = player;

		board.InitializeBoard(player);
	}

	// 
	public void CreateDie()
	{
		board.GenerateRandomDie();
	}

	public bool Merge(Die src, Vector2 pos)
	{
		// TODO: 다이스 합성 후 랜덤한 곳에 생성
		bool canMerge = false;

		// 해당 위치가 유효한지 체크
		Die dst = board.CheckValidationOnBoard(src, pos);

		if (dst == null)
		{
			return canMerge;
		}

		// 합성하기 위해서는 종류와 레벨이 같아야 한다
		if (Die.Equals(src, dst))
		{
			canMerge = true;

			// 성공하면 
			src.OnDetached();

			board.OnMerged(dst);
		}

		return canMerge;
	}

	// TODO: Upgrade dice

	// 
}
