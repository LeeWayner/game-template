using System;
using System.Collections;
using System.Collections.Generic;
using MyTool.Core.Runtime.Singleton;
using MyTool.Core.Runtime.Utils;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HandController : Singleton<HandController>
{
//    private readonly int TAP_ANIM = Animator.StringToHash("tap");
//    private readonly int HOLD_ANIM = Animator.StringToHash("hold");
//    private readonly int START_HOLD_ANIM = Animator.StringToHash("start_hold");
//    private readonly int END_HOLD_ANIM = Animator.StringToHash("end_hold");
    public static bool isShowHand;
    public static bool isHandShadow;
    public static bool isHandFollowMouse;
    public static Vector2 handOffset;
    public static float handRotation;
    public static bool isFrame16x9;
    public static Vector2Int frameSize = new Vector2Int(9, 16);
    public ParticleSystem clickEff;
    public static int handTypeIndex { get; set; } = 1;
    [SerializeField] private RectTransform handTrans;
    [SerializeField] private Image handImg;

    [SerializeField] private Shadow handShadow;
    [SerializeField] private List<Sprite> fingerUpSprites;
    [SerializeField] private List<Sprite> fingerDownSprites;
    
    /// <summary>
    /// thời gian chờ để ẩn tay sau khi tap
    /// </summary>
    [SerializeField] private float tapTime;

    private RectTransform rectTrans;
    private bool isHolding;
    private Canvas canvas;
    private Rect frameRect = new Rect();
    public override void Init()
    {
    }

    private void Start()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        rectTrans = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        handShadow.enabled = isShowHand;
        handTrans.gameObject.SetActive(isHandFollowMouse);
        handShadow.enabled = isHandShadow;
        handImg.transform.localRotation = Quaternion.Euler(0, 0, handRotation);
        SetHandType(handTypeIndex);
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    }

    private void Update()
    {
        if (!isHolding && isShowHand && isHandFollowMouse)
        {
            Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 screenPoint = new Vector2(viewportPos.x * canvas.pixelRect.width,
                viewportPos.y * canvas.pixelRect.height);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, screenPoint, null,
                out Vector2 localPos))
            {
                handTrans.anchoredPosition = localPos + handOffset;
            }
        }
    }

    private void OnFingerUpdate(LeanFinger fingerInfo)
    {
        UpdateHandPos(fingerInfo.ScreenPosition);
    }

    private void OnFingerUp(LeanFinger fingerInfo)
    {
        HandUp(fingerInfo.ScreenPosition);
    }

    private void OnFingerDown(LeanFinger fingerInfo)
    {
        HandDown(fingerInfo.ScreenPosition);
    }
    
    public void SetHandFollowMouse(bool _isHandFollowMouse)
    {
        isHandFollowMouse = _isHandFollowMouse;
        handTrans.gameObject.SetActive(isHandFollowMouse);
    }

    public void SetHandShadow(bool isShadow)
    {
        isHandShadow = isShadow;
        handShadow.enabled = isHandShadow;
    }

    public void SetHandRotation(int rotation)
    {
        handImg.transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    public void SetHandType(int handType)
    {
        handTypeIndex = handType;
        handTypeIndex = (handTypeIndex - 1) % fingerUpSprites.Count;
        Sprite fingerDownSprite = fingerDownSprites[handTypeIndex];
        handImg.sprite = fingerDownSprite;
        handImg.rectTransform.pivot = new Vector2(fingerDownSprite.pivot.x / fingerDownSprite.rect.width,
            fingerDownSprite.pivot.y / fingerDownSprite.rect.height);
        ;
        handImg.rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// gọi trong sự kiện finger down
    /// </summary>
    /// <param name="screenPoint"></param>
    public void HandDown(Vector2 screenPoint)
    {
        if (!isShowHand)
        {
            return;
        }

        isHolding = true;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, screenPoint, null, out Vector2 localPos))
        {
            handTrans.gameObject.SetActive(true);
            handTrans.anchoredPosition = localPos + handOffset;
            StartCoroutine(CoHandDown());
        }
    }

    /// <summary>
    /// gọi trong sự kiện fingerUpdate, khi finger đang ấn trên màn hình
    /// </summary>
    /// <param name="screenPoint"></param>
    public void UpdateHandPos(Vector2 screenPoint)
    {
        if (!isShowHand)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, screenPoint, null, out Vector2 localPos))
        {
            handTrans.anchoredPosition = localPos + handOffset;
        }
    }

    /// <summary>
    /// gọi trong sự kiện finger up
    /// </summary>
    /// <param name="screenPoint"></param>
    public void HandUp(Vector2 screenPoint)
    {
        if (!isShowHand)
        {
            return;
        }

        isHolding = false;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, screenPoint, null, out Vector2 localPos))
        {
            handTrans.anchoredPosition = localPos + handOffset;
            StartCoroutine(CoHandUp());
        }
    }

    private WaitForSeconds wait0p1Sec = new WaitForSeconds(0.1f);
    private IEnumerator CoHandDown()
    {
        handImg.sprite = fingerUpSprites[handTypeIndex];
        yield return wait0p1Sec;
        handImg.sprite = fingerDownSprites[handTypeIndex];
        clickEff.transform.position = handTrans.position;
        clickEff.Play();
        yield return new WaitForSeconds(tapTime);
    }

    private IEnumerator CoHandUp()
    {
        yield return wait0p1Sec;
        handImg.sprite = fingerUpSprites[handTypeIndex];
        yield return new WaitForSeconds(tapTime);
        if (!isHandFollowMouse)
        {
            handTrans.gameObject.SetActive(false);
        }
    }

    private IEnumerator CoHandTap(Vector2 localPos)
    {
        handTrans.anchoredPosition = localPos;
        handTrans.gameObject.SetActive(true);
        handImg.sprite = fingerUpSprites[handTypeIndex];
        yield return wait0p1Sec;
        handImg.sprite = fingerDownSprites[handTypeIndex];
        yield return wait0p1Sec;
        handImg.sprite = fingerUpSprites[handTypeIndex];
        yield return new WaitForSeconds(tapTime);
        if (!isHandFollowMouse)
        {
            handTrans.gameObject.SetActive(false);
        }
    }

    #region Draw Frame

    public void CalculateFrameRect()
    {
        int unit = Screen.height / frameSize.y;
        int width = unit * frameSize.x;
        int x = (Screen.width - width) / 2;
        frameRect = new Rect(x, 0, width, Screen.height);
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isFrame16x9 && frameRect.x >= 0)
        {
            CalculateFrameRect();
            float lineWidth = 10;
            Drawing.DrawLine(new Vector2(frameRect.xMin, frameRect.yMin), new Vector2(frameRect.xMin, frameRect.yMax), Color.red, lineWidth);
            Drawing.DrawLine(new Vector2(frameRect.xMax, frameRect.yMax), new Vector2(frameRect.xMax, frameRect.yMin), Color.red, lineWidth);
        }
    }

    #endregion
}