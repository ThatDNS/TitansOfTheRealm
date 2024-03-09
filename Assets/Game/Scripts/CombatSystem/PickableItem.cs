using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PickableItem : MonoBehaviour
{
    protected Collider _collider;
    protected GameObject _collidingObject;
    protected Character _character;

    protected bool _pickable = false;

    protected WaitForSeconds _disableDelay;

    [Header("Pick Conditions")]
    /// if this is true, this pickable item will only be pickable by objects with a Character component 
    [Tooltip("if this is true, this pickable item will only be pickable by objects with a Character component")]
    public bool RequireCharacterComponent = true;
    /// if this is true, this pickable item will only be pickable by objects with a Character component of type player
    [Tooltip("if this is true, this pickable item will only be pickable by objects with a Character component of type player")]
    public bool RequirePlayerType = true;


    public bool DisableColliderOnPick = false;
    /// if this is set to true, the object will be disabled when picked
    [Tooltip("if this is set to true, the object will be disabled when picked")]
    public bool DisableObjectOnPick = true;
    /// the duration (in seconds) after which to disable the object, instant if 0

    [Tooltip("the duration (in seconds) after which to disable the object, instant if 0")]
    public float DisableDelay = 0f;
    /// if this is set to true, the object will be disabled when picked
    [Tooltip("if this is set to true, the object will be disabled when picked")]
    public bool DisableModelOnPick = false;
 

    public GameObject Model;
    /// the object to disable on pick if DisableTargetObjectOnPick is true 
    void Start()
    {
        _collider = GetComponent<Collider>();

    }


    public virtual void OnTriggerEnter(Collider collider)
    {
        _collidingObject=collider.gameObject;
        PickItem(collider.gameObject);
        Debug.Log("Enter");
    }


    public virtual void PickItem(GameObject picker)
    {
        if (CheckIfPickable())
        {
            Pick(picker);
            if (DisableColliderOnPick)
            {
                if (_collider != null)
                {
                    _collider.enabled = false;
                }

            }
            if (DisableModelOnPick && (Model != null))
            {
                Model.gameObject.SetActive(false);
            }
            if (DisableObjectOnPick)
            {
                // we desactivate the gameobject
                if (DisableDelay == 0f)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(DisablePickerCoroutine());
                }
            }

        }
    }

    /// <summary>
    /// Checks if the object is pickable.
    /// </summary>
    /// <returns><c>true</c>, if if pickable was checked, <c>false</c> otherwise.</returns>
    protected virtual bool CheckIfPickable()
    {
        // if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
        _character = _collidingObject.GetComponent<Character>();
        if (RequireCharacterComponent)
        {
            if (_character == null)
            {
                return false;
            }

            if (RequirePlayerType && (_character.CharacterType != Character.CharacterTypes.Warrior))
            {
                return false;
            }
        }


        return true;
    }


    protected virtual IEnumerator DisablePickerCoroutine()
    {
        yield return _disableDelay;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Override this to describe what happens when the object gets picked
    /// </summary>
    protected virtual void Pick(GameObject picker)
    {

    }
}
