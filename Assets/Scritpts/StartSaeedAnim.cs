using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSaeedAnim : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private Animator saeedAnim;
    [SerializeField] private GameObject saeed;

    private void Start()
    {
        saeed.SetActive(false);
    }
    public void PlaySaeedIntro()
    {
        StartCoroutine(DelaySaedIntro());
    }

        // Delay the player animation until the transition animaiton is done
    IEnumerator DelaySaedIntro()
    {
        // first set the transition animation to play
        yield return new WaitForEndOfFrame();

        float transitionDuration = transitionAnim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(transitionDuration);

        // play player animation when transition finishes
        saeed.SetActive(true);
        saeedAnim.SetTrigger("StartSaeed");
    }
}
