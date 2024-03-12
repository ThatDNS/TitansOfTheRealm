using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class Weapon : MonoBehaviour
{

    /// the possible states the weapon can be in
    public enum WeaponStates { WeaponIdle, WeaponStart, WeaponDelayBeforeUse, WeaponUse, WeaponDelayBetweenUses, WeaponStop, WeaponReloadNeeded, WeaponReloadStart, WeaponReload, WeaponReloadStop, WeaponInterrupted }

    /// the name of the weapon, only used for debugging
    [Tooltip("the name of the weapon, only used for debugging")]
    public string WeaponName;

    /// the weapon's owner
    public Character Owner;

    protected bool _triggerReleased = false;
    protected bool _reloading = false;

    protected Weapon _weapon;

    [Tooltip("the current amount of ammo loaded inside the weapon")]
    public int CurrentAmmoLoaded = 0;
    public WeaponStates weaponState { get; private set; }
    [Tooltip("the current amount of ammo available in the inventory")]
    public int CurrentAmmoAvailable;

    /// the weapon's owner's CharacterHandleWeapon component
    public CharacterHandleWeapon CharacterHandleWeapon;
    /// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
    [Tooltip("an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.")]
    public Vector3 WeaponAttachmentOffset = Vector3.zero;


    public void TurnWeaponOff()
    {

        _triggerReleased = true;



        bool needToReload = false;
        needToReload = (CurrentAmmoLoaded <= 0);

        if (needToReload)
        {
            InitiateReloadWeapon();
        }

    }


    /// <summary>
    /// Initiates a reload
    /// </summary>
    public void InitiateReloadWeapon()
    {


        // if we're already reloading, we do nothing and exit
        if (_reloading)
        {
            return;
        }
        weaponState = WeaponStates.WeaponReloadStart;
        _reloading = true;
    }

    /// <summary>
    /// Initialize this weapon.
    /// </summary>
    public void Initialization()
    {
        weaponState = WeaponStates.WeaponIdle;
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
}
