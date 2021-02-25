using System.Collections.Generic;
using DoodleJump.Common;
using DoodleJump.Gameplay.Chunks;
using DoodleJump.UI;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class GameplayController : CommonBehaviour
    {
        private CharacterController _character;
        private PlanetGenerator _planetGenerator;
        private HUD _hud;
        private List<IChunk> _chunks;
        private IChunkSystem _chunkSystem;
        private IntroAnimationController _introAnimationController;
        private OutroAnimationController _outroAnimationController;
        
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private MovingPlatform movingPlatformPrefab;
        [SerializeField] private Rocket rocketPrefab;
        [SerializeField] private Spring springPrefab;
        [SerializeField] private Planet planetPrefab;

        private IWorld _world;

        private IEntityFactory _entityFactory;
        private IEntityFactory entityFactory
        {
            get
            {
                if(_entityFactory == null)
                {
                    _entityFactory = _entityFactory ?? new EntityFactory(Common.Logger.Instance);
                    
                    _entityFactory.AddPrefab<Platform>(platformPrefab);
                    _entityFactory.AddPrefab<MovingPlatform>(movingPlatformPrefab);
                    _entityFactory.AddPrefab<Rocket>(rocketPrefab);
                    _entityFactory.AddPrefab<Spring>(springPrefab);
                    _entityFactory.AddPrefab<Planet>(planetPrefab);
                }

                return _entityFactory;
            }
        }

        private OutroMenu _outroMenu;

        public void Initialize(IChunkSystem chunkSystem, HUD hud, CharacterController character, PlanetGenerator planetGenerator, IntroAnimationController introAnimationController, OutroAnimationController outroAnimationController, OutroMenu outroMenu)
        {
            _character = character;
            _planetGenerator = planetGenerator;
            _introAnimationController = introAnimationController;
            _outroAnimationController = outroAnimationController;
            _outroMenu = outroMenu;
            _hud = hud;

            _chunks = new List<IChunk>();

            ResetGame();
        }


        private UniversalCamera universalCamera => UniversalCamera.Instance;

        void Update()
        {
            //@TODO: these two ifs are absolute hacks, refactor them!
            if(_lost)
            {
                return;
            }

            if(!_character)
            {
                return;
            }

            var characterPosiiton = _character.Position;

            var cameraBox = universalCamera.CameraBox;
            float topY = cameraBox.BottomY;

            foreach(var chunk in _chunks)
            {
                chunk.Update();

                if(chunk.IsActive)
                {
                    topY = Mathf.Max(chunk.BoundingBox.TopY, topY);
                }
            }
            _chunks.RemoveAll(chunk => !chunk.IsActive);

            //create new chunk on top
            if(cameraBox.TopY > topY - 1)
            {
                var chunk = CreateChunk(topY, 10);
                _chunks.Add(chunk);
            }
        }

        IChunk CreateChunk(float bottomY, float length)
        {
            var configuration = new SimplePlatformChunk.Configuration(
                _world,
                Vector2.up * bottomY,
                10,
                Mathf.Lerp(1f, 1.5f, bottomY / 100f),
                Mathf.Lerp(1, 3, bottomY / 100f)
            );

            var chunk = new SimplePlatformChunk(configuration);
            chunk.Initialize();
            return chunk;
        }
        
        private void ClearChunks()
        {
            foreach(var chunk in _chunks)
            {
                if(chunk != null)
                {
                    chunk.Dispose();
                }
            }

            _chunks.Clear();
        }

        private void ResetGame()
        {
            ClearGame();
            _outroAnimationController.Reset();
            _outroMenu.Hide();
            _lost = true;
            _introAnimationController.StartIntroAnimation(() => StartGame());
        }

        private void ClearGame()
        {
            if(_world != null)
            {
                _world.Reset();
                _world = null;
            }
            ClearChunks();

            _introAnimationController.Reset();
        }

        private void StartGame()
        {
            _lost = false;
            _world = new World(_character, entityFactory, OnLose);
            _world.OnStart();

            _hud.Initialize(_world);
            _planetGenerator.Initialize(_world);
            universalCamera.Initialize(_world);

            _outroAnimationController.Reset();
            _outroMenu.Hide();
        }

        //@TODO : refactor this hack by keeping world state and update entities on demand
        private bool _lost;
        public void OnLose(int score)
        {
            if(!_lost)
            {
                _lost = true;
                _outroAnimationController.StartOutroAnimation(
                    _character, 
                    () => _outroMenu.Show(score, ResetGame),
                    () => {
                        ClearGame();
                    }
                );
            }
        }
    }

    public interface IChunkSystem
    {
        IChunk CreateChunk(float bottomY, float length);
    }
}
