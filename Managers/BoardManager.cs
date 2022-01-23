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
		// TODO: ���̽� �ռ� �� ������ ���� ����
		bool canMerge = false;

		// �ش� ��ġ�� ��ȿ���� üũ
		Die dst = board.CheckValidationOnBoard(src, pos);

		if (dst == null)
		{
			return canMerge;
		}

		// �ռ��ϱ� ���ؼ��� ������ ������ ���ƾ� �Ѵ�
		if (Die.Equals(src, dst))
		{
			canMerge = true;

			// �����ϸ� 
			src.OnDetached();

			board.OnMerged(dst);
		}

		return canMerge;
	}

	// TODO: Upgrade dice

	// 
}
