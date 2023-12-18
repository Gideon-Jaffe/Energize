using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUiController : MonoBehaviour
{
    public void onButtonClicked()
    {
        GameController.Instance.GoToNextLevel(false);
    }
}
