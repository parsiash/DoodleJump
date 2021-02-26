namespace DoodleJump.Gameplay
{
    public class OneTimePlatform : Platform
    {
        private bool _used;
        public override void Init(IWorld world)
        {
            base.Init(world);
            _used = false;
        }

        public override bool OnCharacterJump(CharacterController character)
        {
            base.OnCharacterJump(character);

            if(!_used)
            {
                _used = true;
                return true;
            }

            return false;
        }
    }
}