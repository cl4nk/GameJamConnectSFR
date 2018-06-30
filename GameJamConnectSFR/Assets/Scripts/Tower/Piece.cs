using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Tower
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Piece : MonoBehaviour
    {
        public UnityEvent OnHasFallen;

        private bool m_hasFallen;
        public bool HasFallen
        {
            get { return m_hasFallen; }
        }

        private Collider2D m_collider;
        public Collider2D Collider
        {
            get
            {
                if (m_collider == null)
                {
                    m_collider = GetComponent<Collider2D>();
                }

                return m_collider;
            }
        }

        private Rigidbody2D m_rigidBody;
        public Rigidbody2D Rigidbody
        {
            get
            {
                if (m_rigidBody == null)
                {
                    m_rigidBody = GetComponent<Rigidbody2D>();
                }

                return m_rigidBody;
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!m_hasFallen)
            {
                m_hasFallen = true;
                OnHasFallen.Invoke();
            }
        }
    }
}
