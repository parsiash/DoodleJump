using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface IWorld
    {
        float LeftEdgeX { get; }
        float RightEdgeX { get; }
        CharacterController Character { get; }

        void OnStart();
        void Reset();
    }
    
    public class World : IWorld
    {
        public float LeftEdgeX => -2f;
        public float RightEdgeX => 2f;

        private CharacterController _character;
        public CharacterController Character => _character;

        public World(CharacterController character)
        {
            _character = character;
        }

        public void OnStart()
        {
            _character.Init(this);
        }

        public void Reset()
        {
            _character.Reset();
        }

    }
}
