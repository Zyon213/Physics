using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartSaeedAnim : MonoBehaviour
{
    [Header("SERIALIZEFIELD")]
    [SerializeField] private GameObject transitionCanvas;
    [SerializeField] private GameObject saeed;
    [SerializeField] private Animator saeedAnim;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI firstDialogue;
    [SerializeField] private TextMeshProUGUI secondDialogue;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButton;


    [Header("PUBLIC")]

    [Header("PRIVATE")]
    private float one = 1f;
    private float zero = 0f;
    private float delay = 0.5f;
    private Vector3 targetScale = Vector3.one;
    private float transitionDuration  = 3f;
    private LeanTweenType easeLinear = LeanTweenType.linear;


    // dialogue box varialbles
    private float dialogueBoxDuration = 0.75f;
    private LeanTweenType easeQuad = LeanTweenType.easeOutQuad;

    // continue button variables
    private float buttonDuration = 2f;
    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;

    // dialogue string variables
    private float dialogueSpeed = 0.01f;

    private readonly string firstDialogueString = "“Hi! My name is Saeed. I live here in the UAE, and I’ve always " +
        "looked up at the stars… One day, I want to become an astronomer and explore space. But to do that, " +
        "I need to understand science…Will you join me on this journey?”";

    private readonly string secondDialogueString = "“Great! Let’s choose a topic to explore " +
        "together. What would you like to learn today?”";

    private void Awake()
    {
        if (saeed != null)
            saeed.SetActive(false);

        // dialogue box
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        dialogueBox.transform.localScale = Vector3.zero;

        // Next button
        if (nextButton.gameObject != null)
        {
            nextButton.gameObject.SetActive(false);
            nextButton.transform.localScale = Vector3.zero;
        }

        // Continue button
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
            continueButton.transform.localScale = Vector3.zero;
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);
        HideButtons();
        StartCoroutine(LaunchGame());
    }

    private void HideButtons()
    {
        if (choiceButton != null)
        {
            foreach (Button button in choiceButton)
            {
                button.gameObject.SetActive(false);
                button.transform.localScale = Vector3.zero;
            }
        }
    }

    IEnumerator LaunchGame()
    {    
        StartTransition();
        yield return new WaitForEndOfFrame();

        // start transition 
        yield return new WaitForSeconds(transitionDuration);

        // play player animation when transition finishes
        saeed.SetActive(true);
        saeedAnim.SetTrigger("StartSaeed");

        // display dialogue box
         yield return new WaitForSeconds(delay);

        dialogueBox.SetActive(true);
        ScaleUpGameObject(dialogueBox, easeQuad, dialogueBoxDuration);

        yield return new WaitForSeconds(delay);
        
        // display first dialogue text
        StartCoroutine(TypeText(firstDialogue, firstDialogueString));

        yield return new WaitForSeconds(delay);
    }

    // fade in start transition 
    private void StartTransition()
    {
        LeanTween.alphaCanvas(transitionCanvas.GetComponent<CanvasGroup>(), zero, transitionDuration).setEase(easeLinear).setOnComplete(() =>
        {
            transitionCanvas.SetActive(false);

        });
    }
    // scale up an object
    private void ScaleUpGameObject(GameObject gameObject, LeanTweenType leanType, float duration)
    {
        LeanTween.scale(gameObject, targetScale, duration).setDelay(delay).setEase(leanType);
    }

    // scale up and show button
    private void ShowButton(Button button, LeanTweenType leanType, float duration)
    {
        button.gameObject.SetActive(true);
        LeanTween.scale(button.gameObject, targetScale, duration).setDelay(delay).setEase(leanType);
    }

    // display the dialouge text showing each character 
    IEnumerator TypeText(TextMeshProUGUI tmPro, string dialogue)
    {
        yield return new WaitForSeconds(delay);
        tmPro.text = dialogue;
        tmPro.maxVisibleCharacters = 0;

        tmPro.ForceMeshUpdate();

        int dialogueLength = dialogue.Length;
        int counter = 0;

        while (counter <= dialogueLength)
        {
            tmPro.maxVisibleCharacters = counter;
            counter++;

            yield return new WaitForSeconds(dialogueSpeed);
        }
        // show continuebutton after delay
        yield return new WaitForSeconds(delay);
        ShowButton(nextButton, easeElastic, buttonDuration);
    }

    // disable the continue button, fade out the dialogue text and the button
    // when it finish load the next text and choice buttons
    public void PressedContinue()
    {
        nextButton.interactable = false;
        LeanTween.value(firstDialogue.gameObject, one, zero, delay).setOnComplete(() => 
        { firstDialogue.gameObject.SetActive(false); });

        LeanTween.alphaCanvas(nextButton.GetComponent<CanvasGroup>(), zero, delay).setOnComplete(() => 
        {
            StartCoroutine(TypeText(secondDialogue, secondDialogueString));
            continueButton.interactable = false;
            ShowButton(continueButton, easeElastic, buttonDuration);
            StartCoroutine( ShowChoiceButtons());
        });      
    }

    //show choice buttons

    IEnumerator ShowChoiceButtons()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(transitionDuration);

        choicePanel.SetActive(true);
        for (int i = 0; i < choiceButton.Length; i++)
        {
            float duration = 0.3f;
            ShowButton(choiceButton[i], easeElastic, buttonDuration);
            yield return new WaitForSeconds(duration);
        }
    }

}
