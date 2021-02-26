namespace DoodleJump.Gameplay
{
    [System.Serializable]
    public class ChunkSystemConfiguration
    {
        public float oneTimePlatformMinY  = 150f;
        public float oneTimePlatformChance  = 0.8f;
        public float springChance  = 0.4f;
        public float rocketChance  = 0.2f;
        public float movingPlatformChance  = 0.9f;
        public float destroyablePlatformChance  = 0.2f;
        public float destroyablePlatformMarginFactor  = 0.75f;
        public float verticalMovingChunkChance  = 0.5f;
        public int verticalMovingChunkInterval  = 120;
        public float rocketSpawnInterval  = 100f;
        public float springSpawnInterval  = 20f;
        public float reactiveChunkChance  = 0.3f;
        public int reactiveChunkInterval  = 90;
    }
}
