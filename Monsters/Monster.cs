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

	// �⺻ �Ӽ�
	// �̵� �ӵ�
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
		// �����
		currSpeed = speed;
	}

	void Update()
	{
		// �����
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
					// ���÷���
					// �ֺ� ���� ã�Ƽ� ������ ������
					break;
				}
			case DieType.CrackDie:
				{
					// �߰� ���� ������
					CountCracked--;

					break;
				}
			case DieType.PoisonDie:
				{
					// ������ ���� Ÿ��: ��Ʈ

					break;
				}
			case DieType.IceDie:
				{
					// �̼� ����

					break;
				}
			case DieType.IronDie:
				{
					// ���� ���� 2��
					// ü�� ���� ���� �켱

					break;
				}
			default:
				{
					break;
				}
		}
		// ������ �Ա�

	}

	#endregion
}
