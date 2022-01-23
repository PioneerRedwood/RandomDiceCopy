using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	public enum MonsterType
	{
		Normal = 0,
		Speeder = 1,
		Bigger = 2,
		Boss = 3
	}
	public MonsterType monsterType = MonsterType.Normal;

	// 기본 속성
	// 이동 속도
	private Vector2[] waypoints;
	private Vector2 nextPoint;
	private int waypointIdx = 0;

	protected float currSpeed = 0.0f;
	public float speed = 0.0f;
	// HP
	[SerializeField] private float hp;
	public float GetHP()
	{
		return hp;
	}
	
	public void InitWaypoint()
	{
		waypoints = StageManager.Instance.GetWaypoints();
		nextPoint = waypoints[waypointIdx + 1];
	}

	protected void MoveToNext()
	{
		transform.position = Vector2.MoveTowards(transform.position, nextPoint, currSpeed * Time.deltaTime);

		if ((Vector2)transform.position == nextPoint && nextPoint != waypoints[waypoints.Length - 1])
		{
			nextPoint = waypoints[++waypointIdx];
		}

		if ((Vector2)transform.position == waypoints[waypoints.Length - 1])
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		// 디버깅
		currSpeed = speed;
	}

	void Update()
	{
		// 디버깅
		MoveToNext();
	}

	#region Damage related

	public int CountCracked { get; private set; }
	public int CountPoisoned { get; private set; }
	public int CountIced { get; private set; }
	public bool HasLocked { get; private set; }

	public void OnDamage(float damage, DieType dieType)
	{
		switch (dieType)
		{
			case DieType.FireDie:
				{
					// 스플래시
					// 주변 몬스터 찾아서 데미지 입히기
					break;
				}
			case DieType.CrackDie:
				{
					// 추가 피해 데미지
					CountCracked--;

					break;
				}
			case DieType.PoisonDie:
				{
					// 데미지 오버 타임: 도트

					break;
				}
			case DieType.IceDie:
				{
					// 이속 감소

					break;
				}
			case DieType.IronDie:
				{
					// 보스 공격 2배
					// 체력 많은 몬스터 우선

					break;
				}
			default:
				{
					break;
				}
		}
		// 데미지 입기

	}

	#endregion
}
