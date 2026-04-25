using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceController : MonoBehaviour, IPointerDownHandler
{
    [Header("SERIALIZEFIELD")]
    [SerializeField] private Button continueButton;

    [Header("PUBLIC")]
    public bool isChoiceSelected = false;
    public string buttonName;

    public void OnPointerDown(PointerEventData eventData)
    {
        isChoiceSelected = true;
        buttonName = gameObject.name;
        continueButton.interactable = true;
    }
}
