using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
	public enum BossType
	{
		Snake = 0,
		Silence = 1,
		Knight = 2,
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/*
	 * 스네이크 패턴
	 * 정지한뒤 일반 2, 스피드 1 소환
	 * 각 체력은 남은 체력의 10%, 5%
	 */

	/*
	 * 사일런스 패턴
	 * 1.5초 정지 뒤 아군 2개 봉인
	 */

	/*
	 * 모든 아군 주사위 랜덤으로 바꿈
	 * 눈은 안 바꿈
	 */
}
