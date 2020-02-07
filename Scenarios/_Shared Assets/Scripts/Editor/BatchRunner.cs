using GeneralComponents;
using Helpers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimulationSetupEditor : OdinMenuEditorWindow
{
    [MenuItem("Model Builder/Simulation Setup")]
    public static void OpenWindow()
    {
        GetWindow<SimulationSetupEditor>().Show();
    }

    AgentProfileEditor agentEditor ;
    VesselProfileEditor vesselEditor;
    CommunityProfileEditor communityEditor;

    protected override void OnEnable()
    {
        base.OnEnable();

        agentEditor = new AgentProfileEditor();
        vesselEditor = new VesselProfileEditor();
        communityEditor = new CommunityProfileEditor();
    }
    ProfileEntity profile;
    mEntity vesselProfile;

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

       // tree.Add("Communities", )
        tree.Add("Agents", agentEditor);
        tree.Add("Vessels", vesselEditor);
        tree.Add("Communities", communityEditor);
        tree.Add("Test", new TestMenu());

        return tree;
    }

    [Button("Build simulation profile")]
    public void test()
    {
        Debug.Log("Building simulation profile...");
    }
}



public class VesselProfileEditor : OdinEditorWindow
{
    [Title("Vessel")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public mEntity profile;
}

public class TestMenu : OdinEditorWindow
{

    public static void OpenWindow()
    {
        GetWindow<TestMenu>().Show();
    }

    [Button("Create New")]
    public void createNew()
    {
        Debug.Log("Creating new...");
        TestMenu menu = new TestMenu();
        menu.Show();
    }
}


