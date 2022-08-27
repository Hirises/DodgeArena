using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void StartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneEnum.GameScene.name);
    }
}
