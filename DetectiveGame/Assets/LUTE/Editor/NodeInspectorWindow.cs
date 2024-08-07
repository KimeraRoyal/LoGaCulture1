using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class NodeInspectorWindow : ScriptableObject
{
    public Node node;
}

[CustomEditor(typeof(NodeInspectorWindow), true)]
public class NodeInspectorEditor : Editor
{
    protected Vector2 nodeScrollPos;
    protected Vector2 orderScrollPos;
    protected bool resize = false;
    protected bool clamp = false;

    protected float windowHeight = 0;
    protected float topPanelHeight = 2;

    protected NodeEditor nodeEditor;
    protected OrderEditor orderEditor;
    protected Order activeOrder;
    protected static List<OrderEditor> cachedEditors = new List<OrderEditor>();

    protected void OnDestroy()
    {
        ClearEditors();
    }

    protected void OnEnable()
    {
        ClearEditors();
    }

    protected void OnDisable()
    {
        ClearEditors();
    }

    protected void ClearEditors()
    {
        //should destroy all cached editors here then clear that list
        foreach (OrderEditor editor in cachedEditors)
        {
            DestroyImmediate(editor);
        }
        cachedEditors.Clear();
        orderEditor = null;
    }

    public override void OnInspectorGUI()
    {
        NodeInspectorWindow inspectorWindow = target as NodeInspectorWindow;

        if (inspectorWindow.node == null)
        {
            return;
        }

        var node = inspectorWindow.node;
        if (node == null)
        {
            return;
        }

        var engine = (BasicFlowEngine)node.GetEngine();

        if (engine.SelectedNodes.Count > 1)
        {
            GUILayout.Label("Multiple nodes selected");
            return;
        }

        if (nodeEditor == null || !node.Equals(nodeEditor.target))
        {
            DestroyImmediate(nodeEditor);
            nodeEditor = Editor.CreateEditor(node, typeof(NodeEditor)) as NodeEditor;
        }

        UpdateWindowHeight();
        float width = EditorGUIUtility.currentViewWidth;

        nodeScrollPos = GUILayout.BeginScrollView(nodeScrollPos, GUILayout.Height(engine.NodeViewHeight));
        //draw node editor name
        nodeEditor.DrawNodeName(engine);
        //draw node editor GUI stuff
        nodeEditor.DrawNodeGUI(engine);
        GUILayout.EndScrollView();

        Order inspectOrder = null;
        if (engine.SelectedOrders.Count == 1)
        {
            inspectOrder = engine.SelectedOrders[0];
        }

        if (Application.isPlaying && inspectOrder != null && !inspectOrder.ParentNode.Equals(node))
        {
            Repaint();
            return;
        }

        if (Event.current.type == EventType.Layout)
        {
            activeOrder = inspectOrder;
        }

        DrawOrderUI(engine, inspectOrder);
    }

    public void DrawOrderUI(BasicFlowEngine engine, Order inspectOrder)
    {
        ResizeScrollView(engine);

        EditorGUILayout.Space();

        nodeEditor.DrawNodeToolBar();

        orderScrollPos = GUILayout.BeginScrollView(orderScrollPos);

        if (inspectOrder != null)
        {
            if (orderEditor == null || !inspectOrder.Equals(orderEditor.target))
            {
                var editors = from e in cachedEditors where e != null && e.target.Equals(inspectOrder) select e;
                if (editors.Count() > 0)
                {
                    orderEditor = editors.First();
                }
                else
                {
                    orderEditor = Editor.CreateEditor((Order)inspectOrder) as OrderEditor;
                    cachedEditors.Add(orderEditor);
                }
            }
            if (orderEditor != null)
            {
                orderEditor.DrawOrderInpsectorGUI();
            }
        }

        GUILayout.EndScrollView();

        // Draw the resize bar after everything else has finished drawing
        // This is mainly to avoid incorrect indenting.
        Rect resizeRect = new Rect(0, engine.NodeViewHeight + topPanelHeight, EditorGUIUtility.currentViewWidth, 4f);
        GUI.color = new Color(0.64f, 0.64f, 0.64f);
        GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
        resizeRect.height = 1;
        GUI.color = new Color32(132, 132, 132, 255);
        GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
        resizeRect.y += 3;
        GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;

        Repaint();
    }

    private void ResizeScrollView(BasicFlowEngine engine)
    {
        Rect cursorChangeRect = new Rect(0, engine.NodeViewHeight + 1 + topPanelHeight, EditorGUIUtility.currentViewWidth, 4f);

        EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

        if (cursorChangeRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                resize = true;
            }
        }

        if (resize && Event.current.type == EventType.Repaint)
        {
            //add a undo event here if you like
            engine.NodeViewHeight = Event.current.mousePosition.y - topPanelHeight;
        }

        ClampNodeViewHeight(engine);

        if (resize && Event.current.type == EventType.MouseDrag)
        {
            Rect windowRect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, windowHeight);
            if (!windowRect.Contains(Event.current.mousePosition))
            {
                resize = false;
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {
            resize = false;
        }
    }

    private void ClampNodeViewHeight(BasicFlowEngine engine)
    {
        // Screen.height seems to temporarily reset to 480 for a single frame whenever a command like 
        // Copy, Paste, etc. happens. Only clamp the block view height when one of these operations is not occuring.
        if (Event.current.commandName != "")
            clamp = false;

        if (clamp)
        {
            //make sure node view is clamped to visible area
            float height = engine.NodeViewHeight;
            height = Mathf.Max(200, height);
            height = Mathf.Min(windowHeight - 200, height);
            engine.NodeViewHeight = height;
        }

        if (Event.current.type == EventType.Repaint)
            clamp = true;
    }

    /// <summary>
    /// In Unity 5.4, Screen.height returns the pixel height instead of the point height
    /// of the inspector window. We can use EditorGUIUtility.currentViewWidth to get the window width
    /// but we have to use this horrible hack to find the window height.
    /// For one frame the windowheight will be 0, but it doesn't seem to be noticeable.
    /// </summary>
    protected void UpdateWindowHeight()
    {
        windowHeight = Screen.height * EditorGUIUtility.pixelsPerPoint;
    }
}
