using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBullet : MonoBehaviour
{
	// 총알 속도
	[SerializeField] private float speed = 10.0f;

	private GameObject target;
	private Die owner;

	public void SetTarget(GameObject _target)
	{
		target = _target;
	}

	public void SetOwner(Die _owner)
	{
		owner = _owner;
	}

	void Update()
	{
		if (target != null)
		{
			transform.position =
				Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

			float distance = Vector2.Distance(transform.position, target.transform.position);

			if (distance <= 0.05f)
			{
				DoDamage();

				Debug.Log("Damage");
				Destroy(gameObject);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	private void DoDamage()
	{
		// 크리티컬 계산

		//target.GetComponent<Monster>().OnDamage(damage, type);
	}
}
