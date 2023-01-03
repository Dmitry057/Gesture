using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviour
{
    [SerializeField] private bool pcMode;
    private void Start()
    {
        if (pcMode) GoToLoadScene();
    }
    public void GoToLoadScene()
    {
        SceneManager.LoadScene("Loading");
    }
}
