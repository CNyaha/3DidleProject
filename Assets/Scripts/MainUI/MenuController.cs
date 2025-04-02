using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    pause,
    settings
}

public class MenuController : MonoBehaviour
{
    public UIType uiType;
    public Type type;
    public Vector3 targetPos;
    public float duration;

    Vector3 lastPos;
    bool isMove;
    Coroutine coroutine;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPos = transform.position;
        targetPos += transform.position;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (animator != null)
            animator.Play("ButtonAnim", 0);
    }

    public void MoveToTarget()
    {
        Time.timeScale = 1;
        if (!isMove)
        {
            if (coroutine != null) StopCoroutine(coroutine);

            gameObject.SetActive(true);

            switch (type)
            {
                case Type.Default:
                    transform.DOMove(targetPos, duration);
                    break;
                case Type.Sine:
                    transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine);
                    break;
                case Type.Cubic:
                    transform.DOMove(targetPos, duration).SetEase(Ease.InOutCubic);
                    break;
            }

            coroutine = StartCoroutine(WaitForOpen(duration));
        }
        else
        {
            if (coroutine != null) StopCoroutine(coroutine);

            switch (type)
            {
                case Type.Default:
                    transform.DOMove(lastPos, duration);
                    break;
                case Type.Sine:
                    transform.DOMove(lastPos, duration).SetEase(Ease.InOutSine);
                    break;
                case Type.Cubic:
                    transform.DOMove(lastPos, duration).SetEase(Ease.InOutCubic);
                    break;
            }

            coroutine = StartCoroutine(WaitForClose(duration));
        }

        isMove = !isMove;
    }

    IEnumerator WaitForOpen(float duration)
    {
        yield return new WaitForSeconds(duration);
        Time.timeScale = 0;
    }

    IEnumerator WaitForClose(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        if (uiType == UIType.settings)
            Time.timeScale = 0;
    }
}
