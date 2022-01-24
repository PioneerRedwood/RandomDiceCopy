using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	private float currSpeed = 0.0f;
	public float speed = 0.0f;
	// HP
	[SerializeField] private float hp;
	[SerializeField] private Text hpText;

	public float GetHP()
	{
		return hp;
	}

	public void SetHP(float HP)
	{
		hp = HP;
		hpText.text = ((int)hp).ToString();
	}
	
	private void CheckHP()
	{
		if(hp <= 0)
		{
			Destroy(gameObject);
		}
		UpdateHP();
	}

	private void UpdateHP()
	{
		hpText.text = ((int)hp).ToString();
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
	private Die damageSource;

	private float cracked = 0.0f;
	private float poisoned = 0.0f;
	public int CountCracked { get; private set; }
	public int CountPoisoned { get; private set; }
	public int CountIced { get; private set; }
	public bool HasLocked { get; private set; }

	public void OnDamage(Die from)
	{
		damageSource = from;

		switch (damageSource.property.Type)
		{
			case DieType.FireDie:
				{
					// 스플래시
					// 주변 몬스터 찾아서 데미지 입히기
					Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
					List<Monster> monsters = new List<Monster>();
					if (colliders != null)
					{
						foreach (Collider2D collider in colliders)
						{
							if (collider.gameObject.CompareTag("Enemy"))
							{
								monsters.Add(collider.gameObject.GetComponent<Monster>());
							}
						}
					}

					if (monsters.Count > 0)
					{
						foreach (Monster monster in monsters)
						{
							monster.GetDamaged(damageSource.property.SplashDamage);
						}
					}

					GetDamaged(damageSource.property.CurrentAttackDamage);

					break;
				}
			case DieType.CrackDie:
				{
					if(CountCracked < 3)
					{
						// 추가 피해 데미지
						CountCracked++;
						GetCracked(damageSource);
					}

					GetDamaged(damageSource.property.CurrentAttackDamage);
					break;
				}
			case DieType.PoisonDie:
				{
					// 데미지 오버 타임: 도트
					if(CountPoisoned < 3)
					{
						CountPoisoned++;

						if (!IsInvoking(nameof(GetPoisoned)))
						{
							InvokeRepeating(nameof(GetPoisoned), 0.0f, 1.0f);
						}

						poisoned += damageSource.property.PoisonDamagePerSecond;
					}

					GetDamaged(damageSource.property.CurrentAttackDamage);
					break;
				}
			case DieType.IceDie:
				{
					if(CountIced < 3)
					{
						// 이속 감소
						CountIced++;

						GetIced(damageSource);
					}

					GetDamaged(damageSource.property.CurrentAttackDamage);
					break;
				}
			case DieType.IronDie:
				{
					// 보스 공격 2배
					if(monsterType == MonsterType.Boss)
					{
						GetDamaged(damageSource.property.CurrentAttackDamage * 2);
					}
					else
					{
						GetDamaged(damageSource.property.CurrentAttackDamage);
					}

					break;
				}
			case DieType.GambleDie:
				{
					// 기존 ~ 2배 사이 데미지 
					GetDamaged(Random.Range(damageSource.property.CurrentAttackDamage, damageSource.property.CurrentAttackDamage * 2f));

					break;
				}
			case DieType.LockDie:
				{
					// 타겟 찾아서 
					if (!HasLocked)
					{
						// 4% 확률
						if (Random.Range(1, 100) <= damageSource.property.LockingPercentage)
						{
							// 잠김 코루틴 시작
							StartCoroutine(Lock(damageSource.property.LockingTime));
							HasLocked = true;
						}
					}

					GetDamaged(damageSource.property.CurrentAttackDamage);

					break;
				}
			default:
				{
					GetDamaged(damageSource.property.CurrentAttackDamage);
					break;
				}
		}
	}

	private void GetDamaged(float damage)
	{
		hp -= (damage * (1 + cracked));
		CheckHP();
	}

	private IEnumerator Lock(int during)
	{
		float tempSpeed = currSpeed;

		currSpeed = 0;
		yield return new WaitForSeconds(during);
		currSpeed = tempSpeed;
	}

	private void GetCracked(Die damageSource)
	{
		// 현재 균열된 수치만큼 추가 데미지
		cracked += damageSource.property.CrackingAdditionDamage;
	}
	
	private void GetPoisoned()
	{
		// 독 포이즌 시작
		hp -= poisoned;
		CheckHP();
	}

	private void GetIced(Die damageSource)
	{
		currSpeed *= (1 - damageSource.property.DecrementSpeed);
	}

	#endregion
}
