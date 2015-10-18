using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.Common.Interaction
{
    public class StringInputController
    {
        private readonly List<Keys> _lastPressedKeys;
        private string[] _validCharacterCodes;

        public InputType InputType { get; private set; }
        public int MaxInputCharacters { get; set; }
        public string CurrentText { get; set; }

        public StringInputController(InputType inputType)
        {
            _lastPressedKeys = new List<Keys>();
            MaxInputCharacters = 256;
            CurrentText = string.Empty;
            SetInputType(inputType);
        }

        public void SetInputType(InputType inputType)
        {
            if (inputType == InputType) return;
            InputType = inputType;
            switch (inputType)
            {
                case InputType.HostNames:
                    _validCharacterCodes = new[] { Keys.OemPeriod.ToString(), "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
                    break;
                case InputType.AlphaNumeric:
                    _validCharacterCodes = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
                    break;
                case InputType.Numeric:
                    _validCharacterCodes = new[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
                    break;
                case InputType.IpAddress:
                    _validCharacterCodes = new[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", Keys.OemPeriod.ToString() };
                    break;
            }
        }

        private static bool IsAnyShiftKeyDown(KeyboardEx keyboard)
        {
            return keyboard.IsKeyDownOnce(Keys.RightShift) || keyboard.IsKeyDown(Keys.LeftShift);
        }

        public void Update(KeyboardEx keyboard, float elapsedTime)
        {
            if (keyboard.IsKeyDownOnce(Keys.Back) && CurrentText.Length > 0)
            {
                CurrentText = CurrentText.Substring(0, CurrentText.Length - 1);
            }
            else if (CurrentText.Length < MaxInputCharacters)
            {
                var allPressedKeys = keyboard.GetPressedKeys();

                var allValidPressedKeys = allPressedKeys.Where(k => _validCharacterCodes.Contains(k.ToString())).ToArray();
                foreach (var pressedKey in allValidPressedKeys)
                {
                    if (_lastPressedKeys.Contains(pressedKey)) continue;
                    _lastPressedKeys.Add(pressedKey);

                    var pressed = pressedKey.ToString();

                    if (!IsAnyShiftKeyDown(keyboard)) pressed = pressed.ToLower();

                    if (pressed.Equals("space"))
                    {
                        pressed = " ";
                    }
                    else if (pressed.Length == 2 && pressed[0] == 'd')
                    {
                        pressed = pressed.Substring(1);
                    }

                    if (InputType == InputType.Numeric && pressed == "d") break;

                    if (pressedKey == Keys.OemPeriod)
                    {
                        CurrentText += ".";
                    }
                    else
                    {
                        CurrentText += pressed;
                    }
                }

                _lastPressedKeys.Clear();
                _lastPressedKeys.AddRange(allValidPressedKeys);
            }
        }
    }
}
