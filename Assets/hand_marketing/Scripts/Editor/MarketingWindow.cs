using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BaseMarketingWindow : OdinEditorWindow
{
    protected override void Initialize()
    {
        OnShowHandChanged();
        OnHandFollowHandChanged();
        OnHandShadowChanged();
        OnHandTypeChanged();
        OnHandRotationChanged();
        OnHandOffsetChanged();
        OnFrame16x9Changed();
        OnSoundChanged();
    }

    protected bool IsApplicationPlaying()
    {
        return Application.isPlaying;
    }

    #region Hand

    [FoldoutGroup("Hand")] [OnValueChanged("OnShowHandChanged")]
    public bool ShowHand;

    private void OnShowHandChanged()
    {
        HandController.isShowHand = ShowHand;
    }

    [FoldoutGroup("Hand")] [OnValueChanged("OnHandFollowHandChanged")]
    public bool HandFollowMouse;

    private void OnHandFollowHandChanged()
    {
        HandController.isHandFollowMouse = HandFollowMouse;
        if (IsApplicationPlaying())
        {
            HandController.Instance.SetHandFollowMouse(HandFollowMouse);
        }
    }

    [FoldoutGroup("Hand")] [OnValueChanged("OnHandShadowChanged")]
    public bool HandShadow;

    private void OnHandShadowChanged()
    {
        HandController.isHandShadow = HandShadow;
        if (IsApplicationPlaying())
        {
            HandController.Instance.SetHandShadow(HandShadow);
        }
    }

    [FoldoutGroup("Hand")] [OnValueChanged("OnHandTypeChanged")]
    public int handType = 1;

    private void OnHandTypeChanged()
    {
        if (handType <= 0)
        {
            handType = 1;
        }

        HandController.handTypeIndex = handType;
        if (IsApplicationPlaying())
        {
            HandController.Instance.SetHandType(handType);
        }
    }

    [FoldoutGroup("Hand")] [OnValueChanged("OnHandRotationChanged")]
    public int handRotation;

    private void OnHandRotationChanged()
    {
        HandController.handRotation = handRotation;
        if (IsApplicationPlaying())
        {
            HandController.Instance.SetHandRotation(handRotation);
        }
    }

    [FoldoutGroup("Hand")] [OnValueChanged("OnHandOffsetChanged")]
    public Vector2 handOffset;

    private void OnHandOffsetChanged()
    {
        HandController.handOffset = handOffset;
    }
    
    [FoldoutGroup("Hand")]  [OnValueChanged("OnFrame16x9Changed")]
    public bool frame16x9;

    private void OnFrame16x9Changed()
    {
        HandController.isFrame16x9 = frame16x9;
    }
    
    [FoldoutGroup("Setting")]  [OnValueChanged("OnSoundChanged")]
    public bool soundOn;

    private void OnSoundChanged()
    {
        EditorUtility.audioMasterMute = !soundOn;
    }

    #endregion
}