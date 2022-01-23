using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Singleton
	private static GameManager instance = null;

	private void InitializeInstance()
	{
		if (null == instance)
		{
			instance = this;

			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	public static GameManager Instance
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
	#endregion

	[SerializeField] public DicePlayer SelfPlayer;
	[SerializeField] public DicePlayer OtherPlayer;

	public bool IsDiceSelectionActivate = false;

	private void Awake()
	{
		//Debug.Log("GameManager AWake()");
		InitializeInstance();
	}
}
