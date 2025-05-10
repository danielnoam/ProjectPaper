using UnityEngine;
using VInspector;

public enum WeaponLimiter
{
    Unlimited,
    FireRate,
    Charge,
    OneShot,
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/SOWeapon")]
public class SOWeapon : ScriptableObject
{
    
    [Header("Weapon Settings")]
    [SerializeField] private float weaponDamage = 5f;
    [SerializeField] private float projectileForce = 5f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileGravity;
    [SerializeField] private WeaponLimiter weaponLimiter = WeaponLimiter.FireRate;
    [SerializeField, ShowIf("weaponLimiter", WeaponLimiter.FireRate)] private float fireRate = 0.5f; [EndIf]
    [SerializeField, ShowIf("weaponLimiter", WeaponLimiter.Charge)] private float chargeTime = 1f; [EndIf]
    
    [Header("References")]
    [SerializeField] private Projectile projectile;
    
    
    public WeaponLimiter WeaponLimiter => weaponLimiter;
    public float FireRate => fireRate;
    public float ChargeTime => chargeTime;
    
    public void Use(IAttacker user)
    {
        if (user == null) return;
        SpawnProjectile(user);
    }
    
    private void SpawnProjectile(IAttacker user)
    {
        if (!projectile) return;
        var projectileInstance = Instantiate(projectile, user.WeaponPosition, Quaternion.identity);
        projectileInstance.Initialize(user, weaponDamage, projectileSpeed, projectileForce, projectileGravity);
    }
    
}
