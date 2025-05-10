using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

public class PlayerController : MonoBehaviour, IDamageable, IAttacker
{
    [Header("Movement")]
    [SerializeField] private Vector2 moveSpeed = new Vector2(5, 5);
    [SerializeField] private Vector2 acceleration = new Vector2(5, 5);
    [SerializeField] private Vector2 deceleration = new Vector2(5, 5);
    

    [Header("Rotation")]
    [SerializeField] private float xRotationSpeed = 35f;
    [SerializeField] private float maxXRotationAngle = 10f;
    [SerializeField] private float zRotationSpeed = 35f;
    [SerializeField] private float maxZRotationAngle = 10f;
    
    [Header("References")]
    [SerializeField] private Transform gfx;
    [SerializeField] private Transform weaponPosition;
    [SerializeField, Self(Flag.Editable)] private Rigidbody2D rigidBody2D;
    [SerializeField] private SOWeapon defaultWeapon;
    [SerializeField] private SOWeapon eraserWeapon;
    [SerializeField] private SOWeapon tippexWeapon;
    [SerializeField] private SOWeapon waterSprayWeapon;
    [SerializeField] private SOWeapon scissorsWeapon;
    

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _attackInput;
    [SerializeField, ReadOnly] private Vector2 _moveInput;
    [SerializeField, ReadOnly] private Vector2 _moveDirection;
    [SerializeField, ReadOnly] private Vector2 _currentVelocity;
    [Space(10)]
    [SerializeField, ReadOnly] private float _lastFireTime;
    [SerializeField, ReadOnly] private float _chargeStartTime;
    [SerializeField, ReadOnly] private bool _isCharging;
    [SerializeField, ReadOnly] private bool _canFire = true;
    
    private float _startXRotation;
    private float _currentXRotation;
    private float _startZRotation;
    private float _currentZRotation;
    
    private bool UseWorldBounds => LevelManager.Instance;
    public float Health { get; set; }
    public float BaseHealth { get; set; } = 100f;
    public float BaseDamage { get; set; } = 0;
    public SOWeapon CurrentWeapon { get; set; }
    public Vector2 WeaponPosition { get; set; }
    public Vector2 AttackDirection { get; set; }
    


    
    
    
    

    private void Awake()
    {
        _startXRotation = gfx.transform.rotation.x;
        _startZRotation = gfx.transform.rotation.z;
        Health = BaseHealth;
        if (defaultWeapon) CurrentWeapon = defaultWeapon;
    }


    private void Update()
    {
        UpdateInput();
        UseWeapon();
        
        
        
        // Set weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(eraserWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(tippexWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(waterSprayWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeWeapon(scissorsWeapon);
        }
    }


    private void FixedUpdate()
    {
        UpdateMovement();
    }


    #region Movement --------------------------------------------------------------------------------
    
    private void UpdateMovement()
    {
        // Calculate the desired velocity based on input and acceleration
        Vector2 targetVelocity = _moveDirection * moveSpeed;
        float targetXRotation = _moveDirection.x * maxXRotationAngle;
        float targetZRotation = _moveDirection.y * maxZRotationAngle;
    
        // Apply acceleration or deceleration based on input
        if (_moveInput != Vector2.zero)
        {
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, acceleration.magnitude * Time.fixedDeltaTime);
            _currentXRotation = Mathf.MoveTowards(_currentXRotation, targetXRotation, xRotationSpeed * Time.fixedDeltaTime);
            _currentZRotation = Mathf.MoveTowards(_currentZRotation, targetZRotation, zRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, Vector2.zero, deceleration.magnitude * Time.fixedDeltaTime);
            _currentXRotation = Mathf.MoveTowards(_currentXRotation, _startXRotation, xRotationSpeed * Time.fixedDeltaTime);
            _currentZRotation = Mathf.MoveTowards(_currentZRotation, _startZRotation, zRotationSpeed * Time.fixedDeltaTime);
        }
    
        // Check bounds if enabled
        if (UseWorldBounds)
        {
            Vector2 nextPosition = rigidBody2D.position + _currentVelocity * Time.fixedDeltaTime;
            nextPosition = LevelManager.Instance.ClampToBounds(nextPosition);
        
            // Adjust velocity to respect bounds
            _currentVelocity = (nextPosition - rigidBody2D.position) / Time.fixedDeltaTime;
        }
    
        // Apply movement and rotation
        rigidBody2D.linearVelocity = _currentVelocity;
        gfx.rotation = Quaternion.Euler(_currentXRotation, 0, _currentZRotation);
    }

    #endregion Movement --------------------------------------------------------------------------------


    #region Input ---------------------------------------------------------------------------------

    private void UpdateInput()
    {
        // Movement input
        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _moveDirection = _moveInput.normalized;
        
        // Attack input
        if (Input.GetButtonDown("Fire1"))
        {
            _attackInput = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _attackInput = false;
        }
    }
    

    #endregion Input ---------------------------------------------------------------------------------
    
    
    
    #region IDamageable --------------------------------------------------------------------------

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
        if (rigidBody2D)
        {
            rigidBody2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
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
    
    #endregion IDamageable --------------------------------------------------------------------------
    



    #region IAttacker --------------------------------------------------------------------------

    public void UseWeapon()
    {
        if (!CurrentWeapon) return;
    
        // Set attack direction
        if (_moveDirection == Vector2.zero)
        {
            AttackDirection = new Vector2(1, 0);
        }
        else
        {
            AttackDirection = new Vector2(1, _moveDirection.y);
        }

        if (weaponPosition) WeaponPosition = weaponPosition.position;
    
        // Handle different weapon limiters
        switch (CurrentWeapon.WeaponLimiter)
        {
            case WeaponLimiter.Unlimited:
                if (_attackInput)
                {
                    CurrentWeapon.Use(this);
                }
                break;
            
            case WeaponLimiter.FireRate:
                if (_attackInput && Time.time >= _lastFireTime + CurrentWeapon.FireRate)
                {
                    CurrentWeapon.Use(this);
                    _lastFireTime = Time.time;
                }
                break;
            
            case WeaponLimiter.Charge:
                // Start charging when button is pressed
                if (_attackInput && !_isCharging)
                {
                    _isCharging = true;
                    _chargeStartTime = Time.time;
                }
                // Release charge when button is released
                else if (!_attackInput && _isCharging)
                {
                    _isCharging = false;
                    // Only fire if charged long enough
                    if (Time.time >= _chargeStartTime + CurrentWeapon.ChargeTime)
                    {
                        CurrentWeapon.Use(this);
                    }
                }
                break;
            
            case WeaponLimiter.OneShot:
                if (_attackInput && _canFire)
                {
                    CurrentWeapon.Use(this);
                    _canFire = false;
                    ChangeWeapon(defaultWeapon);
                }
                break;
        }
    }
    

    public void ChangeWeapon(SOWeapon weapon)
    {
        if (!weapon) return;
        CurrentWeapon = weapon;
    
        // Reset weapon state
        _canFire = true;
        _isCharging = false;
        _lastFireTime = 0;
        
        Debug.Log($"Weapon changed to {CurrentWeapon}");
    }

    #endregion IAttacker --------------------------------------------------------------------------
}
