using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
public class ButtonText : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Public Properties
    public TextMeshProUGUI targetText;

    public Color hoverTextColor;
    public Color pressedTextColor;

    public UnityEvent OnClick;
    #endregion
    //--------------------------------------------------------------------------------
    #region Private Properties
    Sprite normalSprite;
    Color normalTextColor;
    bool tracking;
    bool inBounds;
    #endregion
    //--------------------------------------------------------------------------------
    #region Interface Methods
    void Start()
    {
        targetText = GetComponent<TextMeshProUGUI>();
        normalTextColor = targetText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inBounds = true;
        UpdateStyle();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inBounds = false;
        UpdateStyle();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tracking = true;
        inBounds = true;
        UpdateStyle();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (tracking && inBounds && OnClick != null) OnClick.Invoke();
        tracking = false;
        inBounds = false;
        UpdateStyle();
    }
    #endregion
    //--------------------------------------------------------------------------------
    #region Private Methods
    void Set(Color textColor)
    {
        targetText.color = textColor;
    }

    void UpdateStyle()
    {
        if (!inBounds)
        {
            Set(normalTextColor);
        }
        else if (tracking)
        {
            Set(pressedTextColor);
        }
        else
        {
            Set(hoverTextColor);
        }
    }
    #endregion
}