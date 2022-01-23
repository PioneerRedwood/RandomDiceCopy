using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
	private static DiceManager instance = null;
	private void InitInstance()
	{
		if (null == instance)
		{
			instance = this;

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public static DiceManager Instance
	{
		get
		{
			if (null == instance)
			{
				return null;
			}
			return instance;
		}
	}

	private void Start()
	{
		Debug.Log("DiceManager start");
		InitInstance();

	}

	[SerializeField]
	public List<Die> Dice;
}
