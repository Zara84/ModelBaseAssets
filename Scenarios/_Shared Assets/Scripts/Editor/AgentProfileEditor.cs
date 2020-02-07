using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Helpers;
using ProfileFlags;

public class AgentProfileEditor : OdinEditorWindow
{
   // [Title("Agent")]

   // [OnValueChanged("LoadProfile")]
   // [VerticalGroup]
   // [HorizontalGroup("One", 0.4f)]
    [BoxGroup("Agent Profile")]
    [AssetList(CustomFilterMethod ="GetAgentProfiles")]
    [OnValueChanged("LoadProfile")]
    public ProfileEntity profile;

    [BoxGroup("Details")]
    
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    [OnValueChanged("LoadProfile")]
    public ProfileEntity selected;
    

    //public List<mEntity> entities = new List<mEntity>();
    //public ActionListComponent actions = new ActionListComponent();
    //public GoalListComponent goals = new GoalListComponent();
    //public NormListComponent norms = new NormListComponent();
    // public TestMenu testmenu = new TestMenu();

    public void LoadProfile()
    {
        Debug.Log("Loading profile");
        selected = profile;
    }

    public bool GetAgentProfiles(ProfileEntity e)
    {
        return (e.getComponent<AgentProfileFlag>() != null);
    }
}