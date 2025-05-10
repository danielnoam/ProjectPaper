


using System;
using UnityEngine;

public interface IAttacker
{
    SOWeapon CurrentWeapon { get; set; }
    float BaseDamage { get; set; }
    Vector2 WeaponPosition { get; set; }
    Vector2 AttackDirection { get; set; }
    void UseWeapon();
    void ChangeWeapon(SOWeapon weapon);


    
        
}