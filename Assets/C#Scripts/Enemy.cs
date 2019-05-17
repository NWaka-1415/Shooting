using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _attackPower = 10;

    public int AttackPower => _attackPower;
}