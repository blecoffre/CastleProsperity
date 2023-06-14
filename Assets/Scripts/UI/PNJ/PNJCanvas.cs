using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNJCanvas : MonoBehaviour
{
    [SerializeField] private Image _selectedFeedback = default;

    private void Start()
    {
        SelectedFeedbackDisplayState(false);    
    }


    public void SelectedFeedbackDisplayState(bool active)
    {
        _selectedFeedback.gameObject.SetActive(active);
    }
}
