using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionTypes;

public class ActionUIController : MonoBehaviour
{
    [SerializeField] List<ActionIconController> actionIconControllers;

    [SerializeField] ActionIconController currentActive;

    public static ActionUIController Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        actionIconControllers = GetComponentsInChildren<ActionIconController>().ToList();
        currentActive = actionIconControllers.First();
    }

    public void SetActive(ActionsEnum type)
    {
        currentActive.SetActive(false);
        currentActive = actionIconControllers.Where(controller => controller.actionType == type).First();
        currentActive.SetActive(true);
    }

    public void DisableItem(ActionsEnum type)
    {
        actionIconControllers.Where(controller => controller.actionType == type).First().DisableItem();
    }
}
