using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseShortcut : MonoBehaviour
{
    public GameObject pauseModal;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseGame.Instance.isPaused)
            {
                PauseGame.Instance.UnPause();
                pauseModal.SetActive(false);
            }
            else{
                PauseGame.Instance.Pause();
                pauseModal.SetActive(true);
            }
        }
    }
}
