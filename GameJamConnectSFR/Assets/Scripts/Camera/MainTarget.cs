using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Camera
{
    [RequireComponent(typeof(Cinemachine.CinemachineTargetGroup))]
    public class MainTarget : Singleton<MainTarget>
    {
        [SerializeField] private float m_weight = 1.0f;
        [SerializeField] private float m_radius = 1.0f;

        private CinemachineTargetGroup m_targetGroup = null;
        public CinemachineTargetGroup TargetGroup
        {
            get
            {
                return m_targetGroup ?? (m_targetGroup = GetComponent<CinemachineTargetGroup>());
            }
        }
    }
}
