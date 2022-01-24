using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DieType
{
	EmptyDie = 0,
	FireDie = 1,
	CrackDie = 2,
	WindDie = 3,
	PoisonDie = 4,
	IceDie = 5,
	IronDie = 6,
	MineDie = 7,
	GambleDie = 8,
	LockDie = 9,
	LightDie = 10,
	TestDie = 11,
}

public enum DieLevel
{
	None,
	One,
	Two,
	Three,
	Four,
	Five,
	Six,
	Seven
}

// 수정 필요

[Serializable]
public class BaseDieProperty
{
	[Header("Basic")]
	public DieType Type;
	public Color Color;

	[Header("Attack")]
	public float AttackDamage;
	public float CurrentAttackDamage;
	public float DamageUp;

	public float AttackSpeed;
	public float CurrentAttackSpeed;

	public float AttackAreaRadius;

	[Header("Fire")]
	public float SplashDamage;
	public float SplashDamageUp;

	[Header("Crack")]
	[Range(0, 1f)]
	public float CrackingAdditionDamage;
	public float CrackingAdditionDamageUp;

	[Header("Poison")]
	public float PoisonDamagePerSecond;
	public float PoisonDamagePerSecondUp;

	[Header("Ice")]
	[Range(0, 1f)]
	public float DecrementSpeed;
	public float DecrementSpeedUp;

	[Header("Light")]
	[Range(0, 1f)]
	public float IncrementAttackSpeed;
	public float IncrementAttackSpeedUp;

	[Header("Lock")]
	[Range(0, 100f)]
	public float LockingPercentage;
	public float LockingPercentageUp;
	public int LockingTime;
	public int LockingTimeUp;

	[Header("Mine")]
	public int EarningOutput;
	public int EarningOutputUp;
}
