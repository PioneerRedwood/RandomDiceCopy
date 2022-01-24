using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
	public DieLevel Level;
	public BaseDieProperty property;
	public SpriteRenderer BodySpriteRenderer;

	public bool Initialized { get; protected set; }

	public Vector2 OriginCoordinate { get; protected set; }

	protected void Initialize(DieLevel dieLevel)
	{
		Level = dieLevel;
		ChangeEyesVisiblity();

		ChangeEyeColor(property.Color);

		if (Level == DieLevel.None)
		{
			isBuffed = false;
			buffSource = null;

			return;
		}

		switch (property.Type)
		{
			case DieType.LightDie:
				{
					InvokeRepeating(nameof(UpdateBuffTarget), 0.0f, 0.5f);
					break;
				}
			case DieType.MineDie:
				{
					// 10�ʴ� ���� ���
					// ��������� ������ 1�ʴ�
					InvokeRepeating(nameof(MiningSP), 0.0f, 1.0f);

					break;
				}
			default:
				{
					InvokeRepeating(nameof(UpdateAttackTarget), 0.0f, 0.25f);
					break;
				}
		}
	}
	#region Die Component system
	public void SetOrigin(Vector2 origin)
	{
		OriginCoordinate = origin;
		transform.localPosition = OriginCoordinate;
	}

	public void OnAttached(Die src, DieLevel level)
	{
		Initialized = true;
		InitProperty(src.property);

		BodySpriteRenderer.sprite = src.BodySpriteRenderer.sprite;

		Initialize(level);
	}

	public void OnDetached()
	{
		Initialized = false;

		transform.localPosition = OriginCoordinate;

		InitProperty(new BaseDieProperty());
		property.Color = Color.clear;

		BodySpriteRenderer.sprite = null;

		Initialize(DieLevel.None);

		// ��� �ݺ� �Լ� ȣ�� ���
		CancelInvoke();
	}

	private void InitProperty(BaseDieProperty @base)
	{
		property = @base;

		property.CurrentAttackDamage = property.AttackDamage;
		property.CurrentAttackSpeed = property.AttackSpeed;
	}
	#endregion

	void Update()
	{
		if (Initialized)
		{
			CheckDragging();
		}
	}

	#region Drag
	public bool IsDragging { get; private set; }

	private void CheckDragging()
	{
		if (IsDragging)
		{
			Vector2 mousePos =
				Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			transform.Translate(mousePos);
			//Debug.Log(name + " " + transform.position);
		}
	}

	private void OnMouseDown()
	{
		//Debug.Log(name + " " + "OnMouseDown()");
		IsDragging = true;

	}

	private void OnMouseUp()
	{
		//Debug.Log(name + " " + "OnMouseUp()");
		IsDragging = false;

		TryMerge();
	}
	#endregion

	#region Merge
	private void TryMerge()
	{
		if (!GameManager.Instance.SelfPlayer.boardManager.Merge(this, transform.position))
		{
			transform.localPosition = new Vector3(OriginCoordinate.x, OriginCoordinate.y, 0);
		}
	}
	#endregion

	public static bool Equals(Die src, Die dst)
	{
		if (src.property.Type == dst.property.Type && src.Level == dst.Level)
		{
			return true;
		}
		return false;
	}

	public void LevelUp()
	{
		if (Level != DieLevel.Seven && Level != DieLevel.None)
		{
			Level++;
			ChangeEyesVisiblity();

		}
	}

	public void IncrementStaticDamage()
	{
		// ���׷��̵��� �� ���
	}

	#region Eyes visibilty
	[SerializeField] GameObject[] eyes;

	private void ChangeEyesVisiblity()
	{
		switch (Level)
		{
			case DieLevel.None:
				{
					DeactivateAll();
					break;
				}
			case DieLevel.One:
				{
					DeactivateAll();
					eyes[0].SetActive(true);
					break;
				}
			case DieLevel.Two:
				{
					// 1, 2
					DeactivateAll();
					eyes[1].SetActive(true);
					eyes[2].SetActive(true);
					break;
				}
			case DieLevel.Three:
				{
					// 0, 1, 2
					// ���ϴ� ��
					DeactivateAll();
					eyes[0].SetActive(true);
					eyes[1].SetActive(true);
					eyes[2].SetActive(true);

					break;
				}
			case DieLevel.Four:
				{
					// 1, 2, 3, 5
					// ���� ��
					ActivateAll();
					eyes[0].SetActive(false);
					eyes[4].SetActive(false);
					eyes[6].SetActive(false);
					break;
				}
			case DieLevel.Five:
				{
					// 0, 1, 2, 3, 5
					ActivateAll();
					// ���°�
					eyes[4].SetActive(false);
					eyes[6].SetActive(false);

					break;
				}
			case DieLevel.Six:
				{
					ActivateAll();
					eyes[0].SetActive(false);
					break;
				}
			case DieLevel.Seven:
				{
					ActivateAll();
					break;
				}
			default:
				{
					DeactivateAll();
					break;
				}
		}
	}

	private void ActivateAll()
	{
		for (int i = 0; i < eyes.Length; ++i)
		{
			eyes[i].SetActive(true);
		}
	}

	private void DeactivateAll()
	{
		for (int i = 0; i < eyes.Length; ++i)
		{
			eyes[i].SetActive(false);
		}
	}

	private void ChangeEyeColor(Color color)
	{
		if (Level == DieLevel.None)
		{
			color.a = 0f;
		}
		else
		{
			color.a = 1f;
		}

		for (int i = 0; i < eyes.Length; ++i)
		{
			eyes[i].GetComponent<SpriteRenderer>().color = color;

			DieEye bullet = eyes[i].GetComponent<DieEye>();
			if (bullet != null)
			{
				bullet.InitEye(this);
			}
		}
	}

	#endregion

	#region Targeting system
	// ����� ���� Ÿ���� ��� �ν����Ϳ��� ���̰� ��
	public GameObject target;

	public List<Monster> FindMonstersInRadius(float radius)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
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

		return monsters;
	}

	private void UpdateAttackTarget()
	{
		List<Monster> monsters = FindMonstersInRadius(property.AttackAreaRadius);

		// Ÿ���� �ϳ��̸� �ٷ� ����
		if (monsters.Count > 1)
		{
			// �ΰ��� ���� ����

			switch (property.Type)
			{
				case DieType.CrackDie:
					{
						// �߰� ���� ������
						Monster temp = monsters[0];
						for (int index = 1; index < monsters.Count; ++index)
						{
							// �߰� ���� ���� �ʾҴ� �� �켱
							if (temp.CountCracked > monsters[index].CountCracked)
							{
								temp = monsters[index];
							}

							if (temp.CountCracked == 0)
							{
								break;
							}
						}
						SetAttackTarget(temp.gameObject);

						break;
					}
				case DieType.PoisonDie:
					{
						// DOT; Damage Over Time
						Monster temp = monsters[0];
						for (int index = 1; index < monsters.Count; ++index)
						{
							// �� ���� ���� �ʾҴ� �� �켱
							if (temp.CountPoisoned > monsters[index].CountPoisoned)
							{
								temp = monsters[index];
							}

							if (temp.CountPoisoned == 0)
							{
								break;
							}
						}
						SetAttackTarget(temp.gameObject);

						break;
					}
				case DieType.IceDie:
					{
						// �̼� ����
						Monster temp = monsters[0];
						for (int index = 1; index < monsters.Count; ++index)
						{
							// �̼� ���� �ȵƴ� �� �켱
							if (temp.CountIced > monsters[index].CountIced)
							{
								temp = monsters[index];
							}

							if (temp.CountIced == 0)
							{
								break;
							}
						}
						SetAttackTarget(temp.gameObject);

						break;
					}
				case DieType.IronDie:
					{
						// ���� ���� 2��
						// �켱 ����; ���� > ü�� ���� ����
						Monster temp = monsters[0];
						for (int index = 1; index < monsters.Count; ++index)
						{
							if (temp.monsterType == Monster.MonsterType.Boss)
							{
								temp = monsters[index];
							}
							else
							{
								if (temp.GetHP() < monsters[index].GetHP())
								{
									temp = monsters[index];
								}
							}
						}
						SetAttackTarget(temp.gameObject);

						break;
					}
				default:
					{
						SetAttackTarget(monsters[0].gameObject);
						break;
					}
			}
		}
		else if (monsters.Count == 1)
		{
			SetAttackTarget(monsters[0].gameObject);
		}
		else
		{
			SetAttackTarget(null);
			return;
		}

		if(isBuffed)
		{
			if (buffSource == null)
			{
				isBuffed = false;
				property.CurrentAttackSpeed = property.AttackSpeed;
			}
		}
	}

	private void SetAttackTarget(GameObject @object)
	{
		if (target != null && target == @object)
		{
			return;
		}

		target = @object;

		for (int i = 0; i < eyes.Length; ++i)
		{
			DieEye bullet = eyes[i].GetComponent<DieEye>();
			if (bullet != null)
			{
				bullet.InitEye(this);
				bullet.SetTarget(@object);
			}
		}
	}

	// ���� �޴� �� �ߺ� �ȵ�
	bool isBuffed = false;
	Die buffSource = null;

	private void UpdateBuffTarget()
	{
		// ���忡 �ִ� �ڽ� ��ǥ X +1/-1�� ��ǥ Y +1/-1�� Ÿ������ �����ϰ� ���ݼӵ��� ����Ѵ�
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
		if (colliders != null)
		{
			foreach (Collider2D collider in colliders)
			{
				if (collider.gameObject.CompareTag("Die")
					&&
					collider.gameObject != gameObject
					&&
					Vector2.Distance(transform.position, collider.gameObject.transform.position) <= 0.75f)
				{
					Die target = collider.gameObject.GetComponent<Die>();

					target.IncreaseAttackSpeed(this, property.IncrementAttackSpeed);
				}
			}
		}
	}
	
	private void IncreaseAttackSpeed(Die from, float up)
	{
		isBuffed = true;

		buffSource = from;

		property.CurrentAttackSpeed = property.AttackSpeed + up;
	}


	private void MiningSP()
	{
		GameManager.Instance.SelfPlayer.AddSP(property.EarningOutput);
	}
	#endregion
}
