using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AnswerController : MonoBehaviour, IPointerDownHandler
{
    [Header("SERIALIZEFIELD")]
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject gameManager;

    [Header("PRIVATE")]
    private DialogueController dialogueController;

    [Header("PUBLIC")]
    public bool isChoiceSelected = false;
    public string buttonName;

    private void Start()
    {
        if (gameManager != null)
        {
            dialogueController = gameManager.GetComponent<DialogueController>();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isChoiceSelected = true;
        buttonName = gameObject.name;
        continueButton.interactable = true;
        Button btn = GetComponent<Button>();
        if (dialogueController != null)
        {
            dialogueController.SelectedButton(btn);
        }
    }
}
