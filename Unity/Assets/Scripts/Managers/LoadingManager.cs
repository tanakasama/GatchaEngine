using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    /// <summary>
    /// The callback for when our loading is complete
    /// </summary>
    private Action _onComplete;
    /// <summary>
    /// The progress function we are using for progressing
    /// </summary>
    private Func<float> _progress;
    /// <summary>
    /// This is the current coroutine we are using for the progress
    /// </summary>
    private Coroutine _progressCoroutine;

    /// <summary>
    /// The content for the loading manager
    /// </summary>
    [SerializeField]
    private GameObject _content;

    /// <summary>
    /// How long do we transition to the blackscreen
    /// </summary>
    [SerializeField]
    private float _transitionBlackScreen = 1f;
    /// <summary>
    /// The blackscreen canvas (this is just a generic fader we are using)
    /// </summary>
    [SerializeField]
    private CanvasGroup _blackScreen;
    /// <summary>
    /// The loading screen canvas
    /// </summary>
    [SerializeField]
    private CanvasGroup _loadingScreen;
    /// <summary>
    /// The current progress text
    /// </summary>
    [SerializeField]
    private Text _progressText;
    /// <summary>
    /// The current progress slider
    /// </summary>
    [SerializeField]
    private Slider _progressSlider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //This is just a simple test to showcase the loading manager working
            float start = Time.time;
            float max = 5;
            Setup(() =>
            {
                Debug.Log("Loading Complete");
            },
            () =>
            {
                float delta = (Time.time - start) / max;
                return delta > 1 ? 1 : delta;
            });
        }
    }

    /// <summary>
    /// Setup our loading manager
    /// </summary>
    /// <param name="onComplete">The callback we will be using when we finish</param>
    /// <param name="Progress">The progress function</param>
    public void Setup(Action onComplete, Func<float> Progress)
    {
        _onComplete = onComplete;
        _progress = Progress;

        _blackScreen.alpha = 0;
        _loadingScreen.alpha = 1;
        _content.SetActive(true);

        if (_progressCoroutine != null)
            StopCoroutine(_progressCoroutine);

        _progressCoroutine = StartCoroutine(CheckProgress());
    }

    /// <summary>
    /// Iterate through the progress until we are finished
    /// </summary>
    private IEnumerator CheckProgress()
    {
        yield return null;
        float currentProgress = 0;

        while (currentProgress < 1f)
        {
            currentProgress = _progress();
            yield return null;
            _progressText.text = currentProgress.ToString("p1");
            _progressSlider.value = currentProgress;
        }

        if (_onComplete != null)
            _onComplete();

        currentProgress = 0;

        while (currentProgress < _transitionBlackScreen)
        {
            currentProgress += Time.deltaTime;
            _blackScreen.alpha = currentProgress / _transitionBlackScreen;
            yield return null;
        }

        _blackScreen.alpha = 1;
        _loadingScreen.alpha = 0;

        currentProgress = 0;

        while (currentProgress < _transitionBlackScreen)
        {
            currentProgress += Time.deltaTime;
            _blackScreen.alpha = _transitionBlackScreen - (currentProgress / _transitionBlackScreen);
            yield return null;
        }

        _blackScreen.alpha = 0;
        _content.SetActive(false);
    }
}
