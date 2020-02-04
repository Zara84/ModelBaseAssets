using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class AgentProfileEditor : OdinEditorWindow
{
   [MenuItem("Model Builder/Agent Profile")]
   public static void OpenWindow()
    {
        GetWindow<AgentProfileEditor>().Show();
    }

    [PreviewField]
    public mEntity profile;


}
