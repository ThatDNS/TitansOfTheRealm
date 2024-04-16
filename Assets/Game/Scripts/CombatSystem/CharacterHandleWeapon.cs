using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class CharacterHandleWeapon : MonoBehaviour
{

    [Tooltip("the weapon currently equipped by the Character")]
    public Weapon CurrentWeapon;

        /// the position the weapon will be attached to. If left blank, will be this.transform.
    [Tooltip("the position the weapon will be attached to. If left blank, will be this.transform.")]
    public Transform WeaponAttachment;

    /// the position from which projectiles will be spawned (can be safely left empty)
    [Tooltip("the position from which projectiles will be spawned (can be safely left empty)")]
    public Transform ProjectileSpawn;

    /// an animator to update when the weapon is used
    public Animator CharacterAnimator;

    protected Character _character;

    protected WeaponAim _weaponAim;

    private void Awake()
    {
        _character=gameObject.GetComponent<Character>();
        CharacterAnimator=GetComponentInChildren<Animator>();
    }
    public virtual void ChangeWeapon(Weapon newWeapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.TurnWeaponOff();

            ShootStop();
            Destroy(CurrentWeapon.gameObject);
            CharacterAnimator.SetBool("HandleWeapon", false);

        }
        if (newWeapon != null)
        {
            InstantiateWeapon(newWeapon);
        }
        else
        {
            CurrentWeapon = null;
        }


    }

    /// <summary>
    /// Instantiates the specified weapon
    /// </summary>
    /// <param name="newWeapon"></param>
    /// <param name="weaponID"></param>
    /// <param name="combo"></param>
    protected virtual void InstantiateWeapon(Weapon newWeapon)
    {
    
        CurrentWeapon = (Weapon)Instantiate(newWeapon, WeaponAttachment.transform.position + newWeapon.WeaponAttachmentOffset, WeaponAttachment.transform.rotation);
       
        CurrentWeapon.name = newWeapon.name;
        CurrentWeapon.transform.parent = WeaponAttachment.transform;
        CurrentWeapon.transform.localPosition = newWeapon.WeaponAttachmentOffset;
        CurrentWeapon.SetOwner(_character, this);


        // we turn off the gun's emitters.
        CurrentWeapon.Initialization();
        _weaponAim=CurrentWeapon.GetComponent<WeaponAim>();
        CharacterAnimator.SetBool("HandleWeapon",true);

    }



    /// <summary>
    /// Causes the character to stop shooting
    /// </summary>
    public virtual void ShootStop()
    {
        // if the Shoot action is enabled in the permissions, we continue, if not we do nothing
        if ( CurrentWeapon == null)
        {
            return;
        }

        if (CurrentWeapon.weaponState == Weapon.WeaponStates.WeaponIdle)
        {
            return;
        }

        if ((CurrentWeapon.weaponState == Weapon.WeaponStates.WeaponDelayBeforeUse))
        {
            return;
        }

        if ((CurrentWeapon.weaponState == Weapon.WeaponStates.WeaponDelayBetweenUses))
        {
            return;
        }

        if (CurrentWeapon.weaponState == Weapon.WeaponStates.WeaponUse)
        {
            return;
        }

        ForceStop();
    }

    /// <summary>
    /// Causes the character to start shooting
    /// </summary>
    public virtual void ShootStart()
    {
        if (CurrentWeapon.GetCurrentAmmo() <= 0) 
        {
            _weaponAim.RemoveReticle();
            CurrentWeapon.DestroyWeapon();
        }
        CurrentWeapon.WeaponInputStart();
    }
    /// <summary>
    /// Forces the weapon to stop 
    /// </summary>
    public void ForceStop()
    {

        if (CurrentWeapon != null)
        {
            CurrentWeapon.TurnWeaponOff();
        }
    }
}