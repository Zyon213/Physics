using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // button scale variables for hover
    private float buttonScaleFactor = 1.2f;
    private float buttonDuration = 0.2f;
    private LeanTweenType easeBack = LeanTweenType.easeOutBack;
    private LeanTweenType easeIn = LeanTweenType.easeInOutSine;
    private Vector3 targetScale = Vector3.one;

    [SerializeField] private Color hoverColor;
    private Color initialColor;
    private Graphic graphic;

    private void Start()
    {
        graphic = GetComponent<Graphic>();
        if (graphic != null)
        {
            initialColor = graphic.color;
            // initialColor = Color.white;
        }

    }

    // scale up and change color buttons when hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        if (gameObject.GetComponent<Button>().interactable)
        {
            LeanTween.scale(gameObject, targetScale * buttonScaleFactor, buttonDuration)
                .setEase(easeBack);
            LeanTween.value(gameObject, graphic.color, hoverColor, buttonDuration)
                .setOnUpdate((Color col) => { graphic.color = col; });
        }
    }

    // scale down and change color to original when exit
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

    //     if (graphic != null)
        {
     //       initialColor = graphic.color;
          // initialColor = Color.white;
        }

        ChoiceController control = gameObject.GetComponent<ChoiceController>();
        AnswerController answer = gameObject.GetComponent<AnswerController>();
        if (control != null && control.isChoiceSelected)
        {
            LeanTween.scale(gameObject, Vector3.one * buttonScaleFactor, buttonDuration)
                .setEase(LeanTweenType.easeOutQuad);
            LeanTween.value(gameObject, graphic.color, hoverColor, buttonDuration)
                .setOnUpdate((Color col) => { graphic.color = col; });
        }
        else if (answer != null && answer.isChoiceSelected)
        {
            LeanTween.scale(gameObject, Vector3.one * buttonScaleFactor, 0.2f)
                .setEase(LeanTweenType.easeOutQuad);
            LeanTween.value(gameObject, graphic.color, hoverColor, buttonDuration)
                .setOnUpdate((Color col) => { graphic.color = col; });
        }
        else
        {
            LeanTween.scale(gameObject, Vector3.one, buttonDuration)
                .setEase(easeIn);
            LeanTween.value(gameObject, graphic.color, initialColor, buttonDuration)
                .setOnUpdate((Color col) => { graphic.color = col; });
        }
    }
    
}
