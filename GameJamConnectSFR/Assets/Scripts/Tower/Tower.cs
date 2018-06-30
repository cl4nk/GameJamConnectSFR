using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Tower : MonoBehaviour
    {
        private float m_maxHeight;

        public float MaxHeight
        {
            get { return m_maxHeight; }
        }

        private Piece m_fallingPiece;
        protected List<Piece> m_currentPieces = new List<Piece>();

        protected void LateUpdate()
        {
            m_maxHeight = float.MinValue;
            foreach (Piece currentPiece in m_currentPieces)
            {
                m_maxHeight = Mathf.Max(m_maxHeight, currentPiece.Collider.bounds.max.y);
            }
        }
    }
}
