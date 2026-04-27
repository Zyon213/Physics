using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaeedMoves : MonoBehaviour
{
    [SerializeField] private GameObject saeed;
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI firstQuestionTMP;
    [SerializeField] private GameObject earth;
    [SerializeField] private GameObject mars;
    [SerializeField] private Button[] answerButtons;

    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;
    private LeanTweenType easeBack = LeanTweenType.easeOutBack;
    private LeanTweenType easeIn = LeanTweenType.easeInOutSine;
    private Animator anim;
    private DialogueController dialogueController;
    private Coroutine questionCorotine;


    private readonly string firstText = "Scenario: You are the pilot of the Hope Probe II," +
        " Your rocket is 1,000,000 km from Mars.You must reach the planet in 10 hours.\n\n" +
        "Challenge: What average speed is required?";


    private void Awake()
    {
        if (saeed != null)
        {
            anim = saeed.GetComponent<Animator>();
            anim.SetTrigger("Idle");
        }
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
        MoveSaeed();

    }

    // move and scale down saeed
    public void MoveSaeed()
    {
        Vector3 localPos = targetPos.position;

        Vector3 destination = saeed.transform.parent.InverseTransformPoint(localPos);

        LeanTween.moveLocal(saeed, destination, 0.8f)
            .setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(saeed, new Vector3(0.35f, 0.35f, 1f), 0.8f)
            .setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                StartCoroutine(FirstQuestion());
            });
    }

    IEnumerator FirstQuestion()
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

}
