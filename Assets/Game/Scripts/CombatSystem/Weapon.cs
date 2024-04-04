using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.ParticleSystem;
using static Weapon;

public class Weapon : MonoBehaviour
{

    /// the possible states the weapon can be in
    public enum WeaponStates { WeaponIdle, WeaponStart, WeaponDelayBeforeUse , WeaponUse, WeaponDelayBetweenUses, WeaponStop }

    /// the name of the weapon, only used for debugging
    [Tooltip("the name of the weapon, only used for debugging")]
    public string WeaponName;
    [Tooltip("the number of projectiles to spawn per shot")]
    public int ProjectilesPerShot = 1;
    /// the weapon's owner
    public Character Owner;

    protected bool _triggerReleased = false;
    

    protected Weapon _weapon;

    public WeaponStates weaponState { get; private set; }

    /// the weapon's owner's CharacterHandleWeapon component
    public CharacterHandleWeapon CharacterHandleWeapon;
    /// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
    [Tooltip("an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.")]
    public Vector3 WeaponAttachmentOffset = Vector3.zero;

    public WeaponStates WeaponState=WeaponStates.WeaponIdle;

    [Tooltip("the projectile's spawn position")]
    public Vector3 SpawnPosition = Vector3.zero;

    /// the offset position at which the projectile will spawn
    [Tooltip("the offset position at which the projectile will spawn")]
    public Vector3 ProjectileSpawnOffset = Vector3.zero;

   // public GameObject weaponprojectile;
    public AssetReference loadWeaponprojectile;

    /// the delay before use, that will be applied for every shot
    [Tooltip("the delay before use, that will be applied for every shot")]
    public float DelayBeforeUse = 0f;
    public float _delayBetweenUsesCounter=0.0f;
    public float TimeBetweenUses = 1.5f;
    private float _lastShootRequestAt = -float.MaxValue;
    public int _MaxAmmo = 6;
    private int _CurrentAmmo;
    private float _delayBeforeUseCounter = 0f;





    /// <summary>
    /// Initialize this weapon.
    /// </summary>
    public void Initialization()
    {
        weaponState = WeaponStates.WeaponIdle;
        _CurrentAmmo = _MaxAmmo;
    }


    /// <summary>
    /// Determines whether or not the weapon can fire
    /// </summary>
    /// 
    public virtual IEnumerator ShootRequestCo()
    {
        if (Time.time - _lastShootRequestAt < TimeBetweenUses)
        {
            yield break;
        }

        int remainingShots = 1;
        float interval = 1;

        while (remainingShots > 0)
        {
            ShootRequest();
            _lastShootRequestAt = Time.time;
            remainingShots--;
            yield return new WaitForSeconds(interval);
        }
    }

    public virtual void ShootRequest()
    {
        // if we have a weapon ammo component, we determine if we have enough ammunition to shoot
        if (_CurrentAmmo <= 0)
        {
            return;
        }
        _CurrentAmmo--;

        WeaponState=WeaponStates.WeaponUse;
    }

    /// <summary>
    /// On LateUpdate, processes the weapon state
    /// </summary>
    protected virtual void LateUpdate()
    {
        ProcessWeaponState();
    }
    /// <summary>
    /// Called every lastUpdate, processes the weapon's state machine
    /// </summary>
    protected virtual void ProcessWeaponState()
    {

        switch (WeaponState)
        {
            case WeaponStates.WeaponStart:
                CaseWeaponStart();
                break;
            case WeaponStates.WeaponDelayBeforeUse:
                CaseWeaponDelayBeforeUse();
                break;


            case WeaponStates.WeaponUse:
                CaseWeaponUse();
                break;

            case WeaponStates.WeaponDelayBetweenUses:
                CaseWeaponDelayBetweenUses();
                break;

            case WeaponStates.WeaponStop:
                CaseWeaponStop();
                break;
        }
    }
    /// <summary>
    /// When the weapon starts we switch to a delay or shoot based on our weapon's settings
    /// </summary>
    public virtual void CaseWeaponStart()
    {
        if (DelayBeforeUse > 0)
        {
            _delayBeforeUseCounter = DelayBeforeUse;
            WeaponState = WeaponStates.WeaponDelayBeforeUse;
        }
        else
        {
            StartCoroutine(ShootRequestCo());
        }
    }

    /// <summary>
    /// If we're in delay before use, we wait until our delay is passed and then request a shoot
    /// </summary>
    public virtual void CaseWeaponDelayBeforeUse()
    {
        _delayBeforeUseCounter -= Time.deltaTime;
        if (_delayBeforeUseCounter <= 0)
        {
            StartCoroutine(ShootRequestCo());
        }
    }
    /// <summary>
    /// On weapon use we use our weapon then switch to delay between uses
    /// </summary>
    public void CaseWeaponUse()
    {
        WeaponUse();
        _delayBetweenUsesCounter = TimeBetweenUses;
        WeaponState=WeaponStates.WeaponDelayBetweenUses;
    }
    /// <summary>
    /// Called everytime the weapon is used
    /// </summary>
    public void WeaponUse()
    {
        DetermineSpawnPosition();

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            SpawnProjectile(SpawnPosition);
        }
    }
    /// <summary>
    /// When in delay between uses, we either turn our weapon off or make a shoot request
    /// </summary>
    public virtual void CaseWeaponDelayBetweenUses()
    {
        _delayBetweenUsesCounter -= Time.deltaTime;
        if(_delayBetweenUsesCounter < 0)
        {
            WeaponState = WeaponStates.WeaponStop;
        }

    }
    private void SpawnProjectile(Vector3 spawnPosition)
    {

        AssetManager.Instance.Inst(loadWeaponprojectile, spawnPosition, Quaternion.identity, InitializeNextProjectile);

        // we position the object
        //nextGameObject.transform.position = spawnPosition;
        // we set its direction

    }

    private void InitializeNextProjectile(NetworkObject nextGameObject)
    {
        if (nextGameObject.TryGetComponent<Projectile>(out var projectile))
        {
            projectile.SetWeapon(this);
            if (Owner != null)
            {
                projectile.SetOwner(Owner.gameObject);
            }
        }
        if (nextGameObject.TryGetComponent<Health>(out var health))
        {
            health.SetNetworkObject(nextGameObject);
        }

        // we activate the object
        nextGameObject.gameObject.SetActive(true);

        if (projectile != null)
        {
            projectile.SetDirection(transform.forward, transform.rotation, true);
        }
    }

    /// <summary>
    /// On weapon stop, we switch to idle
    /// </summary>
    public void CaseWeaponStop()
    {
        WeaponState = WeaponStates.WeaponIdle;
    }
    /// <summary>
    /// Determines the spawn position based on the spawn offset and whether or not the weapon is flipped
    /// </summary>
    public void DetermineSpawnPosition()
    {
        SpawnPosition = this.transform.position + this.transform.rotation * ProjectileSpawnOffset;
    }




    /// <summary>
    /// Sets the weapon's owner
    /// </summary>
    /// <param name="newOwner">New owner.</param>
    public void SetOwner(Character newOwner, CharacterHandleWeapon handleWeapon)
    {
        Debug.Log("Set new Owner");
        Owner = newOwner;
        if (Owner != null)
        {
            CharacterHandleWeapon = handleWeapon;
        }
    }

    /// <summary>
    /// Called by input, turns the weapon on
    /// </summary>
    public void WeaponInputStart()
    {
        if (_CurrentAmmo <= 0)
        {
            return;
        }

        if (WeaponState == WeaponStates.WeaponIdle)
        {
            _triggerReleased = false;
            WeaponState=WeaponStates.WeaponStart;
        }
    }

    /// <summary>
    /// Turns the weapon off.
    /// </summary>
    public void TurnWeaponOff()
    {
        if ((WeaponState == WeaponStates.WeaponIdle || WeaponState == WeaponStates.WeaponStop))
        {
            return;
        }
        _triggerReleased = true;
        WeaponState=WeaponStates.WeaponStop;
    }
    public void DestroyWeapon()
    {

        Destroy(this.gameObject);
    }

    public int GetCurrentAmmo()
    {
        return _CurrentAmmo;
    }
}
