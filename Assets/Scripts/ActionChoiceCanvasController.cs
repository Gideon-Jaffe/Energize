using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionTypes;

public class ActionChoiceCanvasController : MonoBehaviour
{
    List<ActionChoice> allOptions;

    void Awake()
    {
        allOptions = GetComponentsInChildren<ActionChoice>().ToList();
    }

    public void Initialize(List<ActionsEnum> possibleOptions)
    {
        foreach (var choice in allOptions)
        {
            choice.SetDisabled(!possibleOptions.Contains(choice.type));
        }
    }
}
