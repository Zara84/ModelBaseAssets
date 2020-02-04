using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using XNode;
using UnityEngine.UI;
using System;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[CustomNodeEditor(typeof(CognitiveElementNode))]
public class SimpleNodeEditor : NodeEditor
{
   // public Button resize;
    private NewNode simpleNode;
    public Type type;
    private int width = 180;
    private bool resizing;
    public Texture resizeTex = Resources.Load("Textures/RedCircle3") as Texture;
    public Vector2 startResizeMousePosition = new Vector2();
    bool showPosition = false;
    string status = "closed";

    mEntity test = new mEntity();
   // IComponent testC = new Vessel.BaseQuota();
    [Indent]
    PropertyTree tree;

    string debug = "not there";

    public GUIStyle resizeLabel;

    Color idleColor = Color.black, hoveredColor = Color.black, currentColor = Color.black;

    public OdinEditorWindow inspectStuff = new OdinEditorWindow();


    void OnAwake()
    {
       // resizeTex = Resources.Load("Textures/RedCircle") as Texture;
    }

    public override int GetWidth()
    {
        
        return width;
    }

    public override void OnHeaderGUI()
    {
        GUILayout.Label(target.GetType().ToString(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        highlight((Node)target);

        type = target.GetType();
        var thisNode = target;
        Convert.ChangeType(thisNode, type);

        resizeLabel = GUI.skin.label;
        resizeLabel.imagePosition = ImagePosition.ImageLeft;

        Rect rt = GUILayoutUtility.GetRect(15, 15);
        rt.position = new Vector2(width - (15 + 15/2), 15/2);
        
        GUI.Label(rt, resizeTex);

        Event e = Event.current;
        if (resizing)
        {
            width += (int)(e.mousePosition.x - startResizeMousePosition.x);
            // debug = width.ToString();
            startResizeMousePosition = e.mousePosition;
            window.Repaint();
        }
        if (e.type == EventType.MouseUp)
        {
            if (resizing)
                resizing = false;
        }

        if (rt.Contains(e.mousePosition))
        {
            NodeEditorWindow.currentActivity = NodeEditorWindow.NodeActivity.Idle;
            //Debug.Log(e.type.ToString());
           // e.Use();
            switch (e.type)
            {
                
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        //Debug.Log("Mouse down");
                        if (NodeEditorWindow.currentActivity == NodeEditorWindow.NodeActivity.DragNode)
                            break;

                        startResizeMousePosition = e.mousePosition;
                        resizing = true;
                    }
                    break;

                case EventType.MouseDrag:
                    if (resizing)
                    {
                       // NodeEditorWindow.currentActivity = NodeEditorWindow.NodeActivity.Idle;
                       // NodeEditorWindow.isPanning = false;
                        width += (int)(e.mousePosition.x - startResizeMousePosition.x);
                       // debug = width.ToString();
                        startResizeMousePosition = e.mousePosition;
                        window.Repaint();
                        //  NodeEditorWindow.RepaintAll();
                        //Repaint();
                    }
                    break;

                case EventType.MouseUp:
                    if (resizing)
                        resizing = false;
                    break;
            }
        }
    }

    public void resizeWidth()
    {
        width += 10;
    }

    public void resizeWidth(int i)
    {
        width += i;
    }

    [Button("Meh")]
    public void setWidth(int i)
    {
        width = i;
    }

    public override Color GetTint()
    {
        if (idleColor == Color.black)
            idleColor = base.GetTint();

        if (currentColor == Color.black)
            currentColor = idleColor;

        if (hoveredColor == Color.black)
            hoveredColor = new Color(idleColor.r, idleColor.g, idleColor.b, idleColor.a + 0.05f);

        return currentColor;
    }

    public void highlight(Node node)
    {
        if (NodeEditorWindow.current.hoveredNode == node)
        {
            if (currentColor != hoveredColor)
            {
                currentColor = hoveredColor;
               // GUI.color = currentColor;
                window.Repaint();
            }
        }

        else
            if (currentColor != idleColor)
                currentColor = idleColor;
    }

    public void drawFilter(NodeFilter filter)
    {
        if (filter != null)
        {
            if (filter.orientation == PortOrientation.In)
            {
                SirenixEditorGUI.BeginBox();
                Rect rect = EditorGUILayout.GetControlRect();
                rect = EditorGUI.PrefixLabel(rect, new GUIContent("In filter"));
                if (GUI.Button(rect.AlignRight(15), EditorIcons.ArrowDown.Raw))
                {
                    filter.style.unfolded = !filter.style.unfolded;
                };
                SirenixEditorGUI.EndBox();
                // NodeEditorGUILayout.AddPortField(filter.port);

            }
            else
            { 
                SirenixEditorGUI.BeginBox();
                Rect rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(rect.AlignRight(60), "Out filter");
                if (GUI.Button(rect.AlignLeft(15), EditorIcons.ArrowDown.Raw))
                {
                    filter.style.unfolded = !filter.style.unfolded;
                };
                SirenixEditorGUI.EndBox();
            }

            // NodeEditorGUILayout.


            if (filter.style.unfolded)
            {
                SirenixEditorGUI.BeginBox();
                for (int i = 0; i < filter.filter.Count; i++)
                {
                    drawEntity(filter.filter[i]);
                }
                SirenixEditorGUI.EndBox();
            }
            else
            {
                NodeEditorGUILayout.AddPortField(filter.port);
            }
        }
        
    }


    public void drawEntity(NodeMEntity entity)
    {
        string name = "Entity";

        if (entity.entityName != "")
            name = entity.entityName;

        if (entity.orientation == PortOrientation.In)
        {
            //SirenixEditorGUI.
            SirenixEditorGUI.BeginIndentedHorizontal();
            SirenixEditorGUI.BeginBox();
            Rect rect = EditorGUILayout.GetControlRect();
            //  rect = EditorGUI.PrefixLabel(rect, new GUIContent("   "));
            

            rect = EditorGUI.PrefixLabel(rect, new GUIContent(name));

            if (GUI.Button(rect.AlignRight(15), EditorIcons.ArrowDown.Raw))
            {
                entity.style.unfolded = !entity.style.unfolded;
            };
            SirenixEditorGUI.EndBox();
            SirenixEditorGUI.EndIndentedHorizontal();
            // NodeEditorGUILayout.AddPortField(entity.port);
        }
        else
        {
            SirenixEditorGUI.BeginBox();
            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(rect.AlignRight(80), name);
            if (GUI.Button(rect.AlignLeft(15), EditorIcons.ArrowDown.Raw))
            {
                entity.style.unfolded = !entity.style.unfolded;
            };

            SirenixEditorGUI.EndBox();
            //  NodeEditorGUILayout.AddPortField(entity.port);
        }
        //  entity.style.unfolded = SirenixEditorGUI.Foldout(entity.style.unfolded, "");


        if (entity.style.unfolded)
        {

            SirenixEditorGUI.BeginBox();
            for (int i = 0; i < entity.components.Count; i++)
            {
                drawComponent(entity.components[i]);
            }
            SirenixEditorGUI.EndBox();
        }
        else
        {
            NodeEditorGUILayout.AddPortField(entity.port);
        }
    }

    public void drawComponent(NodeIComponent component)
    {
        SirenixEditorGUI.BeginBoxHeader();

        component.style.unfolded = SirenixEditorGUI.Foldout(component.style.unfolded, component.component.GetType().ToString());
        Rect rect = GUILayoutUtility.GetLastRect();
        SirenixEditorGUI.EndBoxHeader();

        NodeEditorGUILayout.AddPortField(component.port);

        if (component.style.unfolded)
        {
            SirenixEditorGUI.BeginBox();
            if (component.tree == null)
            {
                GUILayout.Label("the tree is null somehow");
            }
            if (component.tree != null)
                component.tree.Draw(false);
            else
            {
                component.tree = PropertyTree.Create(component.component);
                component.tree.Draw(false);
            }
                //Debug.Log(component.tree.WeakTargets);
            SirenixEditorGUI.EndBox();

        }
    }

}
