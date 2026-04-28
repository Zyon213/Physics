using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI firstQuestionTMP;
    [SerializeField] private GameObject earth;
    [SerializeField] private GameObject mars;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private GameObject hintPage;
    [SerializeField] private GameObject correctPage;
    [SerializeField] private GameObject firstQuestion;
    [SerializeField] private GameObject secondQuestion;

    [Header("PRIVATE")]
    private LoadScene loadScene;
    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;
    private DialogueController dialogueController;

    private readonly string firstText = "Scenario: You are the pilot of the Hope Probe II," +
        " Your rocket is 1,000,000 km from Mars.You must reach the planet in 10 hours.\n\n" +
        "Challenge: What average speed is required?";

    private void Awake()
    {
        if (dialogueBox != null)
        {
            dialogueBox.transform.localScale = Vector3.zero;
            dialogueBox.SetActive(false);
        }

        dialogueController = GetComponent<DialogueController>();
        if (dialogueController != null)
        {
            dialogueController.SetObjectState(continueButton.gameObject, false);
            continueButton.interactable = false;
        }
        dialogueController.SetObjectState(earth, false);
        dialogueController.SetObjectState(mars, false);
        dialogueController.HideButtons(answerButtons);
        dialogueController.SetObjectState(hintPage, false);
        dialogueController.SetObjectState(correctPage, false);
        dialogueController.SetObjectState(secondQuestion, false);
    }
    private void Start()
    {
        loadScene = GetComponent<LoadScene>();
    }
    public IEnumerator LoadQuestion()
    {
        yield return new WaitForEndOfFrame();
        dialogueController.ScaleDialogueBox(dialogueBox);

        yield return new WaitForSeconds(0.5f);
        dialogueController.ShowQuestion(answerButtons, continueButton, firstQuestionTMP, firstText);

        yield return new WaitForSeconds(0.5f);

        dialogueController.ScaleObject(earth, Vector3.one * 8, easeElastic, 2f);

        yield return new WaitForSeconds(0.7f);

        dialogueController.ScaleObject(mars, Vector3.one * 4, easeElastic, 2f);
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
        dialogueController.ScaleDownObject(firstQuestion);

        yield return new WaitForSeconds(0.2f);
        dialogueController.ScaleDownObject(earth);

        yield return new WaitForSeconds(0.2f);
        dialogueController.ScaleDownObject(mars);

        yield return new WaitForSeconds(0.4f);

        secondQuestion.SetActive(true);
    }

    public void LoadHomePage()
    {
        if (loadScene == null) return;
        dialogueController.ScaleDownObject(dialogueBox);
        dialogueController.ScaleDownObject(firstQuestion);
       loadScene.CubeTransition("Scene01");
    }
}
