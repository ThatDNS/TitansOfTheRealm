using Unity.VisualScripting;
using UnityEngine;


public class WeaponAim : MonoBehaviour
{



    private Camera _mainCamera;
    [SerializeField]private Weapon _weapon;


    protected Vector3 _weaponAimCurrentAim;

    [SerializeField]private GameObject _reticle;

    private Vector3 _mousePosition;
    private Vector3 _lastMousePosition;
    private Vector3 _direction;

    [Tooltip("the radius around the weapon rotation centre where the mouse will be ignored, to avoid glitches")]
    public float MouseDeadZoneRadius = 0.5f;

    public Canvas _targetCanvas;


    [Tooltip("the gameobject to display as the aim's reticle/crosshair. Leave it blank if you don't want a reticle")]
    public GameObject Reticle;

    [Tooltip("if set to true, the reticle will replace the mouse pointer")]
    public bool ReplaceMousePointer = true;

    [Tooltip("if set to false, the reticle won't be added and displayed")]
    public bool DisplayReticle = true;
    /// <summary>
    /// On Start(), we trigger the initialization
    /// </summary>
    private void Start()
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

        _targetCanvas = _weapon.Owner.GetComponent<Character>().MainCanvas;

        InitializeReticle();

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

        _reticle = Instantiate(Reticle);
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
    private void MoveReticle()
    {
        if (_reticle == null) { return; }

  


    }
    private void GetCurrentAim()
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
        DebugExtension.DebugArrow(ray.origin,ray.direction * 100, Color.red);
        Vector3 target = ray.direction*100;

        _direction = target;

        _weaponAimCurrentAim = _direction - _weapon.transform.position;
    }

    /// <summary>
    /// Every frame, we compute the aim direction and rotate the weapon accordingly
    /// </summary>
    private void Update()
    {
        HideMousePointer();
        HideReticle();
        GetCurrentAim();
        DetermineWeaponRotation(this.transform.position + _weaponAimCurrentAim);
    }

    private void FixedUpdate()
    {
        MoveReticle();
    }


    /// <summary>
    /// Hides or show the mouse pointer based on the settings
    /// </summary>
    private  void HideMousePointer()
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
    private  void HideReticle()
    {
        if (_reticle != null)
        {
            _reticle.gameObject.SetActive(DisplayReticle);
        }
    }
    /// <summary>
    /// On Destroy, we reinstate our cursor if needed
    /// </summary>
    private void OnDestroy()
    {
        if (ReplaceMousePointer)
        {
            Cursor.visible = true;
        }
    }

    private Vector3 _aimAtDirection;
    public Quaternion _aimAtQuaternion;
    protected void DetermineWeaponRotation(Vector3 target)
    {
        _aimAtDirection = target - transform.position;
        _aimAtQuaternion = Quaternion.LookRotation(_aimAtDirection, Vector3.up);
        transform.rotation = _aimAtQuaternion;
    }

}
