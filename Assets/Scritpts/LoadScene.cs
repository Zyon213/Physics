using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject circleTransition;
    [SerializeField] private GameObject rectTransition;
    [SerializeField] private GameObject cubeTransition;
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject bottomBar;

    private RectTransform[] cubes;
    private float duration = 1.0f;
    private float rectMoveX = 7091f;

    private void Start()
    {
        SetSafeOjbect(circleTransition, false);
        if (rectTransition != null)
            rectTransition.SetActive(false);
        if (cubeTransition != null)
        {
            cubeTransition.SetActive(false);
            cubes = cubeTransition.GetComponentsInChildren<RectTransform>();
            MinimizeCube();
        }

        if (topBar != null && bottomBar != null)
        {
            topBar.SetActive(false);
            bottomBar.SetActive(false);
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
    // minimize array of cubes
    private void MinimizeCube()
    {
        foreach (RectTransform grid in cubes)
        {
            grid.transform.localScale = Vector3.zero;
        }
    }

    // scale transition
    public void CircleTransition(string sceneName)
    {
        circleTransition.SetActive(true);

        LeanTween.scale(circleTransition, new Vector3(10f, 10f, 1f), duration)
                 .setEase(LeanTweenType.easeInOutExpo)
                 .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    // slide to the side transition 
    public void SlideTransition(string sceneName)
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
    public void GateTransition(string sceneName)
    {
        float screenHeight = Screen.height;

        topBar.SetActive(true);
        bottomBar.SetActive(true);
        LeanTween.moveY(topBar.GetComponent<RectTransform>(), -screenHeight / 2, 2f)
            .setEaseOutQuint();
        LeanTween.moveY(bottomBar.GetComponent<RectTransform>(), screenHeight / 2, 2f)
            .setEaseOutQuint()
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    public void GateTransitionOnly(GameObject obj)
    {
        float screenHeight = Screen.height;
        topBar.SetActive(true);
        bottomBar.SetActive(true);
        LeanTween.moveY(topBar.GetComponent<RectTransform>(), -screenHeight / 2, 2f)
            .setEaseOutQuint();
        LeanTween.moveY(bottomBar.GetComponent<RectTransform>(), screenHeight / 2, 2f)
            .setEaseOutQuint()
            .setOnComplete(() => obj.SetActive(true));
    }
    // Cubes scale transition
    public void CubeTransition(string sceneName)
    {
        StartCoroutine(GridWipe(sceneName));
    }
    // cover the screen with cubes
    public IEnumerator GridWipe(string sceneName)
    {
        cubeTransition.SetActive(true);
        for (int i = 0; i < cubes.Length; i++)
        {
            LeanTween.scale(cubes[i], Vector3.one, 0.4f).setEaseOutBack();

            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(sceneName);   
    }
}
