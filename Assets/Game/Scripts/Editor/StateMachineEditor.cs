using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StateMachineEditor : EditorWindow
{
    private StateMachineGraphView graphView;

    [MenuItem("Window/StateMachineEditor")]
    public static void OpenStateMachineEditor()
    {
        var window = GetWindow<StateMachineEditor>();
        window.titleContent = new GUIContent("State Machine Editor");
    }

    private void CreateGUI()
    {
        graphView = new StateMachineGraphView
        {
            name = "State Machine Graph"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }
}
