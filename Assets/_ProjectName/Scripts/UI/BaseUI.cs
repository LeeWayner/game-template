using DoozyUI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIElement))]
public class BaseUI : MonoBehaviour
{
	[SerializeField]
	protected UIElement uiElement;

	public UIElement UiElement { get => uiElement; }

	private void OnValidate()
	{
		uiElement = GetComponent<UIElement>();
	}

	public bool IsVisible()
	{
		return uiElement.isVisible;
	}
	public virtual void OnInAnimationStart() { }

	public virtual void OnInAnimationFinish() { }

	public virtual void OnOutAnimationStart() { }

	public virtual void OnOutAnimationFinish() { }

	public void CloseUIElement()
	{
		uiElement.Hide(false);
	}

	[Button("Show")]
	public void Show()
	{
		uiElement.Show(false);
	}

	public IEnumerator WaitToShow(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Show();
	}
	
	[Button("Hide")]
	public void Hide()
	{
		uiElement.Hide(false);
	}
}
