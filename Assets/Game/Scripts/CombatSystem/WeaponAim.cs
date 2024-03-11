using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Cinemachine.CinemachineTargetGroup;

public class WeaponAim : MonoBehaviour
{



    protected Camera _mainCamera;
    protected Weapon _weapon;
    protected Vector3 _currentAim = Vector3.zero;
    protected Vector3 _currentAimAbsolute = Vector3.zero;
    protected Quaternion _initialRotation;
    protected GameObject _reticle;
    protected Vector3 _reticlePosition;
    public Canvas _targetCanvas;
    protected bool _initialized = false;

    [Tooltip("the gameobject to display as the aim's reticle/crosshair. Leave it blank if you don't want a reticle")]
    public GameObject Reticle;

    [Tooltip("if set to true, the reticle will replace the mouse pointer")]
    public bool ReplaceMousePointer = true;
    /// <summary>
    /// On Start(), we trigger the initialization
    /// </summary>
    protected void Start()
    {
        Initialization();
    }
    /// <summary>
    /// Grabs the weapon component, initializes the angle values
    /// </summary>
    private void Initialization()
    {
        _weapon = GetComponent<Weapon>();
        _mainCamera = Camera.main;


        _initialRotation = transform.rotation;
        InitializeReticle();
        _initialized = true;
    }

    /// <summary>
    /// Initializes the reticle based on the settings defined in the inspector
    /// </summary>
    private  void InitializeReticle()
    {
        if (_weapon.Owner == null) { return; }
        if (Reticle == null) { return; }


        if (_reticle != null)
        {
            Destroy(_reticle);
        }

        _reticle = (GameObject)Instantiate(Reticle);
        _reticle.transform.SetParent(_targetCanvas.transform);
        _reticle.transform.localScale = Vector3.one;
        if (_reticle.gameObject.GetComponent<UIFollowMouse>() != null)
        {
            _reticle.gameObject.GetComponent<UIFollowMouse>().TargetCanvas = _targetCanvas;
        }

    }
    /// <summary>
    /// Every frame, moves the reticle if it's been told to follow the pointer
    /// </summary>
    protected void MoveReticle()
    {
        if (_reticle == null) { return; }

        _reticlePosition = _reticle.transform.position;


    }
}
