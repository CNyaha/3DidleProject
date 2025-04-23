using DG.Tweening;
using System.Collections;
using UnityEngine;

public enum Type
{
    Default,
    Sine,
    Cubic
}

public class BackgroundController : MonoBehaviour
{
    public Type easingType = Type.Default;
    public Vector3 offset;
    public float duration = 1f;

    private Vector3 startPos;
    private bool isRunning = true;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        Vector3 target = startPos + offset;
        while (isRunning)
        {
            yield return MoveTo(target);
            yield return MoveTo(startPos);
        }
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        transform.DOMove(destination, duration).SetEase(GetEase(easingType));
        yield return new WaitForSeconds(duration);
    }

    private Ease GetEase(Type type)
    {
        return type switch
        {
            Type.Sine => Ease.InOutSine,
            Type.Cubic => Ease.InOutCubic,
            _ => Ease.Linear
        };
    }

    public void Stop() => isRunning = false;
}
