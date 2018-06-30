using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        public Transform LeftTransform;
        public Transform RightTransform;

        public Vector2 Bounds = new Vector2(0, 30.0f);
        public Tower TowerPrefab;

        public Piece[] PiecesPrefabs;
        public int PiecesBufferCount;

        private readonly List<Tower> m_towers = new List<Tower>();
        private readonly List<Piece> m_piecesPrefabBuffer = new List<Piece>();

        protected void LateUpdate()
        {
            float maxHeight = float.MinValue;
            foreach (Tower tower in m_towers)
            {
                maxHeight = Mathf.Max(maxHeight, tower.MaxHeight);
            }
            LeftTransform.localPosition = new Vector3(Bounds.x, maxHeight);
            RightTransform.localPosition = new Vector3(Bounds.y, maxHeight);
        }

        public void PullBufferPieces()
        {
            m_piecesPrefabBuffer.Clear();
            for (int i = 0; i < PiecesBufferCount; i++)
            {
                m_piecesPrefabBuffer.Add(PiecesPrefabs[Random.Range(0, PiecesPrefabs.Length)]);
            }
        }

        public void SetTowerCount(int count)
        {
            ClearTowers();
            float diff = Bounds.y - Bounds.x;
            for (int i = 0; i < count; i++)
            {
                m_towers.Add(Instantiate(TowerPrefab));
                m_towers[i].transform.localPosition = new Vector3((diff /(float)count) * i  + Bounds.x, 0, 0);
            }
        }

        protected void ClearTowers()
        {
            foreach (Tower tower in m_towers)
            {
                Destroy(tower.gameObject);
            }
            m_towers.Clear();
        }
    }
}
