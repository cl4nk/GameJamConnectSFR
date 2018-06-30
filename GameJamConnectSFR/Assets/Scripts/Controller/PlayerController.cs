using System;
using System.Collections;
using Project.Scripts.Input;
using Project.Scripts.StateMachineBehaviour;
using UnityEngine;
using UnityEngine.Events;
//using XInputDotNetPure;

namespace Project.Scripts.Controller
{
    public class PlayerController : ABaseController
    {
        [Serializable]
        public class ActionButton
        {
            public GamepadInput.XboxControllerButtons Button;
            [HideInInspector, NonSerialized]
            public KeyCode Key;
            [HideInInspector, NonSerialized]
            public bool Pressed;
        }

        [Serializable]
        public class TowerActionButton : ActionButton
        {
            //public ArmedPawn.ArmedPawnAction Action;
        }

        [Serializable]
        public class GenericActionButton : ActionButton
        {
            public UnityEvent OnPressed;
            public UnityEvent OnHold;
            public UnityEvent OnReleased;
        }

        [Header("Axis/Button names")]
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public float AxisThresold = 0.8f;
    
        [Tooltip("Used for multiplayer")]
        public bool UseSuffix = true;

        [SerializeField] private ActionButton m_pawnActionButton;
        [SerializeField] private TowerActionButton[] m_armedPawnActionButtons;
        [SerializeField] private GenericActionButton[] m_genericActionButtons;


        private int m_inputNum = -1;
        public int InputNum
        {
            get { return m_inputNum; }
            set
            {
                if (m_inputNum != value)
                {
                    m_inputNum = value;
                    CorrectedInputNum = value + 1;

                    m_pawnActionButton.Key = GamepadInput.GetKeyCode(m_pawnActionButton.Button, m_inputNum);

                    foreach (TowerActionButton handler in m_armedPawnActionButtons)
                    {
                        handler.Key = GamepadInput.GetKeyCode(handler.Button, m_inputNum);
                    }

                    foreach (GenericActionButton handler in m_genericActionButtons)
                    {
                        handler.Key = GamepadInput.GetKeyCode(handler.Button, m_inputNum);
                    }
                }
            }
        }

        public int CorrectedInputNum { get; private set; }

        //private ArmedPawn m_armedPawn;

        protected override void Awake()
        {
            base.Awake();

            m_pawnActionButton.Key = GamepadInput.GetKeyCode(m_pawnActionButton.Button, m_inputNum);

            foreach (TowerActionButton handler in m_armedPawnActionButtons)
            {
                handler.Key = GamepadInput.GetKeyCode(handler.Button, m_inputNum);
            }

            foreach (GenericActionButton handler in m_genericActionButtons)
            {
                handler.Key = GamepadInput.GetKeyCode(handler.Button, m_inputNum);
            }
        }

        public void Start()
        {
            //InputNum = PlayerInputManager.Instance.Register(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PlayerInputManager.Instance.Unregister(this);

            if (m_vibrationCoroutine != null)
            {
                StopCoroutine(m_vibrationCoroutine);
                m_vibrationCoroutine = null;

                if (InputNum > -1 && InputNum < 5)
                {
                    //GamePad.SetVibration((PlayerIndex)InputNum, 0.0f, 0.0f);
                }
            }
        }
        /*
        protected override void Possess(Pawn pawn)
        {
            PlayerInputManager.Instance.SetUsed(this, true);
            SceneLinkedSMB<PlayerController>.Initialise(pawn.Animator, this);

            m_armedPawn = pawn as ArmedPawn;
        }
        */
        protected override void Unpossess()
        {
            PlayerInputManager.Instance.SetUsed(this, false);
         //   m_armedPawn = null;
        }

        protected override void ControllerUpdate()
        {
       /*     PossessedPawn.InputMove(new Vector2(UnityEngine.Input.GetAxis(GetInputName(HorizontalAxis)), UnityEngine.Input.GetAxis(GetInputName(VerticalAxis))));

            if (UnityEngine.Input.GetKeyDown(m_pawnActionButton.Key))
            {
                PossessedPawn.LaunchPawnAction();
            }

            if (m_armedPawn)
            {
                foreach (TowerActionButton handler in m_armedPawnActionButtons)
                {
                    if (handler.Pressed)
                    {
                        if (UnityEngine.Input.GetKeyUp(handler.Key))
                        {
                            handler.Pressed = false;
                            m_armedPawn.SetArmedPawnActionPressed(handler.Action, handler.Pressed);
                        }
                    }
                    else if (UnityEngine.Input.GetKeyDown(handler.Key))
                    {
                        handler.Pressed = true;
                        m_armedPawn.SetArmedPawnActionPressed(handler.Action, handler.Pressed);
                    }
                }
            }

            foreach (GenericActionButton handler in m_genericActionButtons)
            {
                if (handler.Pressed)
                {
                    if (UnityEngine.Input.GetKey(handler.Key))
                    {
                        handler.OnHold.Invoke();
                    }
                    else
                    {
                        handler.OnReleased.Invoke();
                        handler.Pressed = false;
                    }
                }
                else if(UnityEngine.Input.GetKeyDown(handler.Key))
                {
                    handler.OnPressed.Invoke();
                    handler.Pressed = true;
                }
            }*/
        }

        protected Coroutine m_vibrationCoroutine;
        protected float m_endTime;

        public override void TryVibrate(float duration, float leftPower, float rightPower)
        {
            if (InputNum > -1 && InputNum < 5)
            {
                if (Time.time + duration > m_endTime)
                {
                    if (m_vibrationCoroutine != null)
                    {
                        StopCoroutine(m_vibrationCoroutine);
                        m_vibrationCoroutine = null;
                    }

                    StartCoroutine(VibrationCoroutine(duration, leftPower, rightPower));
                }
            }
        }

        protected IEnumerator VibrationCoroutine(float duration, float leftPower, float rightPower)
        {
            //GamePad.SetVibration((PlayerIndex)InputNum, leftPower, rightPower);
            yield return new WaitForSeconds(duration);
            //GamePad.SetVibration((PlayerIndex)InputNum, 0.0f, 0.0f);
            m_vibrationCoroutine = null;
        }

        private string GetInputName(string input)
        {
            return UseSuffix ? string.Format(PlayerInputManager.AxisFormat, input, CorrectedInputNum) : input;
        }
    }
}
