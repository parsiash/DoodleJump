using DoodleJump.Common;
using DoodleJump.UI;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class GameplayController : CommonBehaviour
    {
        private PlanetGenerator _planetGenerator;
        private HUD _hud;
        private IChunkSystem _chunkSystem;
        private IntroAnimationController _introAnimationController;
        private OutroAnimationController _outroAnimationController;

        [SerializeField] private CharacterController characterPrefab;
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private MovingPlatform movingPlatformPrefab;
        [SerializeField] private OneTimePlatform oneTimePlatformPrefab;
        [SerializeField] private DestroyablePlatform destroyablePlatformPrefab;
        [SerializeField] private Rocket rocketPrefab;
        [SerializeField] private Spring springPrefab;
        [SerializeField] private Planet planetPrefab;
        [SerializeField] private FallingRocket fallingRocketPrefab;
        [SerializeField] private PrefabChunk[] prefabChunks;

        private IWorld _world;

        private IEntityFactory _entityFactory;
        private IEntityFactory entityFactory
        {
            get
            {
                if(_entityFactory == null)
                {
                    _entityFactory = _entityFactory ?? new EntityFactory(Common.Logger.Instance);
                    
                    _entityFactory.AddPrefab<CharacterController>(characterPrefab);
                    _entityFactory.AddPrefab<Platform>(platformPrefab);
                    _entityFactory.AddPrefab<MovingPlatform>(movingPlatformPrefab);
                    _entityFactory.AddPrefab<OneTimePlatform>(oneTimePlatformPrefab);
                    _entityFactory.AddPrefab<DestroyablePlatform>(destroyablePlatformPrefab);

                    _entityFactory.AddPrefab<Spring>(springPrefab);
                    _entityFactory.AddPrefab<Rocket>(rocketPrefab);
                    _entityFactory.AddPrefab<FallingRocket>(fallingRocketPrefab);

                    _entityFactory.AddPrefab<Planet>(planetPrefab);

                    foreach(var prefabChunk in prefabChunks)
                    {
                        if(prefabChunk)
                        {
                            _entityFactory.AddPrefab(prefabChunk.ChunkName, prefabChunk);
                        }
                    }
                }

                return _entityFactory;
            }
        }

        private OutroMenu _outroMenu;

        public void Initialize(IChunkSystem chunkSystem, HUD hud, PlanetGenerator planetGenerator, IntroAnimationController introAnimationController, OutroAnimationController outroAnimationController, OutroMenu outroMenu)
        {
            _planetGenerator = planetGenerator;
            _introAnimationController = introAnimationController;
            _outroAnimationController = outroAnimationController;
            _outroMenu = outroMenu;
            _hud = hud;

            ResetGame();
        }


        private UniversalCamera universalCamera => UniversalCamera.Instance;

        void Update()
        {
            _world?.OnUpdate(Time.deltaTime);
        }

        private void ResetGame()
        {
            ClearGame();
            _outroAnimationController.Reset();
            _outroMenu.Hide();
            _introAnimationController.StartIntroAnimation(() => StartGame());
        }

        private void ClearGame()
        {
            if(_world != null)
            {
                _world.Reset();
                _world = null;
            }

            _introAnimationController.Reset();
        }

        private void StartGame()
        {
            _world = new World(entityFactory, OnLose);
            _world.OnStart();

            _hud.Initialize(_world);
            _planetGenerator.Initialize(_world);
            universalCamera.Initialize(_world);

            _outroAnimationController.Reset();
            _outroMenu.Hide();
        }

        public void OnLose(int score)
        {
            _outroAnimationController.StartOutroAnimation(
                _world.Character, 
                () => _outroMenu.Show(score, ResetGame),
                () => {
                    ClearGame();
                }
            );
        }
    }
}
