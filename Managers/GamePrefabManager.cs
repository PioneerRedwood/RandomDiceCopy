using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Prefab Manager", menuName = "Singleton/Game Prefab Manager")]
class GamePrefabManager : SingletonGenericObject<GamePrefabManager>
{
	public Die EmptyDie;

	// TODO: 디버깅을 위해
	public Die RedDie;

	public List<Die> Dice;
}