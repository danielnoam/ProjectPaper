using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private bool _initialized;
    private IAttacker _owner;
    private Vector2 _projectileMoveDirection;
    private float _projectileSpeed;
    private float _projectileDamage;
    private float _projectileForce;

    
    private bool UseWorldBounds => LevelManager.Instance;
    private Rigidbody2D _rigidbody;
    private float _projectileLifeTime = 5f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (UseWorldBounds)
        {
            if (!LevelManager.Instance.IsWithinBounds(transform.position))
            {
                Destroy(gameObject);
                return;
            }
        }
        
        _projectileLifeTime -= Time.deltaTime;
        if (_projectileLifeTime <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!_initialized) return;
        

        _rigidbody.linearVelocity = _projectileMoveDirection * _projectileSpeed;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<IAttacker>(out var attacker)) if (_owner == attacker) return;
        
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_projectileDamage);
            damageable.ApplyForce(_projectileForce, _projectileMoveDirection);
            Destroy(gameObject);
        }
    }
    
    public void Initialize(IAttacker owner, float weaponDamage, float weaponProjectileSpeed, float weaponProjectileForce, float projectileGravity)
    {
        _owner = owner;
        _projectileDamage = weaponDamage + owner.BaseDamage;
        _projectileSpeed = weaponProjectileSpeed;
        _projectileForce = weaponProjectileForce;
        _projectileMoveDirection = owner.AttackDirection.normalized;
        _rigidbody.gravityScale = projectileGravity;
        if (_projectileMoveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(_projectileMoveDirection.y, _projectileMoveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        _initialized = true;
    }
}
