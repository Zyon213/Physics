using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Button[] answerButtons;

    private float dialogueSpeed = 0.01f;
    private float buttonScaleFactor = 1.2f;
    private Coroutine corotine;
    public Color initialColor;
    private Vector3 targetScale = Vector3.one;
    public string selectedChoiceName;
    private LeanTweenType easeLinear = LeanTweenType.linear;
    private LeanTweenType easeIn = LeanTweenType.easeInOutSine;
    private LeanTweenType easeElastic = LeanTweenType.easeOutElastic;
    private LeanTweenType easeQuad = LeanTweenType.easeOutQuad;

    public float buttonDuration = 2f;
    // scale dialogue box
    public void ScaleDialogueBox(GameObject gameObject)
    {
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, Vector3.one, 0.8f).
            setEase(LeanTweenType.easeInOutQuad);
    }

    public void ScaleObject(GameObject gameObject, Vector3 scale, LeanTweenType leanType, float duration)
    {
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, scale, duration).
            setEase(leanType);
    }

    public void ScaleDownObject(GameObject obj)
    {

        LeanTween.scale(obj, Vector3.zero, 0.5f).
            setEase(LeanTweenType.easeInBack).setOnComplete(() => { obj.SetActive(false); });
    }
    // dialogue text writing
    public IEnumerator TypeText(TextMeshProUGUI tmpro, string dialogue, System.Action onComplete)
    {
        yield return new WaitForEndOfFrame();

        if (tmpro != null && dialogue != null)
        {
            CanvasGroup canvas = tmpro.GetComponent<CanvasGroup>();
            if (canvas != null) canvas.alpha = 1;
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
    }

    public void ShowQuestion(Button[] buttons, Button button, TextMeshProUGUI tmpro, string text)
    {
        if (text != null)
            corotine = StartCoroutine(TypeText(tmpro, text, () =>
            {
                ShowButton(button, buttonDuration);
                StartCoroutine(ShowAnswerButtons(buttons));

            }));
    }

    public void ShowButton(Button button, float duration)
    {
        button.gameObject.SetActive(true);
        button.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(button.gameObject, Vector3.one, duration)
            .setDelay(0.2f).setEase(easeElastic);
    }

    public void SetObjectState(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
            obj.transform.localScale = Vector3.zero;
        }
    }

    // Display answer buttons
    public IEnumerator ShowAnswerButtons(Button[] buttons)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < buttons.Length; i++)
        {
            ShowButton(buttons[i], 1.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HideButtons(Button[] buttons)
    {
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
                button.transform.localScale = Vector3.zero;
            }
        }
    }

    // get the selected button 
    public void SelectedButton(Button selectedBtn)
    {
        foreach (Button btn in answerButtons)
        {
            if (btn == null) continue;
            AnswerController answer = btn.GetComponent<AnswerController>();
            if (answer == null) continue;
            answer.isChoiceSelected = (btn == selectedBtn);
            answer.buttonName = btn.name;
        }

        ScaleSelectedButton();
    }
    // scale the selected button and scale down to normal the rest;
    public void ScaleSelectedButton()
    {
        foreach (Button button in answerButtons)
        {
            if (button == null) continue;

            AnswerController answer = button.GetComponent<AnswerController>();
            var graphic = button.GetComponent<Graphic>();
            if (answer != null && graphic != null)
            {
                if (answer.isChoiceSelected)
                {
                    LeanTween.scale(button.gameObject, Vector3.one * buttonScaleFactor, 0.2f)
                        .setEase(easeQuad);
                    LeanTween.value(button.gameObject, graphic.color, hoverColor, buttonDuration)
                        .setOnUpdate((Color col) => { graphic.color = col; });
                }
                else
                {
                    answer.isChoiceSelected = false;
                    LeanTween.scale(button.gameObject, targetScale, 0.2f).setEase(easeIn);
                    LeanTween.value(button.gameObject, graphic.color, initialColor, buttonDuration)
                        .setOnUpdate((Color col) => { graphic.color = col; });
                }
            }
        }

    }
    public void CheckSelectedButton()
    {
        selectedChoiceName = null;
        foreach (Button button in answerButtons)
        {
            AnswerController answer = button.GetComponent<AnswerController>();
            if (answer != null && answer.isChoiceSelected)
            {
                selectedChoiceName = answer.buttonName;
                break;
            }
        }
    }
   
    // reset answer buttons
    public void ResetAnswerButtons()
    {
        selectedChoiceName = null;
        foreach (Button button in answerButtons)
        {
            if (button == null) continue;

            AnswerController answer = button.GetComponent<AnswerController>();
            var graphic = button.GetComponent<Graphic>();
            if (answer != null && graphic != null)
            {
                answer.isChoiceSelected = false;
                LeanTween.scale(button.gameObject, targetScale, 0.2f).setEase(easeIn);
                LeanTween.value(button.gameObject, graphic.color, initialColor, buttonDuration)
                    .setOnUpdate((Color col) => { graphic.color = col; });                
            }
        }
    }

    public void FadeOutObject(GameObject obj)
    {
        if (obj == null) return;
        CanvasGroup canvas = obj.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvas, 0f, 1f)
            .setEase(easeLinear).setOnComplete(() =>
            {
                obj.SetActive(false);
            });
    }

}

