using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;



public class StateMachineGraphView : GraphView
{
    public StateMachineGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(CreateStateNode("IdleState", new Vector2(100, 200)));
        AddElement(CreateStateNode("WalkingState", new Vector2(300, 200)));
        // Add more states as needed
    }

    public Node CreateStateNode(string stateName, Vector2 position)
    {
        var node = new Node
        {
            title = stateName,
            style = { top = position.y, left = position.x }
        };

        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);

        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        outputPort.portName = "Output";
        node.outputContainer.Add(outputPort);

        node.RefreshPorts();
        node.RefreshExpandedState();
        node.SetPosition(new Rect(position, Vector2.zero));
        return node;
    }

    // You can add methods to create edges, handle node interactions, etc.
}
