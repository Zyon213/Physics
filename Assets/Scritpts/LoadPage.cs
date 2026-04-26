using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadPage : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private CanvasGroup transitionGroup;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private GameObject circleTransition;
    [SerializeField] private GameObject rectTransition;
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject bottomBar;
   // [SerializeField] private GameObject[] gridCells;
    private RectTransform[] cubes;
    [SerializeField] private GameObject obj;

    private void Awake()
    {
        cubes = obj.GetComponentsInChildren<RectTransform>();
        ScaleCube();
    }

    private float rectMoveX = 7091f;

    private void Start()
    {
        SetSafeOjbect(circleTransition, false);
        if (rectTransition != null)
            rectTransition.SetActive(false);
    }

    private void ScaleCube()
    {
        foreach (RectTransform grid in cubes)
        {
            grid.transform.localScale = Vector3.zero;
        }
    }
    private void SetSafeOjbect(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
            obj.transform.localScale = Vector3.zero;
        }
    }
    
    // fade in transition
    public void ChangeScene()
    {
        transitionGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(transitionGroup, 0, duration)
                 .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    // scale transition
    public void ScaleTransition()
    {
        circleTransition.SetActive(true);

        LeanTween.scale(circleTransition, new Vector3(10f, 10f, 1f), duration)
                 .setEase(LeanTweenType.easeInOutExpo)
                 .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    // slide to the side transition 
    public void SlideTransition()
    {
        rectTransition.SetActive(true);
        RectTransform rect = rectTransition.GetComponent<RectTransform>();

        CanvasGroup rectCanvas = rectTransition.GetComponent<CanvasGroup>();
        LeanTween.moveX(rect, rectMoveX, duration)
                 .setEase(LeanTweenType.easeOutQuint)
                 .setOnComplete(() =>
                 LeanTween.alphaCanvas(rectCanvas, 0, 0.5f)
                 .setOnComplete(() => SceneManager.LoadScene(sceneName)));
    }

    // close gate transtion
    public void GateTransition()
    {
        float screenHeight = Screen.height;

        LeanTween.moveY(topBar.GetComponent<RectTransform>(), -screenHeight / 2, 2f).setEaseOutQuint();
        LeanTween.moveY(bottomBar.GetComponent<RectTransform>(), screenHeight / 2, 2f).setEaseOutQuint()
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    // Cubes scale transition
    public void ScaleCubes()
    {
        StartCoroutine(GridWipe());
    }
    // cover the screen with cubes
    public IEnumerator GridWipe()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            LeanTween.scale(cubes[i], Vector3.one, 0.4f).setEaseOutBack();

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
