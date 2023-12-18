using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Image XImage;
    public ActionTypes.ActionsEnum type;

    public bool isEnabled = true;

    public void SetDisabled(bool isDisabled)
    {
        isEnabled = !isDisabled;
        XImage.enabled = isDisabled;
    }

    public void OnButtonClick()
    {
        if (!isEnabled) return;
        GameController.Instance.DisableItem(type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnabled) return;
        XImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isEnabled) return;
        XImage.enabled = false;
    }
}
