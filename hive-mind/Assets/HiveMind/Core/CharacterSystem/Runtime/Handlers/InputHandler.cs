using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Handlers
{
    public sealed class InputHandler
    {
        #region Fields
        private Vector2 movementInputValue;
        private Vector2 rotationInputValue;
        private bool runKeyPressed;
        #endregion

        #region Getters
        public Vector2 MovementInputValue => movementInputValue;
        public Vector2 RotationInputValue => rotationInputValue;
        public bool RunKeyPressed => runKeyPressed;
        #endregion

        #region Executes
        public void Input(InputData inputData)
        {
            switch (inputData.InputStyle)
            {
                case InputStyles.PC:
                    PCInput();
                    break;
                case InputStyles.Mobile:
                    MobileInput();
                    break;
            }
        }
        private void PCInput()
        {
        }
        private void MobileInput()
        {
        }
        #endregion
    }
}
