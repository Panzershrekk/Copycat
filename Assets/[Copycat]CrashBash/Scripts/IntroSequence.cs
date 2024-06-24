using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameSequences : MonoBehaviour
{
    [SerializeField] private Image _blackScreen;
    [SerializeField] private Image _3;
    [SerializeField] private Image _2;
    [SerializeField] private Image _1;
    [SerializeField] private Image _go;
    [HideInInspector] public UnityEvent onStartSequenceOver = new UnityEvent();
    [HideInInspector] public UnityEvent onEndSequenceOver = new UnityEvent();

    public void StartIntroSequence()
    {
        _3.color = new Color(_3.color.r, _3.color.g, _3.color.b, 0);
        _2.color = new Color(_2.color.r, _2.color.g, _2.color.b, 0);
        _1.color = new Color(_1.color.r, _1.color.g, _1.color.b, 0);
        _go.color = new Color(_go.color.r, _go.color.g, _go.color.b, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_blackScreen.DOFade(0, 0.4f).SetEase(Ease.Linear))
               .AppendInterval(0.4f)
               .Append(_3.DOFade(1, 0.5f).SetEase(Ease.InOutQuad))
               .Append(_3.DOFade(0, 0.5f).SetEase(Ease.InOutQuad))
               .Append(_2.DOFade(1, 0.5f).SetEase(Ease.InOutQuad))
               .Append(_2.DOFade(0, 0.5f).SetEase(Ease.InOutQuad))
               .Append(_1.DOFade(1, 0.5f).SetEase(Ease.InOutQuad))
               .Append(_1.DOFade(0, 0.5f).SetEase(Ease.InOutQuad))
                .Append(_go.DOFade(1, 0.3f).SetEase(Ease.InOutQuad))
                .Append(_go.DOFade(0, 0.3f).SetEase(Ease.InOutQuad));


        sequence.Play();
        sequence.OnComplete(() => onStartSequenceOver?.Invoke());
    }

    public void StartEndSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.4f).
                Append(_blackScreen.DOFade(1, 0.4f).SetEase(Ease.Linear));
        sequence.Play();
        sequence.OnComplete(() => onEndSequenceOver?.Invoke());
    }
}
