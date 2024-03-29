using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public  Canvas TargetCanvas { get; set; }
    protected Vector2 _newPosition;
    protected Vector2 _mousePosition;

    protected virtual void LateUpdate()
    {
#if !ENABLE_INPUT_SYSTEM || ENABLE_LEGACY_INPUT_MANAGER
        _mousePosition = Input.mousePosition;
#endif
        RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetCanvas.transform as RectTransform, _mousePosition, TargetCanvas.worldCamera, out _newPosition);
        transform.position = TargetCanvas.transform.TransformPoint(_newPosition);
    }
}
