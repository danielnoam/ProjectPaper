using UnityEngine;

public interface IDamageable
{
        float BaseHealth { get; set; }
        float Health { get; set; }
        void TakeDamage(float damage);
        void ApplyForce(float force, Vector2 direction);
        void Heal(float amount);
        bool IsDead();

}