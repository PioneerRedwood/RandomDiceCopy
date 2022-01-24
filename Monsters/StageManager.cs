using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	#region Singleton
	private static StageManager instance = null;

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

	public static StageManager Instance
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

	// 스테이지
	[SerializeField] private Stage[] stages = null;
	public int CurrStageIndex { get; private set; }

	public Stage GetCurrentStage()
	{
		return stages[CurrStageIndex];
	}

	[SerializeField] private Vector2[] waypoints = null;

	public Vector2[] GetWaypoints()
	{
		return waypoints;
	}

	private void Awake()
	{
		InitInstance();
	}

	public void LoadStage(int index)
	{
		if (stages.Length > index)
		{
			CurrStageIndex = index;

			Stage stage = Instantiate(stages[index]);
			stage.transform.SetParent(gameObject.transform);

			stage.BeginStage();
		}
		else
		{
			// 모든 스테이지 끝

		}
	}

}
