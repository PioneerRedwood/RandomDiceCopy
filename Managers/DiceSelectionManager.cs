using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiceSelectionManager : MonoBehaviour
{
	#region Singleton
	private static DiceSelectionManager instance = null;

	private void InitializeInstance()
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

	public static DiceSelectionManager Instance
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

	internal struct DieInfo
	{
		public int idx;
		public Sprite sprite;

		public DieInfo(int _diceIndex, Sprite _sprite)
		{
			idx = _diceIndex;
			sprite = _sprite;
		}

		public override string ToString()
		{
			return sprite.ToString();
		}
	}

	internal List<DieInfo> diceInfos = null;

	[Header("User Interface")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private Button leftDie;
	[SerializeField] private Button rightDie;

	private int order = 0;

	[SerializeField] private Image[] otherPlayerDiceSprites;
	[SerializeField] private Image[] selfPlayerDiceSprites;

	// Start is called before the first frame update
	void Start()
	{
		GameManager.Instance.SelfPlayer.Initialize(DicePlayer.PlayType.Self);
		GameManager.Instance.OtherPlayer.Initialize(DicePlayer.PlayType.AI);

		leftDie.onClick.AddListener(() => { SelectDice(false); });
		rightDie.onClick.AddListener(() => { SelectDice(true); });

		ShowDiceCouple();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void Awake()
	{
		//Debug.Log("DiceSelectionManager AWake()");
		InitializeInstance();

		Initialize();
	}

	public void Initialize()
	{
		diceInfos = new List<DieInfo>();

		for (int i = 0; i < GamePrefabManager.Instance.Dice.Count; ++i)
		{
			diceInfos.Add(new DieInfo(i, GamePrefabManager.Instance.Dice[i].BodySprite));
		}

		ShuffleDice();

		canvas.enabled = true;
		order = 0;
	}

	private void ShuffleDice()
	{
		// ref https://www.delftstack.com/ko/howto/csharp/shuffle-a-list-in-csharp/
		// shuffle
		System.Random rand = new System.Random();
		int count = diceInfos.Count;
		while (count > 1)
		{
			count--;
			int idx = rand.Next(count + 1);
			DieInfo info = diceInfos[idx];
			diceInfos[idx] = diceInfos[count];
			diceInfos[count] = info;
		}
	}

	public void ShowDiceCouple()
	{
		leftDie.GetComponent<Image>().sprite = diceInfos[order * 2].sprite;
		rightDie.GetComponent<Image>().sprite = diceInfos[(order * 2) + 1].sprite;
	}

	public void SelectDice(bool right)
	{
		DieInfo infoToSelf;
		DieInfo infoToOther;

		if (!right)
		{
			infoToSelf = diceInfos[order * 2];
			infoToOther = diceInfos[(order * 2) + 1];
		}
		else
		{
			infoToSelf = diceInfos[(order * 2) + 1];
			infoToOther = diceInfos[order * 2];
		}

		GameManager.Instance.SelfPlayer.AddDie(GamePrefabManager.Instance.Dice[infoToSelf.idx]);
		GameManager.Instance.OtherPlayer.AddDie(GamePrefabManager.Instance.Dice[infoToOther.idx]);

		selfPlayerDiceSprites[order].sprite = infoToSelf.sprite;
		otherPlayerDiceSprites[order].sprite = infoToOther.sprite;

		UpdateUI();
	}

	public void UpdateUI()
	{
		order++;
		if (order > 4)
		{
			Deactivate();
		}
		else
		{
			ShowDiceCouple();
		}
	}

	public void Activate()
	{
		canvas.enabled = true;

		Initialize();
	}

	// TODO: 10개 모두 고른 상태이므로 해당 UI는 비활성화되면서 본격적으로 게임 시작
	public void Deactivate()
	{
		canvas.enabled = false;

		// TODO: Start stage
		UIManager.Instance.Activate();
	}
}