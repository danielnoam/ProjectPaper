


using System;
using UnityEngine;

public interface IAttacker
{
    SOWeapon CurrentWeapon { get; set; }
    float BaseDamage { get; set; }
    Transform ProjectileSpawnPositon { get; set; }
    Vector2 AttackDirection { get; set; }
    void UseWeapon();
    void ChangeWeapon(SOWeapon weapon);


    
        
}