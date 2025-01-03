using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public event Action GameRestarted = delegate { };

    public void PlayAgain()
    {
        GameRestarted();
    }

}
