using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimationEventController))]
public class AnimationEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimationEventController animationController = (AnimationEventController)target;

        // 애니메이션 클립 목록을 가져오고 각 클립에 대한 이벤트를 추가합니다
        if (GUILayout.Button("Add Event"))
        {
            animationController.SetClips();

            // 수정된 애니메이션 클립을 저장합니다
            EditorUtility.SetDirty(animationController);
            AssetDatabase.SaveAssets();
        }
    }
}

public class AnimationEventController : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트

    [SerializeField] private int wantFrame = 30;

    void Start()
    {
        
    }

    public void SetClips()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator 컴포넌트가 없습니다.");
            return;
        }

        // AnimatorController에서 모든 AnimatorState 검색
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        if (controller == null)
        {
            Debug.LogWarning("AnimatorController가 없습니다.");
            return;
        }

        for (int i = 0; i < controller.layers.Length; i++)
        {
            AnimatorStateMachine stateMachine = controller.layers[i].stateMachine;
            TraverseStateMachine(stateMachine);
        }
    }

    void TraverseStateMachine(AnimatorStateMachine stateMachine)
    {
        // StateMachine의 모든 State 검색
        for (int i = 0; i < stateMachine.states.Length; i++)
        {
            AnimatorState state = stateMachine.states[i].state;

            // AnimatorState에 연결된 클립 가져오기
            AnimationClip clip = state.motion as AnimationClip;
            if (clip != null)
            {
                // AnimatorState의 이름과 클립의 프레임 수 출력
                Debug.Log($"State: {state.name}, Clip: {clip.name}, Clip Frame Count: {clip.length * clip.frameRate}");

                // 이벤트 등록
                AnimationEvent animEvent = new AnimationEvent();
                animEvent.functionName = "MyCustomEvent"; // 호출할 함수 이름
                animEvent.time = wantFrame / clip.frameRate; // 이벤트가 발생할 프레임(시간)
                AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[] { animEvent });
            }
        }

        // StateMachine의 모든 Sub-StateMachine 검색
        for (int i = 0; i < stateMachine.stateMachines.Length; i++)
        {
            AnimatorStateMachine subStateMachine = stateMachine.stateMachines[i].stateMachine;
            TraverseStateMachine(subStateMachine);
        }
    }

    // AnimationEvent에서 호출할 함수
    public void MyCustomEvent(AnimationEvent animEvent)
    {
        Debug.Log($"Animation 이벤트 발생! Time: {animEvent.time}");
    }
}
