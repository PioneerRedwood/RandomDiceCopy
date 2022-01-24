using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
	// 나올 몬스터 종류, 숫자
	[Serializable]
	public struct RoundWave
	{
		public Monster monster;
		public int numOfMonster;
	}

	[SerializeField] private RoundWave[] waves;

	public RoundWave GetWaves(int index)
	{
		return waves[index];
	}

	[SerializeField] private float nextWaveDelay = 30.0f;

	[SerializeField] private float firstSpawnDelay = 0.0f;
	[SerializeField] private float monsterSpawnDelay = 0.0f;
	[SerializeField] private float increasingHP;

	public bool StageCleared { get; private set; }

	private int waveIdx = 0;
	private List<Monster> monsters = new List<Monster>();

	public void BeginStage()
	{
		InvokeRepeating(nameof(StartWave), 0.0f, nextWaveDelay);
	}

	void StartWave()
	{
		if (waves.Length > waveIdx)
		{
			StartCoroutine(SpawnMonster(waveIdx));

			waveIdx++;
		}
		else
		{
			StageManager.Instance.LoadStage(StageManager.Instance.CurrStageIndex + 1);
		}
	}

	private IEnumerator SpawnMonster(int idx)
	{
		yield return new WaitForSeconds(firstSpawnDelay);

		for (int i = 0; i < waves[idx].numOfMonster; ++i)
		{
			Vector2 pos = StageManager.Instance.GetWaypoints()[0];

			Monster monster =
				Instantiate(waves[idx].monster, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

			int waveOffset = waveIdx == 0 ? 1 : waveIdx + 1;

			monster.SetHP(monster.GetHP() + (increasingHP * waveOffset));

			if (monster != null)
			{
				monster.transform.SetParent(gameObject.transform);

				monster.InitWaypoint();
				monsters.Add(monster);
			}
			yield return new WaitForSeconds(monsterSpawnDelay);
		}

		Debug.Log("wave over");
		GameManager.Instance.SelfPlayer.AddSP(150);

	}
}
