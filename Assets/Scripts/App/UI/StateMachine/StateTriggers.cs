using UnityEngine;
using UnityEngine.Events;

namespace App.UI.StateMachine
{
    class StateTriggers : StateMachineBehaviour
    {
        public UnityEvent onStateEnter = new UnityEvent();
        public UnityEvent onStateExit = new UnityEvent();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onStateEnter.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onStateExit.Invoke();
        }
    }
}
