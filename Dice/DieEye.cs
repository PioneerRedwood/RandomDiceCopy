using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEye : MonoBehaviour
{
	[SerializeField] private EyeBullet bullet;

	private bool initialized = false;
	private GameObject target;

	private float attackDelay = 0f;
	private Die owner;
	public int NumOfAttacks { get; private set; }

	public void InitEye(Die _owner)
	{
		owner = _owner;
		bullet.GetComponent<SpriteRenderer>().color = owner.property.Color;

		if (initialized)
		{
			return;
		}

		initialized = true;
		NumOfAttacks = 0;

	}

	public void SetTarget(GameObject _target)
	{
		target = _target;
	}

	void Update()
	{
		if (!initialized || owner.IsDragging)
		{
			return;
		}

		if (owner.property.CurrentAttackSpeed <= attackDelay)
		{
			Attack();
			attackDelay = 0f;
		}
		attackDelay += Time.deltaTime;
	}

	public void Attack()
	{
		if (target != null)
		{
			EyeBullet temp = Instantiate(bullet, transform);

			temp.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1f, 1f, 0);

			Color thisColor = owner.property.Color;
			thisColor.a = 1f;
			// µð¹ö±ë
			temp.GetComponent<SpriteRenderer>().color = thisColor;

			temp.SetTarget(target);
			temp.SetOwner(owner);

			NumOfAttacks++;
		}
	}

}
