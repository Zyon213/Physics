using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaeedMoves : MonoBehaviour
{
    [SerializeField] private GameObject saeed;
    [SerializeField] private Transform targetPos;
    private Animator anim;
    private void Awake()
    {
        if (saeed != null)
        {
            anim = saeed.GetComponent<Animator>();
            anim.SetTrigger("Idle");
        }
        MoveSaeed();
    }

    // move and scale down saeed
    public void MoveSaeed()
    {
        Vector3 localPos = targetPos.position;

        Vector3 destination = saeed.transform.parent.InverseTransformPoint(localPos);

        LeanTween.moveLocal(saeed, destination, 0.8f)
            .setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(saeed, new Vector3(0.35f, 0.35f, 1f), 0.5f)
            .setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
    }
}
