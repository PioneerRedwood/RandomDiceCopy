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
		// �÷��̾�� ���̽� �� ���̱�
		SetDiceImages();
	}
	public void Deactivate()
	{
		canvas.enabled = false;
	}

	// TODO: ����� ���� ����
	public Image[] diceImages;
	public void SetDiceImages()
	{
		// �÷��̾� ���̽��� ������ Ȯ���� ��
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
