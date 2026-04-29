using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SecondQuection : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject dialogueBox;

    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private GameObject hintPage;
    [SerializeField] private GameObject correctPage;
    [SerializeField] private GameObject secondQuest;
    [SerializeField] private GameObject thirdQuest;
    [SerializeField] private GameObject saturn;


    private Coroutine corotine;
    private Coroutine dialogueCorotine;
    private LoadScene loadScene;

    [Header("PRIVATE")]
    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;
    private DialogueController dialogueController;

    private readonly string questText = "Scenario: You have discovered " +
        "a massive planet made of gas with a spectacular system of icy rings.\n\n " +
        "Challenge: Identify this giant planet to register it in your database!";

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
        if (dialogueController != null)
        {
            dialogueController.SetObjectState(continueButton.gameObject, false);
            continueButton.interactable = false;
        }
        dialogueController.HideButtons(answerButtons);
        dialogueController.SetObjectState(hintPage, false);
        dialogueController.SetObjectState(correctPage, false);
        dialogueController.SetObjectState(thirdQuest, false);
        dialogueController.SetObjectState(saturn, false);
    }

    private void Start()
    {
        loadScene = GetComponent<LoadScene>();
    }

    public IEnumerator LoadQuestion()
    {
        yield return new WaitForEndOfFrame();
        secondQuest.SetActive(true);
        secondQuest.transform.localScale = Vector3.one;
        SecondQuestion(answerButtons, continueButton, questionTMP, questText);
        DisplaySecondDialogue();
        yield return new WaitForSeconds(0.5f);
        saturn.transform.localScale = Vector3.one;
        dialogueController.ScaleObject(saturn, Vector3.one * 10f, easeElastic, 2f);

    }
    
    
    private void DisplaySecondDialogue()
    {
        if (questionTMP != null)
            dialogueCorotine = StartCoroutine(TypeText(questionTMP, questText, () =>
            {
             //   dialogueController.ShowButton(continueButton);

            }));
    }
    
    public IEnumerator TypeText(TextMeshProUGUI tmpro, string dialogue, System.Action onComplete)
    {
        yield return new WaitForEndOfFrame();

        if (tmpro != null && dialogue != null)
        {
            CanvasGroup canvas = tmpro.GetComponent<CanvasGroup>();
            if (canvas != null) canvas.alpha = 1;
            tmpro.text = "";
            tmpro.text = dialogue;
            tmpro.maxVisibleCharacters = 0;

            int length = dialogue.Length;
            for (int i = 0; i <= length; i++)
            {
                tmpro.maxVisibleCharacters = i;
                yield return new WaitForSeconds(0.01f);
            }
            onComplete?.Invoke();
        }
    }
    public void SecondQuestion(Button[] buttons, Button button, TextMeshProUGUI tmpro, string text)
    {
        if (text != null)
            corotine = StartCoroutine(TypeText(tmpro, text, () =>
            {
                dialogueController.ShowButton(button, dialogueController.buttonDuration);
                StartCoroutine(dialogueController.ShowAnswerButtons(buttons));

            }));
    }
    public void PressCheckAnswer()
    {
        dialogueController.CheckSelectedButton();
        if (string.IsNullOrEmpty(dialogueController.selectedChoiceName)) return;

        if (dialogueController.selectedChoiceName.Equals("Wrong"))
            dialogueController.ScaleObject(hintPage, Vector3.one, easeElastic, 2f);
        else if (dialogueController.selectedChoiceName.Equals("Correct"))
            dialogueController.ScaleObject(correctPage, Vector3.one, easeElastic, 2f);
    }

    public void PressTryAgain()
    {
        hintPage.SetActive(false);
        dialogueController.ResetAnswerButtons();
    }

    public void PressContinue()
    {
        StartCoroutine(HideObjects());
    }

    // hide object
    IEnumerator HideObjects()
    {
        yield return new WaitForEndOfFrame();
        dialogueController.FadeOutObject(questionTMP.gameObject);
        dialogueController.ScaleDownObject(secondQuest);

        yield return new WaitForSeconds(0.4f);

        thirdQuest.transform.localScale = Vector3.one;
        thirdQuest.SetActive(true);
    }

    // load home page
    public void LoadHomePage()
    {
        if (loadScene == null) return;
        dialogueController.ScaleDownObject(dialogueBox);
        dialogueController.ScaleDownObject(secondQuest);
        loadScene.CubeTransition("Scene01");
    }
}
