using System;
using KBCore.Refs;
using UnityEngine;
using VInspector;

public class Dummy : MonoBehaviour, IDamageable
{
    
    [Header("Movement")]
    [SerializeField] private float friction = 5f;
    
    [Header("References")]
    [SerializeField, Self(Flag.Editable)] private Rigidbody2D rigidBody2D;
    


    
    public float BaseHealth { get; set; } = 100;
    public float Health { get; set; }
    private bool UseWorldBounds => LevelManager.Instance;
    private Vector2 _currentVelocity;


    
    
    private void Awake()
    {
        Health = BaseHealth;
    }


    private void FixedUpdate()
    {
        UpdateMovement();
    }


    private void UpdateMovement()
    {
        if (UseWorldBounds)
        {
            if (!LevelManager.Instance.IsWithinBounds(transform.position))
            {
                Destroy(gameObject);
                return;
            }
        }
        
        if (rigidBody2D)
        {
            _currentVelocity = rigidBody2D.linearVelocity;
            _currentVelocity.x -= _currentVelocity.x * friction * Time.fixedDeltaTime;
            _currentVelocity.y -= _currentVelocity.y * friction * Time.fixedDeltaTime;
            rigidBody2D.linearVelocity = _currentVelocity;
        }
        
    }
    
    
    
    public void TakeDamage(float damage)
    {
        Health -= damage;
        
        if (IsDead())
        {
            Destroy(gameObject);
        }
    }

    public void ApplyForce(float force, Vector2 direction)
    {
        if (!rigidBody2D) return;
        rigidBody2D.AddForce(direction * force, ForceMode2D.Impulse);

    }

    [Button]
    public void Heal(float amount)
    {
        if (Health >= BaseHealth) return;
        
        Health += amount;
        
        if (Health > BaseHealth)
        {
            Health = BaseHealth;
        }
    }

    public bool IsDead()
    {
        return Health <= 0;
    }
}
