using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuContoler : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("CombatScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
