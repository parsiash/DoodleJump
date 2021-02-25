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

        public void Initialize(IChunkSystem chunkSystem, HUD hud, CharacterController character, PlanetGenerator planetGenerator, OutroMenu outroMenu)
        {
            _character = character;
            _planetGenerator = planetGenerator;
            _outroMenu = outroMenu;
            _hud = hud;

            _chunks = new List<IChunk>();

            ResetGame();
        }


        void Update()
        {
            if(!_character)
            {
                return;
            }

            var characterPosiiton = _character.Position;
            var box = Box.CreateByPosition(characterPosiiton, new Vector2(10, 10));

            float topY = box.BottomY;

            //remove chunks that are past
            foreach(var chunk in _chunks)
            {
                var chunkBoundingBox = chunk.BoundingBox;
                if(chunkBoundingBox.TopY < box.BottomY)
                {
                    chunk.Dispose();
                }else
                {
                    topY = Mathf.Max(chunkBoundingBox.TopY, topY);
                }
            }
            _chunks.RemoveAll(chunk => !chunk.IsActive);

            //create new chunk on top
            if(box.TopY > topY - 1)
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
                Mathf.Lerp(0.2f, 1, bottomY / 100f),
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
            if(_world != null)
            {
                _world.Reset(); 
            }
            ClearChunks();

            _world = new World(_character, entityFactory, OnLose);
            _world.OnStart();

            //init hud
            _hud.Initialize(_world);

            _planetGenerator.Initialize(_world);

            _outroMenu.Hide();
        }

        public void OnLose(int score)
        {
            _outroMenu.Show(score, ResetGame);
            ClearChunks();
        }
    }

    public interface IChunkSystem
    {
        IChunk CreateChunk(float bottomY, float length);
    }
}
