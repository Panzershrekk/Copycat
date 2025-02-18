using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using UnityEngine.InputSystem;
using System;

public class GameSequences : MonoBehaviour
{
    private CopyCatInputSystem _tutorialInput;
    
    [SerializeField] private GameObject _tutoPanel;
    [SerializeField] private Music _music;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private Image _blackScreen;
    [SerializeField] private TMP_Text _3;
    [SerializeField] private TMP_Text _2;
    [SerializeField] private TMP_Text _1;
    [SerializeField] private TMP_Text _go;
    [HideInInspector] public UnityEvent onStartSequenceOver = new UnityEvent();
    [HideInInspector] public UnityEvent onEndSequenceOver = new UnityEvent();
    [SerializeField] private EventReference _winSound;
    [SerializeField] private EventReference _321Sound;
    private bool _tutorialSeen = false;

    public void Start()
    {
        _tutorialInput = new CopyCatInputSystem();
        _tutorialInput.Menu.Enable();
        _tutorialInput.Menu.Submit.performed += SubmitTutorial;
        _tutoPanel.SetActive(false);
        if (_tutorialSeen == false)
        {
            PauserManager.Instance.TogglePause(true);
            _tutoPanel.SetActive(true);
        }
    }

    public void StartIntroSequence()
    {
        _3.color = new Color(_3.color.r, _3.color.g, _3.color.b, 0);
        _2.color = new Color(_2.color.r, _2.color.g, _2.color.b, 0);
        _1.color = new Color(_1.color.r, _1.color.g, _1.color.b, 0);
        _go.color = new Color(_go.color.r, _go.color.g, _go.color.b, 0);
        _winText.gameObject.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_blackScreen.DOFade(0, 0.4f).SetEase(Ease.Linear).SetUpdate(true))  
            .Append(StartSoundTween())
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
        sequence.OnComplete(() => { onStartSequenceOver?.Invoke(); });
    }

    public void StartEndSequence(GameObject toZoom, Camera camera, string playerName)
    {
        _winText.text = string.Format("{0} wins !", playerName);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.5f)
                .Append(camera.transform.DOMove(toZoom.transform.position + new Vector3(0, camera.transform.position.y - 1, -2), 0.5f).OnComplete(() =>
                {
                    _winText.gameObject.SetActive(true);
                    FMODUtilities.PlaySoundOneShot(_winSound);
                }))
                .AppendInterval(3.5f)
                .Append(_blackScreen.DOFade(1, 0.4f).SetEase(Ease.Linear));
        sequence.Play();
        sequence.OnComplete(() =>{ onEndSequenceOver?.Invoke(); /*_music.SetGameValue(0);*/ });
    }

    public void SubmitTutorial(InputAction.CallbackContext context)
    {
        _tutorialSeen = true;
        PauserManager.Instance.TogglePause(false);
        _tutoPanel.SetActive(false);        
    }

    public Tween StartSoundTween()
    {
        StartCoroutine(TitomFaitDesTimeScaleCradoAlorsJimprovise());
        return DOTween.To(() => 0, (x) => x = 0, 0, 0.1f);
    }

    private IEnumerator TitomFaitDesTimeScaleCradoAlorsJimprovise()
    {
        yield return new WaitForSeconds(0.1f);
        _music.SetGameValue(1);
        RuntimeManager.PlayOneShot(_321Sound);
    }
}
