using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityProfileEditor : OdinEditorWindow
{
    [TitleGroup("Communities")]
    [TabGroup("Blue")]
    public mEntity blue;

    [TabGroup("Greeen")]
    public mEntity green;
    [TabGroup("Red")]
    public mEntity red;

}
