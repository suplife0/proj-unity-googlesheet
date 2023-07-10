using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTargetController : MonoBehaviour
{
    public void MyCustomEvent(AnimationEvent animEvent)
    {
        Debug.Log($"Animation 이벤트 발생! Time: {animEvent.time}");
    }
}
