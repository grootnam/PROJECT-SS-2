﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("StartStage");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
