using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Input
{
    public class PlayerInputManager : Singleton<PlayerInputManager>
    {
        public const int MaxGamepadCount = 8;

        [Serializable]
        public class InputFoundEvent : UnityEvent<int>
        { }

        public InputFoundEvent OnInputFound = new InputFoundEvent();
        public InputFoundEvent OnPressedOnInputFound = new InputFoundEvent();
        public InputFoundEvent OnInputLost = new InputFoundEvent();

        public GamepadInput.XboxControllerButtons RegisterInputButton = GamepadInput.XboxControllerButtons.Start;

        private KeyCode[] m_registerInputKeyCodes;

        public const string AxisFormat = "{0}_{1}";

        private string m_playerControllerPath = "Controllers/DefaultPlayerController";
        private MonoBehaviour m_playerControllerPrefab;
        private MonoBehaviour[] m_inputs;
        private readonly List<int> m_sortByRegisterTime = new List<int>();
        private bool[] m_used;

        public int PlayerCount
        {
            get
            {
                return m_sortByRegisterTime.Count;
            }
        }

        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            m_playerControllerPrefab = Resources.Load<MonoBehaviour>(m_playerControllerPath);
            m_inputs = new MonoBehaviour[MaxGamepadCount];
            m_used = new bool[MaxGamepadCount];

            Cursor.visible = false;
        }

        private void OnEnable()
        {
            m_registerInputKeyCodes = new KeyCode[MaxGamepadCount];

            for (int i = 0; i < MaxGamepadCount; ++i)
            {
                m_registerInputKeyCodes[i] = GamepadInput.GetKeyCode(RegisterInputButton, i);
            }
        }

        public void Update()
        {
            string[] joystickNames = UnityEngine.Input.GetJoystickNames();

            for (int i = 0; i < MaxGamepadCount; ++i)
            {
                //Test if has no input on index i
                if (i >= joystickNames.Length || joystickNames[i].Trim().Length == 0)
                {
                    //If no input found by Unity bu we have got a Player Controller...
                    if (m_inputs[i] != null)
                    {
                        //...unregister it, because we lost it
                        Unregister(m_inputs[i]);
                        continue;
                    }
                }

                if (UnityEngine.Input.GetKeyUp(m_registerInputKeyCodes[i]))
                {
                    if (m_inputs[i] == null)
                    {
                        m_inputs[i] = Instantiate(m_playerControllerPrefab, transform);
                        m_inputs[i].gameObject.name = "PlayerController_" + i;
                        //m_inputs[i].InputNum = i;
                        OnInputFound.Invoke(i);
                    }
                    else
                    {
                        OnPressedOnInputFound.Invoke(i);
                    }
                }
            }
        }

        public bool HasPlayerController(int index)
        {
            return m_inputs[index] != null;
        }

        public bool GetFreeController(out MonoBehaviour controller)
        {
            foreach (int i in m_sortByRegisterTime)
            {
                if (m_inputs[i] && m_used[i] == false)
                {
                    controller = m_inputs[i];
                    m_used[i] = true;
                    return m_inputs[i] != null;
                }
            }

            controller = null;
            return false;
        }

        public int Register(MonoBehaviour input)
        {
            Debug.Assert(input != null);

            int index = 0
                //input.InputNum
                ;

            if (m_inputs[index] == null || m_inputs[index] == input)
            {
                m_inputs[index] = input;
                if (!m_sortByRegisterTime.Contains(index))
                {
                    m_sortByRegisterTime.Add(index);
                }
                OnInputFound.Invoke(index);
                return index;
            }

            Destroy(input.gameObject);
            throw new IndexOutOfRangeException("Input already exits");
        }

        public void Unregister(MonoBehaviour input)
        {
            Debug.Assert(input != null);

            for (int i = 0; i < m_inputs.Length; ++i)
            {
                if (m_inputs[i] == input)
                {
                    m_inputs[i].enabled = false;
                    m_inputs[i] = null;
                    m_used[i] = false;
                    m_sortByRegisterTime.Remove(i);

                    OnInputLost.Invoke(i);
                    return;
                }
            }
        }

        public void SetUsed(MonoBehaviour controller, bool used)
        {
            for (int i = 0; i < m_inputs.Length; ++i)
            {
                if (m_inputs[i] == controller)
                {
                    m_used[i] = used;
                    return;
                }
            }
        }

        public MonoBehaviour GetPlayerController(int index)
        {
            return m_inputs[index];
        }

        public int GetControllerIndexByRegisterTime(int index)
        {
            return index < m_sortByRegisterTime.Count ? m_sortByRegisterTime[index] : -1;
        }

        public void SetControllerPosition(int controllerNum, int position)
        {
            if (m_sortByRegisterTime.Contains(controllerNum))
            {
                m_sortByRegisterTime.Remove(controllerNum);
                m_sortByRegisterTime.Insert(position, controllerNum);
            }
        }

        public void ForceControllerDestroy(int index)
        {
            if (m_inputs[index])
            {
                Destroy(m_inputs[index]);
            }
        }

        public void ResetAll()
        {
            for (int i = 0; i < MaxGamepadCount; ++i)
            {
                if (m_inputs[i] != null)
                {
                    Unregister(m_inputs[i]);
                }
            }
        }
    }
}
