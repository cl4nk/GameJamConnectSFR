using System;
using System.Collections;
using Project.Scripts.Character;
using Project.Scripts.GameFlow;
using Project.Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Controller
{
	public class ControllerIndicator : MonoBehaviour
	{
	    public enum ControllerState
	    {
            Normal,
            Switching,
            Switched
	    }

	    private ControllerState m_currentControllerState;
	    public ControllerState CurrentControllerState
	    {
	        get { return m_currentControllerState; }
	        set
	        {
	            m_currentControllerState = value;
	            switch (m_currentControllerState)
	            {
	                case ControllerState.Normal:
	                    m_textMeshPro.gameObject.SetActive(false);
	                    m_playerIndicatorSprite.gameObject.SetActive(true);
	                    StopFlicker();
                        StopCoroutine();
                        break;
                    case ControllerState.Switching:
	                    m_textMeshPro.gameObject.SetActive(false);
	                    m_playerIndicatorSprite.gameObject.SetActive(true);
                        if (m_flicker)
                        {
                            m_flicker.LaunchFlicker();
                        }
                        StopCoroutine();
                        break;
	                case ControllerState.Switched:
                        StopFlicker();
	                    ShowIndicator();
                        break;
	                default:
	                    throw new ArgumentOutOfRangeException();
	            }
	        }
	    }
        
        [SerializeField]
		private float m_showDuration = 2.0f;
		public string PlayerControllerTextFormat = "P{0}";
		public string AIControllerText = "AI";

		[SerializeField]
		private TMP_Text m_textMeshPro;
        [SerializeField]
		private Image m_playerIndicatorSprite;
		[SerializeField]
		private Pawn m_pawn;

		private Coroutine m_coroutine;
        private FlickerEnable m_flicker;
        private WaitForSeconds m_waitDelay;

		private void Awake()
		{
			if ( m_textMeshPro == null )
			{
				Debug.LogWarning( "ControllerIndicator.Awake() - Text Mesh Pro is null, trying to GetComponent()" );
				m_textMeshPro = GetComponent<TextMeshPro>();
				if ( m_textMeshPro == null )
				{
					Debug.LogError( "ControllerIndicator.Awake() - Text Mesh Pro is null" );
				}
			}

			if ( m_pawn == null )
			{
				Debug.LogError( "ControllerIndicator.Awake() - Pawn is null, need to be set in Editor" );
			}

		    m_waitDelay = new WaitForSeconds(m_showDuration);
        }

		private void OnEnable()
		{
		    m_pawn.OnControllerChanged.AddListener(SetSwitched);
		    if (m_flicker)
		    {
		        m_flicker.OnEnableState.AddListener(FlickerIndicator);
		    }
            ShowIndicator();
		}

	    private void OnDisable()
	    {
		    m_pawn.OnControllerChanged.RemoveListener(SetSwitched);
	        if (m_flicker)
	        {
	            m_flicker.OnEnableState.RemoveListener(FlickerIndicator);
	        }
        }

        public void Start()
		{
			ShowIndicator();
		}

	    private void SetSwitched()
	    {
	        CurrentControllerState = ControllerState.Switched;
	    }

		private void ShowIndicator()
		{
		    if (enabled && gameObject.activeInHierarchy)
		    {
		        LevelFlowController levelFlowController;
		        LevelFlowController.TryGetInstance(out levelFlowController);
		        if (levelFlowController)
		        {
		            StopCoroutine();
		            m_coroutine = StartCoroutine(ShowIndicatorCoroutine());
		        }
            }
		}

		private IEnumerator ShowIndicatorCoroutine()
		{
			PlayerController playerController = m_pawn.PossessingController as PlayerController;

			m_textMeshPro.text = playerController != null
				? string.Format( PlayerControllerTextFormat, playerController.CorrectedInputNum )
				: AIControllerText;
			m_textMeshPro.color = LevelFlowController.Instance.GetControllerColor( m_pawn );
		    m_playerIndicatorSprite.color = m_textMeshPro.color;

		    m_playerIndicatorSprite.gameObject.SetActive(false);
		    m_textMeshPro.gameObject.SetActive(true);
            yield return m_waitDelay;

			m_coroutine = null;
		    CurrentControllerState = ControllerState.Normal;
		}

	    public void FlickerIndicator(bool show)
	    {
	        m_playerIndicatorSprite.gameObject.SetActive(show);
        }

        public void ShowIndicators(bool show)
	    {
	        m_playerIndicatorSprite.gameObject.SetActive(show);
	        m_textMeshPro.gameObject.SetActive(show);
	        m_textMeshPro.enabled = !show;
        }

	    protected void StopCoroutine()
	    {
	        if (m_coroutine != null)
	        {
	            StopCoroutine(m_coroutine);
	            m_coroutine = null;
	        }
	    }

	    protected void StopFlicker()
	    {
	        if (m_flicker)
	        {
                m_flicker.StopFlicker();
	        }
	    }
    }
}