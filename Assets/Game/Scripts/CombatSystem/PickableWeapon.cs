using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PickableWeapon : PickableItem
{

    [Header("Pickable Weapon")]
    // the new weapon the player gets when collecting this object
    [Tooltip("the new weapon the player gets when collecting this object")]
    public Weapon WeaponToGive;

    protected CharacterHandleWeapon _characterHandleWeapon;


    /// <summary>
    /// What happens when the weapon gets picked
    /// </summary>
    protected override void Pick(GameObject picker)
    {
        Character character = _collidingObject.gameObject.GetComponent<Character>();

        if (character == null)
        {
            return;
        }
        if (_characterHandleWeapon != null)
        {
            _characterHandleWeapon.ChangeWeapon(WeaponToGive);
        }

        Debug.Log("!!!!!!!!!!!!!!!Pick UP weapon!!!!!!!!!!!");
    }
    /// <summary>
    /// Checks if the object is pickable.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    protected override bool CheckIfPickable()
    {
        _character = _collidingObject.GetComponent<Character>();

        // if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
        if ((_character == null) || (_character.GetComponent<CharacterHandleWeapon>() == null))
        {
            return false;
        }
        if (_character.CharacterType != Character.CharacterTypes.Warrior)
        {
            return false;
        }
        // we equip the weapon to the chosen CharacterHandleWeapon
        CharacterHandleWeapon[] handleWeapons = _character.GetComponentsInChildren<CharacterHandleWeapon>();
        foreach (CharacterHandleWeapon handleWeapon in handleWeapons)
        {
            _characterHandleWeapon = handleWeapon;
        }

        if (_characterHandleWeapon == null)
        {
            return false;
        }
        return true;
    }
}

