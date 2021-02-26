namespace DoodleJump.Gameplay
{
    [System.Serializable]
    public class GameplayPrefabs
    {
        public CharacterController characterPrefab;
        public Platform platformPrefab;
        public MovingPlatform movingPlatformPrefab;
        public OneTimePlatform oneTimePlatformPrefab;
        public DestroyablePlatform destroyablePlatformPrefab;
        public Rocket rocketPrefab;
        public Spring springPrefab;
        public Planet planetPrefab;
        public FallingRocket fallingRocketPrefab;
        public PrefabChunk[] prefabChunks;
    }
}
