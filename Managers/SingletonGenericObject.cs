using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public abstract class SingletonGenericObject<T> : ScriptableObject where T : ScriptableObject
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.LoadAll<T>("").FirstOrDefault();
				if (instance == null)
				{
					Debug.LogError($"Can't find Singleton ScriptableObject of type {typeof(T)}");
				}
			}

			return instance;
		}
	}
}