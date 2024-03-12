using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("the model to disable (if set so)")]
    public GameObject Model;

    [Tooltip("the current health of the character")]
    public float CurrentHealth;

    [Tooltip("If this is true, this object can't take damage at this time")]
    public bool Invulnerable = false;

    [Tooltip("the initial amount of health of the object")]
    public float InitialHealth = 10;
    /// the maximum amount of health of the object
    [Tooltip("the maximum amount of health of the object")]
    public float MaximumHealth = 10;
    /// if this is true, health values will be reset everytime this character is enabled (usually at the start of a scene)
    [Tooltip("if this is true, health values will be reset everytime this character is enabled (usually at the start of a scene)")]
    public bool ResetHealthOnEnable = true;

    /// whether or not this Health object can be damaged 
    [Tooltip("whether or not this Health object can be damaged")]
    public bool ImmuneToDamage = false;


    [Tooltip("whether or not this object should get destroyed on death")]
    public bool DestroyOnDeath = true;
    /// the time (in seconds) before the character is destroyed or disabled
    [Tooltip("the time (in seconds) before the character is destroyed or disabled")]
    public float DelayBeforeDestruction = 0f;


    /// if this is true, the model will be disabled instantly on death (if a model has been set)
    [Tooltip("if this is true, the model will be disabled instantly on death (if a model has been set)")]
    public bool DisableModelOnDeath = true;
    /// if this is true, collisions will be turned off when the character dies
    [Tooltip("if this is true, collisions will be turned off when the character dies")]
    public bool DisableCollisionsOnDeath = true;
    /// if this is true, collisions will also be turned off on child colliders when the character dies
    [Tooltip("if this is true, collisions will also be turned off on child colliders when the character dies")]
    public bool DisableChildCollisionsOnDeath = false;

    public virtual float LastDamage { get; set; }
    public virtual Vector3 LastDamageDirection { get; set; }
    public virtual bool Initialized => _initialized;

    // hit delegate
    public delegate void OnHitDelegate();
    public OnHitDelegate OnHit;

    // respawn delegate
    public delegate void OnReviveDelegate();
    public OnReviveDelegate OnRevive;

    // death delegate
    public delegate void OnDeathDelegate();
    public OnDeathDelegate OnDeath;

    protected Vector3 _initialPosition;
    protected Renderer _renderer;
    protected Character _character;

    protected Collider _collider3D;
    protected bool _initialized = false;


    #region Initialization
    protected void Awake()
    {
        Initialization();
        InitializeCurrentHealth();
    }
    /// <summary>
    /// Initializes health to either initial or current values
    /// </summary>
    public  void InitializeCurrentHealth()
    {
         SetHealth(InitialHealth);
    }
    /// <summary>
    /// Grabs useful components, enables damage and gets the inital color
    /// </summary>
    public  void Initialization()
    {
        _character = this.gameObject.GetComponentInParent<Character>();

        if (Model != null)
        {
            Model.SetActive(true);
        }

        if (gameObject.GetComponentInParent<Renderer>() != null)
        {
            _renderer = GetComponentInParent<Renderer>();
        }
 



        _collider3D = this.gameObject.GetComponentInParent<Collider>();


        _initialized = true;

        DamageEnabled();
    }
    /// <summary>
    /// When the object is enabled (on respawn for example), we restore its initial health levels
    /// </summary>
    protected virtual void OnEnable()
    {
        if (ResetHealthOnEnable)
        {
            InitializeCurrentHealth();
        }
        if (Model != null)
        {
            Model.SetActive(true);
        }
        DamageEnabled();
    }

    /// <summary>
    /// On Disable, we prevent any delayed destruction from running
    /// </summary>
    protected virtual void OnDisable()
    {
        CancelInvoke();
    }

#endregion

    /// <summary>
    /// Returns true if this Health component can be damaged this frame, and false otherwise
    /// </summary>
    /// <returns></returns>
    public virtual bool CanTakeDamageThisFrame()
    {
        // if the object is invulnerable, we do nothing and exit
        if (Invulnerable || ImmuneToDamage)
        {
            return false;
        }

        if (!this.enabled)
        {
            return false;
        }

        // if we're already below zero, we do nothing and exit
        if ((CurrentHealth <= 0) && (InitialHealth != 0))
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Sets the current health to the specified new value, and updates the health bar
    /// </summary>
    /// <param name="newValue"></param>
    public  void SetHealth(float newValue)
    {
        CurrentHealth = newValue;
    }

    /// <summary>
    /// Called when the character gets health (from a stimpack for example)
    /// </summary>
    /// <param name="health">The health the character gets.</param>
    /// <param name="instigator">The thing that gives the character health.</param>
    public virtual void ReceiveHealth(float health)
    {
        SetHealth(Mathf.Min(CurrentHealth + health, MaximumHealth));
    }

    /// <summary>
    /// Resets the character's health to its max value
    /// </summary>
    public virtual void ResetHealthToMaxHealth()
    {
        SetHealth(MaximumHealth);
    }

    /// <summary>
    /// Prevents the character from taking any damage
    /// </summary>
    public void DamageDisabled()
    {
        Invulnerable = true;
    }

    /// <summary>
    /// Allows the character to take damage
    /// </summary>
    public  void DamageEnabled()
    {
        Invulnerable = false;
    }

    /// <summary>
    /// Called when the object takes damage
    /// </summary>
    /// <param name="damage">The amount of health points that will get lost.</param>
    /// <param name="instigator">The object that caused the damage.</param>
    /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage - not used anymore, kept to not break retrocompatibility</param>
    /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
    public virtual void Damage(float damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
    {
        if (!CanTakeDamageThisFrame())
        {
            return;
        }

        damage = ComputeDamageOutput(damage);

        // we decrease the character's health by the damage
        float previousHealth = CurrentHealth;

            SetHealth(CurrentHealth - damage);
       

        LastDamage = damage;

        if (OnHit != null)
        {
            OnHit();
        }

        // we prevent the character from colliding with Projectiles, Player and Enemies
        if (invincibilityDuration > 0)
        {
            DamageDisabled();
            StartCoroutine(DamageEnabled(invincibilityDuration));
        }



        // if health has reached zero we set its health to zero

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Kill();
        }


    }
    /// <summary>
    /// Kills the character, instantiates death effects, handles points, etc
    /// </summary>
    public void Kill()
    {
        if (ImmuneToDamage)
        {
            return;
        }
        SetHealth(0);
        // we prevent further damage
        DamageDisabled();

        // we make it ignore the collisions from now on
        if (DisableCollisionsOnDeath)
        {

            if (_collider3D != null)
            {
                _collider3D.enabled = false;
            }

            if (DisableChildCollisionsOnDeath)
            {

                foreach (Collider collider in this.gameObject.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = false;
                }
            }
        }
    }
    /// <summary>
    /// Revive this object.
    /// </summary>
    public virtual void Revive()
    {
        if (!_initialized)
        {
            return;
        }

        if (_collider3D != null)
        {
            _collider3D.enabled = true;
        }
        if (DisableChildCollisionsOnDeath)
        {
            foreach (Collider2D collider in this.gameObject.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = true;
            }
            foreach (Collider collider in this.gameObject.GetComponentsInChildren<Collider>())
            {
                collider.enabled = true;
            }
        }
 

        Initialization();
        InitializeCurrentHealth();

    }

    /// <summary>
    /// Returns the damage this health should take
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public virtual float ComputeDamageOutput(float damage)
    {
        if (Invulnerable || ImmuneToDamage)
        {
            return 0;
        }

        return damage;
    }


    /// <summary>
    /// makes the character able to take damage again after the specified delay
    /// </summary>
    /// <returns>The layer collision.</returns>
    public virtual IEnumerator DamageEnabled(float delay)
    {
        yield return new WaitForSeconds(delay);
        Invulnerable = false;
    }


}
