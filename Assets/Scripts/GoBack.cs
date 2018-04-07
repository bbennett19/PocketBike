using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour
{

	public void GoToMain()
    {
        SceneManager.LoadScene("main_menu");
    }
}
