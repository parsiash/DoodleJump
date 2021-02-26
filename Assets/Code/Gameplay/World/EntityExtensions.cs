namespace DoodleJump.Gameplay
{
    public static class EntityExtensions
    {
        public static void SetY(this IEntity entity, float y)
        {
            var position = entity.Position;
            position.y = y;

            entity.Position = position;
        }

        public static void SetX(this IEntity entity, float x)
        {
            var position = entity.Position;
            position.x = x;

            entity.Position = position;
        }
    }
}
