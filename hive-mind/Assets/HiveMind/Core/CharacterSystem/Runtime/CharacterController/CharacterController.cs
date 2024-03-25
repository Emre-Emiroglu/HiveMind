using HiveMind.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.CharacterSystem.Runtime.Handlers;
using UnityEngine;

namespace HiveMind.CharacterSystem.Runtime.Character
{
    public sealed class CharacterController
    {
        #region Fields
        private readonly CharacterSettings characterSettings;
        private readonly InputHandler inputHandler;
        private readonly MovementHandler movementHandler;
        private readonly RotationHandler rotationHandler;
        #endregion

        #region Constructor
        public CharacterController(CharacterSettings characterSettings, InputHandler inputHandler, MovementHandler movementHandler, RotationHandler rotationHandler)
        {
            this.characterSettings = characterSettings;
            this.inputHandler = inputHandler;
            this.movementHandler = movementHandler;
            this.rotationHandler = rotationHandler;
        }
        #endregion

        #region Executes
        public void Input() => inputHandler?.Input(characterSettings.InputData);
        public void Move(Transform transform) => movementHandler?.Move(transform, inputHandler.MovementInputValue, inputHandler.RunKeyPressed, characterSettings.MovementData);
        public void Rotate(Transform transform) => rotationHandler?.Rotate(transform, inputHandler.RotationInputValue, characterSettings.RotationData);
        #endregion
    }
}
