using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimescaleManager : Singleton<TimescaleManager>
{
    public AnimationCurve[] TimeCurves;
    public bool gamePaused;
    [SerializeField]
    private float finalScale = 1;
    [Header("Debug")]
    public int selectedCurve = 0;
    public float curveDuration = 1;

    public int fixedFrameCount { get; private set; }
    public int trueFrameCount { get; private set; }
    public AudioMixer musicMixer;

    public float masterTimescale
    {
        get
        {
            return gamePaused ? 0.0f : finalScale;
        }
    }

    public float masterDeltaTime
    {
        get
        {
            return Time.unscaledDeltaTime * masterTimescale;
        }
    }

    void Update()
    {
        ++trueFrameCount;
        //if (!gamePaused && !Application.isFocused && PlayerSettings.Instance.settingsBool["UnfocusPause"]) UIManager.Instance.PauseGame(true);
        //finalScale = 1;
    }

    void FixedUpdate()
    {
        ++fixedFrameCount;
    }

    void LateUpdate()
    {
        if (finalScale != 1 && masterTimescale != 0)
        {
            Time.timeScale = finalScale * masterTimescale;
        }
        else
            Time.timeScale = masterTimescale;
        //musicMixer.SetFloat("musicLowpass", Mathf.Lerp(1000f, 22000f, Time.timeScale));
    }

    public void ScaleTime(int curve, float duration)
    {
        StartCoroutine(TimeScale(curve, duration));
    }

    //[BitStrap.Button]
    void TestScale()
    {
        StartCoroutine(TimeScale(selectedCurve, curveDuration));
    }

    public void FreezeFrame(float duration = 0.5f)
    {
        StartCoroutine(FreezeFrameRoutine(duration));
    }

    private IEnumerator FreezeFrameRoutine(float duration)
    {
        Time.timeScale = 0;
        finalScale = 0;
        AnimatorUpdateMode playerUpdateMode = AnimatorUpdateMode.Normal;
        //if (PlayerData.Instance.playerAnimator.updateMode == AnimatorUpdateMode.UnscaledTime) playerUpdateMode = AnimatorUpdateMode.Normal;
        //PlayerData.Instance.playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(duration);
        //PlayerData.Instance.playerAnimator.updateMode = playerUpdateMode;
        Time.timeScale = 1;
        finalScale = 1;
    }

    private IEnumerator TimeScale(int curve, float duration)
    {
        float t = 0;
        while (t < 1)
        {
            finalScale = Mathf.Min(finalScale, TimeCurves[curve].Evaluate(t));
            t += masterDeltaTime / duration;
            yield return null;
        }
    }
}
