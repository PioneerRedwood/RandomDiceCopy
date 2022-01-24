using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Singleton
	private static UIManager instance = null;

	private void InitializeInstance()
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

	public static UIManager Instance
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
	#endregion

	private void Awake()
	{
		InitializeInstance();

		// TODO: Init self canvas
		Initialize();
	}

	[SerializeField] private Canvas canvas;

	void Initialize()
	{
		// 디버깅
		canvas.gameObject.SetActive(false);
	}

	public void Activate()
	{
		canvas.gameObject.SetActive(true);

		SetDiceImages();

		StageManager.Instance.LoadStage(0);
	}
	public void Deactivate()
	{
		canvas.gameObject.SetActive(false);
	}

	public Image[] diceImages;
	public void SetDiceImages()
	{
		// 플레이어 다이스가 없는지 확인할 것
		List<Die> dice = GameManager.Instance.SelfPlayer.OwnedDice;

		for (int i = 0; i < diceImages.Length; ++i)
		{
			diceImages[i].sprite = dice[i].BodySpriteRenderer.sprite;

			int index = dice[i].name.IndexOf("Die");
			string dieName = dice[i].name.Substring(0, index);

			diceImages[i].gameObject.AddComponent<ClickDelegate>();
			diceImages[i].gameObject.GetComponent<ClickDelegate>()
				.SetDelegate(("Upgrade" + dieName + "Dice"));
		}
	}
	
}
