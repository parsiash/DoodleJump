namespace DoodleJump.Gameplay
{
    public class Spring : Entity, ICollectible
    {
        public void OnCollected(CharacterController character)
        {
            if(character.Veolicty.y < 0)
            {
                character.SpringJump();
            }
        }
    }
}
