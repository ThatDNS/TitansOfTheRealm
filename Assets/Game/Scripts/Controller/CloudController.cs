using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudController : MonoBehaviour
{
    public float moveDistance = 10.0f;
    public float duration=5.0f;

    public LoopType loopType=LoopType.Yoyo;
    public int loops = -1;


    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveX(transform.position.x + moveDistance, duration).SetEase(Ease.InOutQuad));
        mySequence.Append(transform.DOMoveX(transform.position.x, duration).SetEase(Ease.InOutQuad));
        mySequence.SetLoops(loops, loopType);
    }

}
