using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ActionTypes;

public class ActionController : MonoBehaviour
{
    [SerializeField] private float specialJumpForce;
    [SerializeField] private GameObject RocketPrefab;
    [SerializeField] private GameObject InverterPrefab;
    [SerializeField] private InputActionReference performAction;
    [SerializeField] private InputActionReference switchCurrentAction;
    [SerializeField] private ActionUIController uIController;
    
    private PlayerMovement playerMovement;

    public int currentAction = 0;
    public List<ActionsEnum> possibleActions;

    public static ActionController Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Instance.transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        possibleActions.Add(ActionsEnum.TELEPORTER);
        possibleActions.Add(ActionsEnum.ROCKET_LAUNCHER);
        possibleActions.Add(ActionsEnum.SUPERJUMP);
        possibleActions.Add(ActionsEnum.GRAVITY_SWITCHER);
        possibleActions.Add(ActionsEnum.INVERTER);
        uIController.SetActive(possibleActions[currentAction]);
    }

    void OnEnable()
    {
        performAction.action.started += PerformAction;
        switchCurrentAction.action.started += SwitchCurrentAction;
    }

    void OnDisable()
    {
        performAction.action.started -= PerformAction;
        switchCurrentAction.action.started -= SwitchCurrentAction;
    }

    private void PerformAction(InputAction.CallbackContext callbackContext)
    {
        Debug.Log(possibleActions[currentAction]);
        switch (possibleActions[currentAction])
        {
            case ActionsEnum.SUPERJUMP:
                SpecialJump();
                break;
            case ActionsEnum.TELEPORTER:
                Teleport();
                break;
            case ActionsEnum.ROCKET_LAUNCHER:
                FireRocket();
                break;
            case ActionsEnum.INVERTER:
                FireInverter();
                break;
            case ActionsEnum.GRAVITY_SWITCHER:
                SwitchGravity();
                break;
            default:
                break;
        }
    }

    private void SpecialJump()
    {
        playerMovement.PlayerJump(specialJumpForce);
    }

    private void Teleport()
    {
        Vector2 direction = playerMovement.facingRight? Vector2.right : Vector2.left;
        transform.position += (Vector3)(direction * 2);
    }

    private void FireRocket()
    {
        RocketPrefab.GetComponent<RocketController>().facingRight = playerMovement.facingRight;
        Instantiate(RocketPrefab, transform.position, Quaternion.identity);
    }

    private void FireInverter()
    {
        InverterPrefab.GetComponent<InverterShotController>().facingRight = playerMovement.facingRight;
        Instantiate(InverterPrefab, transform.position, Quaternion.identity);
    }

    private void SwitchGravity()
    {
        playerMovement.ChangeGravity();
    }

    private void SwitchCurrentAction(InputAction.CallbackContext callbackContext)
    {
        currentAction = (currentAction+1)%possibleActions.Count;
        uIController.SetActive(possibleActions[currentAction]);
    }

    public void DisableItem(ActionsEnum type)
    {
        ActionUIController.Instance.DisableItem(type);
        possibleActions.Remove(type);
        if (currentAction == possibleActions.Count)
        {
            currentAction = 0;
        }
        uIController.SetActive(possibleActions[currentAction]);
    }
}
