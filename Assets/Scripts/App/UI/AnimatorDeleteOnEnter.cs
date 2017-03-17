using UnityEngine;

namespace App.UI
{
    class AnimatorDeleteOnEnter : StateMachineBehaviour
    {

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject.transform.root.gameObject);
        }
    }
}
