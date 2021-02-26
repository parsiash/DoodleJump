using DoodleJump.Common;
using DoodleJump.UI;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    /// <summary>
    /// Responsible for managing the start and end of each game.
    /// </summary>
    public class GameplayController : CommonBehaviour
    {
        private IWorld _world;

        private PlanetGenerator _planetGenerator;
        private HUD _hud;
        private IntroAnimationController _introAnimationController;
        private OutroAnimationController _outroAnimationController;
        private OutroMenu _outroMenu;

        [SerializeField] private GameplayPrefabs prefabs;
        [SerializeField] private ChunkSystemConfiguration chunkSystemConfiguration;

        private IEntityFactory _entityFactory;
        private UniversalCamera universalCamera => UniversalCamera.Instance;

        public void Initialize(HUD hud, PlanetGenerator planetGenerator, IntroAnimationController introAnimationController, OutroAnimationController outroAnimationController, OutroMenu outroMenu)
        {
            _planetGenerator = planetGenerator;
            _introAnimationController = introAnimationController;
            _outroAnimationController = outroAnimationController;
            _outroMenu = outroMenu;
            _hud = hud;

            InitializeEntityFactory();
            ResetGame();
        }

         private void InitializeEntityFactory()
        {
            _entityFactory = new EntityFactory(Common.Logger.Instance);
            
            _entityFactory.AddPrefab<CharacterController>(prefabs.characterPrefab);
            _entityFactory.AddPrefab<Platform>(prefabs.platformPrefab);
            _entityFactory.AddPrefab<MovingPlatform>(prefabs.movingPlatformPrefab);
            _entityFactory.AddPrefab<OneTimePlatform>(prefabs.oneTimePlatformPrefab);
            _entityFactory.AddPrefab<DestroyablePlatform>(prefabs.destroyablePlatformPrefab);

            _entityFactory.AddPrefab<Spring>(prefabs.springPrefab);
            _entityFactory.AddPrefab<Rocket>(prefabs.rocketPrefab);
            _entityFactory.AddPrefab<FallingRocket>(prefabs.fallingRocketPrefab);

            _entityFactory.AddPrefab<Planet>(prefabs.planetPrefab);

            foreach(var prefabChunk in prefabs.prefabChunks)
            {
                if(prefabChunk)
                {
                    _entityFactory.AddPrefab(prefabChunk.ChunkName, prefabChunk);
                }
            }
        }

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
            _world = new World(_entityFactory, chunkSystemConfiguration, OnLose);
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
