using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    private Image _image = default;
    private Button _button = default;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    public void SetIcon(Sprite icon)
    {
        _image.sprite = icon;
    }

    public void BindToOnClick(Action doOnClick)
    {
        _button.onClick.AddListener(() =>
        {
            doOnClick.Invoke();
        });
    }
}
