using UnityEngine;

/// <summary>
/// Implements a basic mouse utility class. It projects the mouse position from the 2D screen 
/// to the 3D world. Update the position of an object to show where the mouse cursor is (in 3D space)
/// </summary>
public class MouseUtil : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public static MouseUtil Instance;
    private GameObject mousePointer;
    private bool isMouseAvailable = true;
    
    /// <summary>
    /// Implements the Singleton pattern, ensure ther will be only one Static Instance of the MouseUtil class
    /// </summary>
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mousePointer = transform.GetChild(0).gameObject;
        } else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Uses the mouse right click to toggle the mouse visual. 
    /// Update the mouse position if it is visible
    /// </summary>
    public void Update()
    {
        if(Input.GetMouseButtonDown(1)) 
        {
            ToggleMouse();
        }
        if (isMouseAvailable)
        {
            mousePointer.SetActive(true);
            transform.position = GetMousePosition();
        } else
        {
            mousePointer.SetActive(false);
        }
    }

    /// <summary>
    /// Returns a Vector3 containing the mouse position projected from the screen into the 3D world
    /// </summary>
    /// <returns>Vector3: the mouse position</returns>
    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask);
        return hitInfo.point;
    }

    /// <summary>
    /// Check if the mouse is available and exposes the mouse position
    /// </summary>
    /// <param name="pos">exposes a Vector3 containing the mouse position as a parameter (local variable)</param>
    /// <returns>bool: true if the mouse is available</returns>
    public bool TryGetMousePosition(out Vector3 pos)
    {
        pos = GetMousePosition();
        return isMouseAvailable;
    }

    /// <summary>
    /// Check if the mouse is available (visible) on the screen
    /// </summary>
    /// <returns>bool: true if the mouse is available</returns>
    public bool IsMouseAvailable()
    {
        return isMouseAvailable;
    }

    /// <summary>
    /// Toggle the mouse visual (pointer)
    /// </summary>
    private void ToggleMouse()
    {
        isMouseAvailable = !isMouseAvailable;
    }
}