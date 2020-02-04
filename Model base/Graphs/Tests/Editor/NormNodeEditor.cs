using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(NormNode))]
public class NormNodeEditor : SimpleNodeEditor
{
    public NormNode node = null;

    public override void OnHeaderGUI()
    {
        if (node == null)
            node = target as NormNode;

        if (node.sourceNorm != null)
        {
            GUILayout.Label(node.sourceNorm.GetType().ToString(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
        else
        {
            inspectStuff.Show();
            OdinEditorWindow.InspectObject(inspectStuff, node);
            //window.Repaint();
        }

        if (node == null)
            node = target as NormNode;

        if (node.context == null)
        {
            node.initNode();
            window.Repaint();
        }
    }

    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        GUILayout.Label("this is a norm");

        if (node.context != null)
        {
            drawFilter(node.context);
        }
    }
}
