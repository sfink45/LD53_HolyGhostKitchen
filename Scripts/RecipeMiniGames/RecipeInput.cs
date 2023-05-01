using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RecipeMiniGames
{
    public class RecipeInput 
    {
        public enum KeyboardActions { Empty, Down, Up, Left, Right, CircleLeft, CircleRight}
        private static readonly List<KeyboardActions> CircleRightOrder = new List<KeyboardActions>  { KeyboardActions.Up, KeyboardActions.Left, KeyboardActions.Down, KeyboardActions.Right };
        private static readonly List<KeyboardActions> CircleLeftOrder = new List<KeyboardActions> { KeyboardActions.Up, KeyboardActions.Right, KeyboardActions.Down, KeyboardActions.Left };

        public static bool IsMoveLeft() => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        public static bool IsMoveRight() => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        public static bool IsChopAction() => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        public static bool IsPressedLeft() => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        public static bool IsPressedRight() => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);

        public static bool IsPressedDown() => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        public static bool IsPressedUp() => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        public static KeyboardActions GetActionFromKeyCode()
        {
                if(IsPressedDown())
                    return KeyboardActions.Down;
                else if(IsPressedLeft())
                    return KeyboardActions.Left;
                else if(IsPressedRight())
                    return KeyboardActions.Right;
                else if(IsPressedUp())
                    return KeyboardActions.Up;
                else
                    return KeyboardActions.Empty;
        }

        public static bool IsKeyboardAction(KeyboardActions input)
        {
            switch(input)
            {
                case KeyboardActions.Down:
                    return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

                case KeyboardActions.Left:
                    return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

                case KeyboardActions.Right:
                    return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

                case KeyboardActions.Up:
                    return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
                default:
                    return false;

            }
        }

        public static KeyboardActions GetNextInCircle(KeyboardActions currentInput, KeyboardActions circleType)
        {

            if (circleType == KeyboardActions.CircleLeft) return CircleLeftOrder[(CircleLeftOrder.IndexOf(currentInput) + 1) % 4];
            if (circleType == KeyboardActions.CircleRight) return CircleRightOrder[(CircleRightOrder.IndexOf(currentInput) + 1) % 4];

            throw new ArgumentException("Must be circle type action");

        }
    }
}
