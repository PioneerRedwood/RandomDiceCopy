using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

	private void Awake()
	{
		InitializeInstance();

		// TODO: Init self canvas
		Initialize();
	}

	[SerializeField] private Canvas canvas;

	void Initialize()
	{
		canvas.enabled = false;
	}

	public void Activate()
	{
		canvas.enabled = true;
		// 플레이어들 다이스 덱 보이기
		SetDiceImages();
	}
	public void Deactivate()
	{
		canvas.enabled = false;
	}

	// TODO: 디버깅 위해 세팅
	public Image[] diceImages;
	public void SetDiceImages()
	{
		// 플레이어 다이스가 없는지 확인할 것
		List<Die> dice = GameManager.Instance.SelfPlayer.OwnedDice;
		for (int i = 0; i < dice.Count; ++i)
		{
			diceImages[i].sprite = dice[i].BodySprite;

			int index = dice[i].name.IndexOf("Die");
			string dieName = dice[i].name.Substring(0, index);

			diceImages[i].gameObject.AddComponent<ClickDelegate>();
			diceImages[i].gameObject.GetComponent<ClickDelegate>()
				.SetDelegate(("Upgrade" + dieName + "Dice"));
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	
}
