using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class MarketingWindow : BaseMarketingWindow
{
    [MenuItem("Big Bear/Marketing")]
    private static void OpenWindow()
    {
        var window = GetWindow<MarketingWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }
    
    [FoldoutGroup("Level")]
    [HorizontalGroup("Level/Button")]
    [EnableIf("@this.IsApplicationPlaying()")]
    [Button("<--",ButtonSizes.Large)]
    public void PreviousLevel()
    {
        //TODO
    }
    
    [FoldoutGroup("Level")]
    [HorizontalGroup("Level/Button")]
    [EnableIf("@this.IsApplicationPlaying()")]
    [Button("-->", ButtonSizes.Large)]
    public void NextLevel()
    {
        //TODO
    }


}