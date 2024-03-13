using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DamageOnTouch : MonoBehaviour
{

    [Tooltip("the layers that will be damaged by this object")]
    public LayerMask TargetLayerMask;
    
    /// the owner of the DamageOnTouch zone
    [Tooltip("the owner of the DamageOnTouch zone")]
    public GameObject Owner;

    protected List<GameObject> _ignoredGameObjects;

    [Tooltip("The min amount of health to remove from the player's health")]
    public float MinDamageCaused = 10f;
    /// The max amount of health to remove from the player's health
    [Tooltip("The max amount of health to remove from the player's health")]
    public float MaxDamageCaused = 10f;

    protected Health _collidingHealth;

    protected Health _colliderHealth;

    protected SphereCollider _sphereCollider;
    protected BoxCollider _boxCollider;

    [Tooltip("The amount of damage taken every time, whether what we collide with is damageable or not")]
    public float DamageTakenEveryTime = 0;

    [Tooltip("The amount of damage taken when colliding with something that is not damageable")]
    public float DamageTakenNonDamageable = 0;

    /// The Health component on which to apply damage taken. If left empty, will attempty to grab one on this object.
    [Tooltip("The Health component on which to apply damage taken. If left empty, will attempty to grab one on this object.")]
    public Health DamageTakenHealth;


    /// The amount of damage taken when colliding with a damageable object
    [Tooltip("The amount of damage taken when colliding with a damageable object")]
    public float DamageTakenDamageable = 0;

    [Tooltip("The duration of the invincibility frames after the hit (in seconds)")]
    public float DamageTakenInvincibilityDuration = 0.5f;


    private void Awake()
    {
        InitializeIgnoreList();
        GrabComponents();
        InitializeColliders();
    }

    /// <summary>
    /// Stores components
    /// </summary>
    protected virtual void GrabComponents()
    {
        if (DamageTakenHealth == null)
        {
            DamageTakenHealth = GetComponent<Health>();
        }
        _boxCollider = GetComponent<BoxCollider>();
        _sphereCollider = GetComponent<SphereCollider>();

    }

    /// <summary>
    /// Initializes colliders, setting them as trigger if needed
    /// </summary>
    protected virtual void InitializeColliders()
    {
        if (_boxCollider != null)
        {
            _boxCollider.isTrigger = true;
        }

        if (_sphereCollider != null)
        {
            _sphereCollider.isTrigger = true;
        }

    }
    /// <summary>
    /// Clears the ignore list.
    /// </summary>
    public virtual void ClearIgnoreList()
    {
        InitializeIgnoreList();
        _ignoredGameObjects.Clear();
    }
    /// <summary>
    /// Initializes the _ignoredGameObjects list if needed
    /// </summary>
    protected virtual void InitializeIgnoreList()
    {
        if (_ignoredGameObjects == null) _ignoredGameObjects = new List<GameObject>();
    }

    /// <summary>
    /// Adds the gameobject set in parameters to the ignore list
    /// </summary>
    /// <param name="newIgnoredGameObject">New ignored game object.</param>
    public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
    {
        InitializeIgnoreList();
        _ignoredGameObjects.Add(newIgnoredGameObject);
    }

    /// <summary>
    /// Removes the object set in parameters from the ignore list
    /// </summary>
    /// <param name="ignoredGameObject">Ignored game object.</param>
    public void StopIgnoringObject(GameObject ignoredGameObject)
    {
        if (_ignoredGameObjects != null) _ignoredGameObjects.Remove(ignoredGameObject);
    }


    /// <summary>
    /// On trigger enter, we call our colliding endpoint
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if ((TargetLayerMask.value & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer) 
        {
            Colliding(collider.gameObject);
        }

    }

    /// <summary>
    /// When colliding, we apply the appropriate damage
    /// </summary>
    /// <param name="collider"></param>
    protected virtual void Colliding(GameObject collider)
    {
        if (!EvaluateAvailability(collider))
        {
            return;
        }

        // cache reset 
        _colliderHealth = collider.gameObject.GetComponent<Health>();

        // if what we're colliding with is damageable
        if (_colliderHealth != null)
        {
            if (_colliderHealth.CurrentHealth > 0)
            {
                OnCollideWithDamageable(_colliderHealth);
            }
        }
        else // if what we're colliding with can't be damaged
        {
            OnCollideWithNonDamageable();
        }
    }

    /// <summary>
    /// Checks whether or not damage should be applied this frame
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    protected virtual bool EvaluateAvailability(GameObject collider)
    {
        // if we're inactive, we do nothing
        if (!isActiveAndEnabled) { return false; }

        // if the object we're colliding with is part of our ignore list, we do nothing and exit
        if (_ignoredGameObjects.Contains(collider)) { return false; }

        // if we're on our first frame, we don't apply damage
        if (Time.time == 0f) { return false; }

        return true;
    }

    /// <summary>
    /// Describes what happens when colliding with a damageable object
    /// </summary>
    /// <param name="health">Health.</param>
    protected virtual void OnCollideWithDamageable(Health health)
    {
        _collidingHealth = health;

        if (health.CanTakeDamageThisFrame())
        {
            // we apply the damage to the thing we've collided with
            float randomDamage =
                UnityEngine.Random.Range(MinDamageCaused, Mathf.Max(MaxDamageCaused, MinDamageCaused));
            _colliderHealth.Damage(randomDamage, gameObject, 0f, DamageTakenInvincibilityDuration);

        }

        // we apply self damage
        if (DamageTakenEveryTime + DamageTakenDamageable > 0)
        {
            SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
        }
    }

    /// <summary>
    /// Describes what happens when colliding with a non damageable object
    /// </summary>
    protected virtual void OnCollideWithNonDamageable()
    {
        float selfDamage = DamageTakenEveryTime + DamageTakenNonDamageable;
        if (selfDamage > 0)
        {
            SelfDamage(selfDamage);
        }

    }


    /// <summary>
    /// Applies damage to itself
    /// </summary>
    /// <param name="damage">Damage.</param>
    protected virtual void SelfDamage(float damage)
    {
        if (DamageTakenHealth != null)
        {
            DamageTakenHealth.Damage(damage, gameObject, 0f, DamageTakenInvincibilityDuration);
        }

 
    }




}
