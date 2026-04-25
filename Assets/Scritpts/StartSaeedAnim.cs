using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject firstScene;
    [SerializeField] private Button[] choiceButton;
    [SerializeField] private Color hoverColor;

    [Header("PRIVATE")]
    private Graphic graphic;
    private Color initialColor;
    private float buttonScaleFactor = 1.2f;
    private float zero = 0f;
    private float delay = 0.5f;
    private Vector3 targetScale = Vector3.one;
    private float transitionDuration  = 3f;
    private LeanTweenType easeLinear = LeanTweenType.linear;
    private LeanTweenType easeIn = LeanTweenType.easeInOutSine;
    // selected choice button
    private string selectedChoiceName;

    // dialogue box varialbles
    private float dialogueBoxDuration = 0.75f;
    private LeanTweenType easeQuad = LeanTweenType.easeOutQuad;

    // continue button variables
    private float buttonDuration = 2f;
    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;

    private Coroutine dialogueCorotine;
    // dialogue string variables
    private float dialogueSpeed = 0.01f;

    private readonly string firstDialogueString = "“Hi! My name is Saeed. I live here in the UAE, and I’ve always " +
        "looked up at the stars… One day, I want to become an astronomer and explore space. But to do that, " +
        "I need to understand science…Will you join me on this journey?”";

    private readonly string secondDialogueString = "“Great! Let’s choose a topic to explore " +
        "together. What would you like to learn today?”";
/*
    private void Start()
    {
        graphic = GetComponent<Graphic>();
        if (graphic != null)
            initialColor = graphic.color;
    }
  */
    private void Awake()
    {
        SetObjectState(saeed, false);
        SetObjectState(dialogueBox, false);
        SetObjectState(nextButton.gameObject, false);
        SetObjectState(continueButton.gameObject, false);
        SetObjectState(choicePanel, false);

        SetObjectScale(dialogueBox.gameObject, Vector3.zero);
        SetObjectScale(nextButton.gameObject, Vector3.zero);
        SetObjectScale(continueButton.gameObject, Vector3.zero);

        HideButtons();
        StartCoroutine(LaunchGame());
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

        yield return new WaitForSeconds(delay);

        // display dialogue box
        dialogueBox.SetActive(true);
        ScaleDialogueBox(dialogueBox, easeQuad, dialogueBoxDuration);

        yield return new WaitForSeconds(delay);
        
        // display first dialogue text
        dialogueCorotine =  StartCoroutine(TypeText(firstDialogue, firstDialogueString, () =>
        {
            ShowButton(nextButton);
        }));

        yield return new WaitForSeconds(delay);
    }

    // fade in start transition 
    private void StartTransition()
    {
        CanvasGroup canvas = transitionCanvas.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvas, zero, transitionDuration)
            .setEase(easeLinear).setOnComplete(() =>
        {
            transitionCanvas.SetActive(false);
        });
    }

    // display the dialouge text showing each character 
    IEnumerator TypeText(TextMeshProUGUI tmpro, string dialogue, System.Action onComplete)
    {
        tmpro.text = dialogue;
        tmpro.maxVisibleCharacters = 0;

        int length = dialogue.Length;
        for (int i = 0; i <= length; i++)
        {
            tmpro.maxVisibleCharacters = i;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        onComplete?.Invoke();
    }

    // disable the next button, fade out the dialogue text and the button
    // when it finish load the next text and choice buttons

    public void PressNextButton()
    {
        continueButton.interactable = false;
        nextButton.interactable = false;
        LeanTween.alpha(firstDialogue.gameObject, zero, delay)
            .setOnUpdate((float val) =>
            {
                firstDialogue.alpha = val;
            }).
        setOnComplete(() =>
        {
            firstDialogue.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            DisplaySecondDialogue();
        });
    }

    // display second dialogue text
    private void DisplaySecondDialogue()
    {
        if (secondDialogueString != null)
            dialogueCorotine = StartCoroutine(TypeText(secondDialogue, secondDialogueString, () =>
            {
                ShowButton(continueButton);

            }));
        StartCoroutine(ShowChoiceButtons());
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
            ShowButton(choiceButton[i]);
            yield return new WaitForSeconds(duration);
        }
    }

    // scale up and show button
    private void ShowButton(Button button)
    {
        button.gameObject.SetActive(true);
        button.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(button.gameObject, targetScale, buttonDuration)
            .setDelay(delay).setEase(easeElastic);
    }
    
    // disable all choice buttons
    public void DisableButtonSelection()
    {
        foreach (Button btn in choiceButton)
        {
            ChoiceController choice = btn.GetComponent<ChoiceController>();
            choice.isChoiceSelected = false;
            choice.buttonName = null;
        }
    }
    // get the name of the selected choice button
    private void CheckSelectedButton()
    {
        selectedChoiceName = null;
        foreach (Button button in choiceButton)
        {
            ChoiceController choice = button.GetComponent<ChoiceController>();
            if (choice.isChoiceSelected)
            {
               selectedChoiceName = choice.buttonName;
               DisableButtonSelection();
                break;
            }
        }
    }

    public void ScaleSelectedButton()
    {
        foreach (Button button in choiceButton)
        {
            if (button == null) continue;

            ChoiceController choice = button.GetComponent<ChoiceController>();
            var graphic = button.GetComponent<Graphic>();
            if (choice != null && graphic != null)
            {
                initialColor = graphic.color;
                if (choice.isChoiceSelected)
                {
                    LeanTween.scale(button.gameObject, Vector3.one * buttonScaleFactor, 0.2f).setEase(easeQuad);
                    LeanTween.value(button.gameObject, graphic.color, hoverColor, buttonDuration)
                        .setOnUpdate((Color col) => { graphic.color = col; });
                    Debug.Log("pointer clided selectee");

                }
                else
                {
                    choice.isChoiceSelected = false;
                    LeanTween.scale(button.gameObject, targetScale, 0.2f).setEase(easeIn);
                    LeanTween.value(button.gameObject, graphic.color, initialColor, buttonDuration)
                        .setOnUpdate((Color col) => { graphic.color = col; });
                    Debug.Log("pointer not button clided selectee");

                }
            }
        }
    }
  

    public void PressContineuButton()
    {
        CheckSelectedButton();
        if (string.IsNullOrEmpty(selectedChoiceName))
            return;
        LeanTween.scale(firstScene, Vector3.zero, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            SceneManager.LoadScene(selectedChoiceName, LoadSceneMode.Additive);
        });
    }

    // set game object state
    private void SetObjectState(GameObject obj, bool state)
    {
        if (obj != null)
            obj.SetActive(state);
    }

    // Set local scale to zero
    private void SetObjectScale(GameObject obj, Vector3 scale)
    {
        if (obj != null)
            obj.transform.localScale = scale;
    }

    // scale dialogue box
    private void ScaleDialogueBox(GameObject gameObject, LeanTweenType leanType, float duration)
    {
        LeanTween.scale(gameObject, targetScale, duration).setDelay(delay).setEase(leanType);
    }

    // set choice buttons hidden and scale to zero
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
}
