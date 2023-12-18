using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ActionTypes;

public class ActionIconController : MonoBehaviour
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite ExsSprite;

    private bool disabled = false;

    public ActionsEnum actionType;
    
    void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        if (disabled) return;
        GetComponent<Image>().sprite = isActive? onSprite : offSprite;
    }

    public void DisableItem()
    {
        disabled = true;
        GetComponent<Image>().sprite = ExsSprite;
    }
}
