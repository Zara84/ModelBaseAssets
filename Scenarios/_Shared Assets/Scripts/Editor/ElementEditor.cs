using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimulationElementsEditor : OdinMenuEditorWindow
{
    EntityEditor entityEditor;
    ActionEditor actionEditor;
    NormEditor normEditor;
    GoalEditor goalEditor;

    protected override void OnEnable()
    {
        base.OnEnable();

        entityEditor = new EntityEditor();
        actionEditor = new ActionEditor();
        normEditor = new NormEditor();
        goalEditor = new GoalEditor();
    }

    [MenuItem("Model Builder/Element Editor")]
    public static void OpenWindow()
    {
        GetWindow<SimulationElementsEditor>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        tree.Add("Entities", entityEditor);
        tree.Add("Actions", actionEditor);
        tree.Add("Goals", goalEditor);
        tree.Add("Norms", normEditor);

        return tree;
    }
}

public class EntityEditor : OdinEditorWindow
{
    [BoxGroup("Select an entity archetype to edit")]
    [AssetSelector(Paths = "Assets/Scenarios/_Shared Assets/Archetypes")]
    [OnValueChanged("LoadEntity")]
    public mEntity entity;

    [ShowIf("@this.selected!=null")]
    [BoxGroup("Details")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public mEntity selected;

    public void LoadEntity()
    {
        selected = entity;
    }
}

public class ActionEditor : OdinEditorWindow
{

}

public class NormEditor : OdinEditorWindow
{

}

public class GoalEditor : OdinEditorWindow
{

}
