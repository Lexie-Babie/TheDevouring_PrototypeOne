using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuControler : MonoBehaviour
{
    public void onStartClick()
    {
        SceneManager.LoadScene("DungeonMap");
    }

    public void onExitClick()
    {
        Application.Quit();
    }
}
