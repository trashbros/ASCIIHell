using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlowDownTimer
{
    
    #region Coroutines
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to unload.
    public static IEnumerator RuneSlowDownTimer()
    {
        if (GameplayParameters.instance.SlowDowns > 0)
        {
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.SLOW_TIME, new object[1] { true });

            Debug.Log("Time is slowed!");

            yield return new WaitForSeconds(GameplayParameters.instance.SlowDownTime);

            CustomEvents.EventUtil.DispatchEvent(CustomEventList.SLOW_TIME, new object[1] { false });
            Debug.Log("Time is back to normal!");
        }
        yield break;
    }
    #endregion
}
