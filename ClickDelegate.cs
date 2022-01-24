using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDelegate : MonoBehaviour
{
	[Serializable]
	public enum ClickDelegateEnum
	{
		None,

		CreateDice,

		UpgradeEmptyDice,
		UpgradeFireDice,
		UpgradeCrackDice,
		UpgradeWindDice,
		UpgradePoisonDice,
		UpgradeIceDice,
		UpgradeIronDice,
		UpgradeMineDice,
		UpgradeGambleDice,
		UpgradeLockDice,
		UpgradeLightDice,
		UpgradeTestDice,

	}

	[SerializeField]
	ClickDelegateEnum clickEvent = ClickDelegateEnum.None;

	private string FilteringDelegateName(string src, string start, string end)
	{
		// TODO: µð¹ö±ë
		string result = "";
		if (src.Contains(start) && src.Contains(end))
		{
			int startIndex = src.IndexOf(start, 0) + start.Length;
			int endIndex = src.IndexOf(end);
			result = src.Substring(startIndex, endIndex - startIndex);

			return result;
		}

		return result;
	}

	public void SetDelegate(ClickDelegateEnum clickDelegate)
	{

	}

	public void SetDelegate(string name)
	{
		clickEvent = (ClickDelegateEnum)Enum.Parse(typeof(ClickDelegateEnum), name);
	}

	private void OnMouseDown()
	{
		switch (clickEvent)
		{
			case ClickDelegateEnum.CreateDice:
				{
					// SP ¼Ò¸ð
					if (GameManager.Instance.SelfPlayer.BuyDie())
					{
						GameManager.Instance.SelfPlayer.boardManager.CreateDie();
					}

					break;
				}
			case ClickDelegateEnum.UpgradeFireDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Fire dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeCrackDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Crack dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeWindDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Wind dice!");
					break;
				}
			case ClickDelegateEnum.UpgradePoisonDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Poison dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeIceDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Ice dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeIronDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Iron dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeMineDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Mine dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeGambleDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Gamble dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeLockDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Lock dice!");
					break;
				}
			case ClickDelegateEnum.UpgradeLightDice:
				{
					//GameManager.Instance.SelfPlayer.Upgrade
					Debug.Log("Upgrade Light dice!");
					break;
				}

			case ClickDelegateEnum.None:
				{
					break;
				}
			default:
				{
					break;
				}
		}
		
	}
}
