using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
	public Color color;
	public DieLevel Level;
	public Sprite BodySprite;
	public DieType dieType;

	public bool Initialized { get; protected set; }

	public Vector2 OriginCoordinate { get; protected set; }

	public void SetOrigin(Vector2 origin)
	{

		OriginCoordinate = origin;
		transform.localPosition = OriginCoordinate;
	}

	public void OnAttached(Die src, DieLevel level)
	{
		Initialized = true;
		dieType = src.dieType;

		color = src.color;
		Initialize(level, color);

		BodySprite = src.BodySprite;
		GetComponentInChildren<SpriteRenderer>().sprite = BodySprite;
	}

	public void OnDetached()
	{
		Initialized = false;
		dieType = DieType.EmptyDie;

		transform.localPosition = OriginCoordinate;

		color = Color.clear;
		Initialize(DieLevel.None, color);

		BodySprite = null;
		GetComponentInChildren<SpriteRenderer>().sprite = null;
	}

	void Update()
	{
		if (Initialized)
		{
			CheckDragging();
		}
	}

	#region Drag
	private bool isDragging = false;

	private void CheckDragging()
	{
		if (isDragging)
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
		isDragging = true;

	}

	private void OnMouseUp()
	{
		//Debug.Log(name + " " + "OnMouseUp()");
		isDragging = false;

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
		if(src.dieType == dst.dieType && src.Level == dst.Level)
		{
			return true;
		}
		return false;
	}

	[SerializeField] GameObject[] eyes;

	public void LevelUp()
	{
		if (Level != DieLevel.Seven)
		{
			Level++;
			ChangeEyesVisiblity();
		}
	}

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
					eyes[0].gameObject.SetActive(true);
					break;
				}
			case DieLevel.Two:
				{
					// 1, 2
					DeactivateAll();
					eyes[1].gameObject.SetActive(true);
					eyes[2].gameObject.SetActive(true);
					break;
				}
			case DieLevel.Three:
				{
					// 0, 1, 2
					// 더하는 거
					DeactivateAll();
					eyes[0].gameObject.SetActive(true);
					eyes[1].gameObject.SetActive(true);
					eyes[2].gameObject.SetActive(true);

					break;
				}
			case DieLevel.Four:
				{
					// 1, 2, 3, 5
					// 빼는 거
					ActivateAll();
					eyes[0].gameObject.SetActive(false);
					eyes[4].gameObject.SetActive(false);
					eyes[6].gameObject.SetActive(false);
					break;
				}
			case DieLevel.Five:
				{
					// 0, 1, 2, 3, 5
					ActivateAll();
					// 빼는거
					eyes[4].gameObject.SetActive(false);
					eyes[6].gameObject.SetActive(false);

					break;
				}
			case DieLevel.Six:
				{
					ActivateAll();
					eyes[0].gameObject.SetActive(false);
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
			eyes[i].gameObject.SetActive(true);
		}
	}

	private void DeactivateAll()
	{
		for (int i = 0; i < eyes.Length; ++i)
		{
			eyes[i].gameObject.SetActive(false);
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

	public void Initialize(DieLevel dieLevel, Color color)
	{
		Level = dieLevel;
		ChangeEyesVisiblity();

		ChangeEyeColor(color);

		if(dieType == DieType.LightDie)
		{
			// 사방 다이스 찾아서 공격속도 증가 걸어주기

			return;
		}

		if (Level != DieLevel.None && !IsInvoking("UpdateTarget"))
		{
			InvokeRepeating("UpdateTarget", 0.0f, 0.25f);
		}
	}

	// 디버깅 위해 타겟을 잠시 보이게 함
	[SerializeField] GameObject target;
	[SerializeField] float detectionRadius;

	private void UpdateTarget()
	{
		// 타겟 찾기
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
		List<Monster> enemies = new List<Monster>();
		if (colliders != null)
		{
			foreach (Collider2D collider in colliders)
			{
				if (collider.gameObject.CompareTag("Enemy"))
				{
					enemies.Add(collider.gameObject.GetComponent<Monster>());
				}
			}
		}

		switch (dieType)
		{
			case DieType.CrackDie:
				{
					// 추가 피해 데미지
					Monster temp = enemies[0];
					for(int index = 1; index < enemies.Count; ++index)
					{
						if(temp.CountCracked > enemies[index].CountCracked)
						{
							temp = enemies[index];
						}

						if(temp.CountCracked == 0)
						{
							break;
						}
					}
					SetTarget(temp.gameObject);

					break;
				}
			case DieType.PoisonDie:
				{
					// 데미지 오버 타임: DOT
					Monster temp = enemies[0];
					for (int index = 1; index < enemies.Count; ++index)
					{
						if (temp.CountPoisoned > enemies[index].CountPoisoned)
						{
							temp = enemies[index];
						}

						if (temp.CountPoisoned == 0)
						{
							break;
						}
					}
					SetTarget(temp.gameObject);

					break;
				}
			case DieType.IceDie:
				{
					// 이속 감소
					Monster temp = enemies[0];
					for (int index = 1; index < enemies.Count; ++index)
					{
						if (temp.CountIced > enemies[index].CountIced)
						{
							temp = enemies[index];
						}

						if (temp.CountIced == 0)
						{
							break;
						}
					}
					SetTarget(temp.gameObject);

					break;
				}
			case DieType.IronDie:
				{
					// 보스 공격 2배
					// 체력 많은 몬스터 우선
					Monster temp = enemies[0];
					for (int index = 1; index < enemies.Count; ++index)
					{
						if (temp.monsterType == Monster.MonsterType.Boss)
						{
							temp = enemies[index];
						}
						else
						{
							if(temp.GetHP() < enemies[index].GetHP())
							{
								temp = enemies[index];
							}
						}
					}
					SetTarget(temp.gameObject);

					break;
				}
			default:
				{
					SetTarget(enemies[0].gameObject);
					break;
				}
		}
	}

	protected void SetTarget(GameObject @object)
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
}
