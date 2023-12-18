using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFinishController : MonoBehaviour
{
    public void OnButtonClick()
    {
        Destroy(GameController.Instance.gameObject);
        Destroy(ActionController.Instance.gameObject);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
