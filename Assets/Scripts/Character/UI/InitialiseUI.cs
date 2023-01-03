using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialiseUI : MonoBehaviour
{
    #region Components
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Transform _canvasPlace;
    [SerializeField] private Camera cam;
    [SerializeField] private GestureInputProcessor GestureProc;
    #endregion
    private void Start()
    {
        InitializeCanvas();
    }
    private void InitializeCanvas()
    {
        GameObject canvas = Instantiate(_canvas, _canvasPlace);
        canvas.transform.SetParent(_canvasPlace);
        canvas.GetComponent<Canvas>().worldCamera = cam;
        GestureProc.fightUI = canvas.GetComponent<FightUI>();
        
    }

}