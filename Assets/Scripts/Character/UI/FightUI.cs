using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    
    [Header("UI")]
    [SerializeField] private Slider ComboSlider;
    [SerializeField] private Image FillImage;
    [SerializeField] private Text ComboText;
    [SerializeField] private Text ComboGestureName;
    private void Start()
    {
        ComboSlider.gameObject.SetActive(false);
        ComboText.text = "";
    }
    public void AddNameGestureText(string text)
    {
        ComboGestureName.text = text;
    }
    public void ShowSlider(int _comboLength)
    {
        ComboSlider.gameObject.SetActive(true);

        if (_comboLength > 1)
            ComboText.text = "X" + _comboLength + " COMBO!".ToString();
        else
            ComboText.text = "";
    }
    public void DecreaseSlider(float relation)
    {
        ComboSlider.value = relation;
        FillImage.color = Color.Lerp(Color.red, Color.green, relation);
    }
    public void HideSlider()
    {
        ComboSlider.gameObject.SetActive(false);
        ComboSlider.value = 1;
    }

}
