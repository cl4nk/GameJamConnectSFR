using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Input
{
    public class GamepadInput
    {
        public enum XboxControllerButtons
        {
            None,
            A,
            B,
            X,
            Y,
            Leftstick,
            Rightstick,
            Back,
            Start,
            LeftBumper,
            RightBumper,
        }

        protected static readonly Dictionary<int, int> k_ButtonsToKeycode_win = new Dictionary<int, int>
        {
            {(int)XboxControllerButtons.A, 0},
            {(int)XboxControllerButtons.B, 1},
            {(int)XboxControllerButtons.X, 2},
            {(int)XboxControllerButtons.Y, 3},
            {(int)XboxControllerButtons.Leftstick, 8},
            {(int)XboxControllerButtons.Rightstick, 9},
            {(int)XboxControllerButtons.Back, 6},
            {(int)XboxControllerButtons.Start, 7},
            {(int)XboxControllerButtons.LeftBumper, 4},
            {(int)XboxControllerButtons.RightBumper, 5},
        };

        protected static readonly Dictionary<int, int> k_ButtonsToKeycode_mac = new Dictionary<int, int>
        {
            {(int)XboxControllerButtons.A, 16},
            {(int)XboxControllerButtons.B, 17},
            {(int)XboxControllerButtons.X, 18},
            {(int)XboxControllerButtons.Y, 19},
            {(int)XboxControllerButtons.Leftstick, 11},
            {(int)XboxControllerButtons.Rightstick, 12},
            {(int)XboxControllerButtons.Back, 10},
            {(int)XboxControllerButtons.Start, 9},
            {(int)XboxControllerButtons.LeftBumper, 13},
            {(int)XboxControllerButtons.RightBumper, 14},
        };

        public static KeyCode GetKeyCode(XboxControllerButtons Button, int index)
        {
            if (index < -1)
            {
                return KeyCode.A;
            }
            else
            {
                return KeyCode.Joystick1Button0 + (index * 20)
#if UNITY_STANDALONE_WIN
                       + k_ButtonsToKeycode_win[(int) Button]
#elif UNITY_STANDALONE_OSX
                   + k_ButtonsToKeycode_mac[(int) Button]
#endif
                    ;
            }
        }
    }
}
