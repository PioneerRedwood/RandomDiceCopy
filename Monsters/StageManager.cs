using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private static StageManager instance = null;

	private void InitInstance()
	{
		if(null == instance)
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
			if(null == instance)
			{
				return null;
			}
			return instance;
		}
	}

	// 스테이지
	[SerializeField] private Stage[] stages = null;
	private int currStageIndex = 0;

	public Stage GetCurrentStage()
	{
		return stages[currStageIndex];
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

	void Start()
	{
		Debug.Log($"Start stage #{currStageIndex}");
		StartStage();
	}

	void Update()
	{

	}

	// 스테이지 시작
	void StartStage()
	{
		Stage stage = Instantiate(stages[currStageIndex++]);

		stage.BeginStage();
	}

	// 스테이지 끝

}
