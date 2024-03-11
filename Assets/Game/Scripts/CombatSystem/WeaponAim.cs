using UnityEngine;


public class WeaponAim : MonoBehaviour
{



    protected Camera _mainCamera;
    protected Weapon _weapon;
    protected Vector3 _currentAim = Vector3.zero;
    protected Vector3 _currentAimAbsolute = Vector3.zero;
    protected Quaternion _initialRotation;
    protected GameObject _reticle;
    protected Vector3 _reticlePosition;
    protected Vector3 _mousePosition;
    protected Vector3 _lastMousePosition;
    protected Vector3 _direction;

    [Tooltip("the radius around the weapon rotation centre where the mouse will be ignored, to avoid glitches")]
    public float MouseDeadZoneRadius = 0.5f;

    private Canvas _targetCanvas;
    protected bool _initialized = false;

    [Tooltip("the gameobject to display as the aim's reticle/crosshair. Leave it blank if you don't want a reticle")]
    public GameObject Reticle;

    [Tooltip("if set to true, the reticle will replace the mouse pointer")]
    public bool ReplaceMousePointer = true;

    [Tooltip("if set to false, the reticle won't be added and displayed")]
    public bool DisplayReticle = true;
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
    protected void GetCurrentAim()
    {
        if (_weapon.Owner == null)
        {
            return;
        }
        GetMouseAim();
    }

    public void GetMouseAim()
    {
        _mousePosition = Input.mousePosition;

        Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);


        _reticlePosition = _direction;

        if (Vector3.Distance(_direction, transform.position) < MouseDeadZoneRadius)
        {
            _direction = _lastMousePosition;
        }
        else
        {
            _lastMousePosition = _direction;
        }

        _currentAim = _direction - _weapon.Owner.transform.position;
    }

    /// <summary>
    /// Every frame, we compute the aim direction and rotate the weapon accordingly
    /// </summary>
    protected void Update()
    {
        HideMousePointer();
        HideReticle();
        GetCurrentAim();
    }

    private void FixedUpdate()
    {
        MoveTarget();
        MoveReticle();
    }


    /// <summary>
    /// Hides or show the mouse pointer based on the settings
    /// </summary>
    protected virtual void HideMousePointer()
    {


        if (ReplaceMousePointer)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
    /// <summary>
    /// Removes any remaining reticle
    /// </summary>
    public virtual void RemoveReticle()
    {
        if (_reticle != null)
        {
            Destroy(_reticle.gameObject);
        }
    }

    /// <summary>
    /// Hides (or shows) the reticle based on the DisplayReticle setting
    /// </summary>
    protected virtual void HideReticle()
    {
        if (_reticle != null)
        {
            _reticle.gameObject.SetActive(DisplayReticle);
        }
    }
    /// <summary>
    /// On Destroy, we reinstate our cursor if needed
    /// </summary>
    protected void OnDestroy()
    {
        if (ReplaceMousePointer)
        {
            Cursor.visible = true;
        }
    }
    protected  void MoveTarget()
    {
        if (_weapon.Owner == null)
        {
            return;
        }





    }
}

}
