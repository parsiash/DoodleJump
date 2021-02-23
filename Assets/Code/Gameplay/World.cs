using DoodleJump.Common;

namespace DoodleJump.Gameplay
{
    public interface IWorld
    {
        float LeftEdgeX { get; }
        float RightEdgeX { get; }
        CharacterController Character { get; }

        void OnStart();
        void Reset();

        IEntityFactory EntityFactory { get; }
        DoodleJump.Common.ILogger Logger { get; }
    }

    public class World : IWorld
    {
        public float LeftEdgeX => -2f;
        public float RightEdgeX => 2f;

        private CharacterController _character;
        public CharacterController Character => _character;

        public Common.ILogger Logger => Common.Logger.Instance;

        private IEntityFactory _entityFactory;
        public IEntityFactory EntityFactory => _entityFactory;

        public World(CharacterController character, IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
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
