using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : GenericSingleton<MySceneManager>
{
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void Restart(float Delay)
    {
        Invoke("Restart",Delay);
    }
}
