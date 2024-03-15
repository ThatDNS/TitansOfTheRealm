using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum MovementVectors { Forward, Right, Up }

    public float Speed = 0;
    public float Acceleration = 0;
    [Tooltip("the current direction of the object")]
    public Vector3 Direction = Vector3.left;
    /// the initial delay during which the projectile can't be destroyed
    [Tooltip("the initial delay during which the projectile can't be destroyed")]
    public float InitialInvulnerabilityDuration = 0f;
    /// should the projectile damage its owner?
    [Tooltip("should the projectile damage its owner?")]
    public bool DamageOwner = false;
    /// if true, the projectile will rotate towards movement
    [Tooltip("if true, the projectile will rotate towards movement")]
    public bool FaceMovement = false;

    protected WaitForSeconds _initialInvulnerabilityDurationWFS;
    protected Collider _collider;
    protected Weapon _weapon;
    protected GameObject _owner;
    protected Vector3 _movement;
    protected Rigidbody _rigidBody;
    protected float _initialSpeed;
    protected Vector3 _initialLocalScale;
    public bool _shouldMove = true;
    protected Health _health;
    protected DamageOnTouch _damageOnTouch;

    public Weapon SourceWeapon { get { return _weapon; } }

    /// <summary>
    /// On awake, we store the initial speed of the object 
    /// </summary>
    protected  void Awake()
    {
        _initialSpeed = Speed;
        _health = GetComponent<Health>();
        _collider = GetComponent<Collider>();

        //_damageOnTouch = GetComponent<DamageOnTouch>();
        _rigidBody = GetComponent<Rigidbody>();

        _initialInvulnerabilityDurationWFS = new WaitForSeconds(InitialInvulnerabilityDuration);

        _initialLocalScale = transform.localScale;
    }

    /// <summary>
    /// Handles the projectile's initial invincibility
    /// </summary>
    /// <returns>The invulnerability.</returns>
    protected virtual IEnumerator InitialInvulnerability()
    {
        if (_damageOnTouch == null) { yield break; }
        if (_weapon == null) { yield break; }

        _damageOnTouch.ClearIgnoreList();
        _damageOnTouch.IgnoreGameObject(_weapon.Owner.gameObject);
        yield return _initialInvulnerabilityDurationWFS;
        if (DamageOwner)
        {
            _damageOnTouch.StopIgnoringObject(_weapon.Owner.gameObject);
        }
    }
    /// <summary>
    /// Initializes the projectile
    /// </summary>
    protected void Initialization()
    {
        Speed = _initialSpeed;
        transform.localScale = _initialLocalScale;
        _shouldMove = true;


        if (_collider != null)
        {
            _collider.enabled = true;
        }

    }
    /// <summary>
    /// On update(), we move the object based on the level's speed and the object's speed, and apply acceleration
    /// </summary>
    protected void FixedUpdate()
    {
        if (_shouldMove)
        {
            Movement();
        }
    }
    /// <summary>
    /// Handles the projectile's movement, every frame
    /// </summary>
    public void Movement()
    {
        _movement = Direction * (Speed / 10) * Time.deltaTime;
        //transform.Translate(_movement,Space.World);
        if (_rigidBody != null)
        {
            _rigidBody.MovePosition(this.transform.position + _movement);
        }

        // We apply the acceleration to increase the speed
        Speed += Acceleration * Time.deltaTime;
    }

    /// <summary>
    /// Sets the projectile's direction.
    /// </summary>
    /// <param name="newDirection">New direction.</param>
    /// <param name="newRotation">New rotation.</param>
    /// <param name="spawnerIsFacingRight">If set to <c>true</c> spawner is facing right.</param>
    public virtual void SetDirection(Vector3 newDirection, Quaternion newRotation, bool spawnerIsFacingRight = true)
    {
        Direction = newDirection;       
        transform.rotation = newRotation;
    }

    /// <summary>
    /// Sets the projectile's parent weapon.
    /// </summary>
    /// <param name="newWeapon">New weapon.</param>
    public virtual void SetWeapon(Weapon newWeapon)
    {
        _weapon = newWeapon;
    }

    /// <summary>
    /// Sets the damage caused by the projectile's DamageOnTouch to the specified value
    /// </summary>
    /// <param name="newDamage"></param>
    public virtual void SetDamage(float minDamage, float maxDamage)
    {
        if (_damageOnTouch != null)
        {
            _damageOnTouch.MinDamageCaused = minDamage;
            _damageOnTouch.MaxDamageCaused = maxDamage;
        }
    }
    /// <summary>
    /// Sets the projectile's owner.
    /// </summary>
    /// <param name="newOwner">New owner.</param>
    public virtual void SetOwner(GameObject newOwner)
    {
        _owner = newOwner;
        DamageOnTouch damageOnTouch = this.gameObject.GetComponent<DamageOnTouch>();
        if (damageOnTouch != null)
        {
            damageOnTouch.Owner = newOwner;
            if (!DamageOwner)
            {
                damageOnTouch.ClearIgnoreList();
                damageOnTouch.IgnoreGameObject(newOwner);
            }
        }
    }
    /// <summary>
    /// Returns the current Owner of the projectile
    /// </summary>
    /// <returns></returns>
    public GameObject GetOwner()
    {
        return _owner;
    }

    /// <summary>
    /// On death, disables colliders and prevents movement
    /// </summary>
    public void StopAt()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }


        _shouldMove = false;
    }

    /// <summary>
    /// On death, we stop our projectile
    /// </summary>
    protected void OnDeath()
    {
        StopAt();
    }

    /// <summary>
    /// On enable, we trigger a short invulnerability
    /// </summary>
    protected  void OnEnable()
    {
        Initialization();
        if (InitialInvulnerabilityDuration > 0)
        {
            StartCoroutine(InitialInvulnerability());
        }

        if (_health != null)
        {
            _health.OnDeath += OnDeath;
        }

    }

    /// <summary>
    /// On disable, we plug our OnDeath method to the health component
    /// </summary>
    protected void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDeath -= OnDeath;
        }
    }
}
