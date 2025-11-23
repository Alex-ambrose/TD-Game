using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    public Button button;
    public TMP_Text buttonText;

    public void Setup(string text, UnityEngine.Events.UnityAction onClickAction)
    {
        buttonText.text = text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClickAction);
    }
}
