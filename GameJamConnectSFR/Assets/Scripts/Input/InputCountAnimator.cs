using UnityEngine;

namespace Project.Scripts.Input
{
    public class InputCountAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private string m_argString = "InputCount";

        private int m_argHash;

        private void Start()
        {
            m_argHash = Animator.StringToHash(m_argString);

            PlayerInputManager.Instance.OnInputFound.AddListener(OnPlayerInputCountChanged);
            PlayerInputManager.Instance.OnInputLost.AddListener(OnPlayerInputCountChanged);
            OnPlayerInputCountChanged(0);
        }

        private void OnDestroy()
        {
            PlayerInputManager.Instance.OnInputFound.RemoveListener(OnPlayerInputCountChanged);
            PlayerInputManager.Instance.OnInputLost.RemoveListener(OnPlayerInputCountChanged);
        }

        protected void OnPlayerInputCountChanged(int index)
        {
            m_animator.SetInteger(m_argHash, PlayerInputManager.Instance.PlayerCount);
        }
    }
}
