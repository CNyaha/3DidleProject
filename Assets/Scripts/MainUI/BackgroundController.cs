using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum Type
{
    Default,
    Sine,
    Cubic
}

public class BackgroundController : MonoBehaviour
{
    public Type type;
    public Vector3 targetPos;
    public float duration;

    Vector3 lastPos;
    bool isExit;

    void Start()
    {
        lastPos = transform.position;
        targetPos += transform.position;

        switch (type)
        {
            case Type.Default:
                StartCoroutine(Default());
                break;
            case Type.Sine:
                StartCoroutine(Sine());
                break;
            case Type.Cubic:
                StartCoroutine(Cubic());
                break;
        }
    }

    IEnumerator Default()
    {
        while (true)
        {
            transform.DOMove(targetPos, duration);
            yield return new WaitForSeconds(duration);
            transform.DOMove(lastPos, duration);
            yield return new WaitForSeconds(duration);

            if (isExit) break;
        }
        yield return null;
    }

    IEnumerator Sine()
    {
        while (true)
        {
            transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(duration);
            transform.DOMove(lastPos, duration).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(duration);

            if (isExit) break;
        }
        yield return null;
    }

    IEnumerator Cubic()
    {
        while (true)
        {
            transform.DOMove(targetPos, duration).SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(duration);
            transform.DOMove(lastPos, duration).SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(duration);

            if (isExit) break;
        }
        yield return null;
    }
}
