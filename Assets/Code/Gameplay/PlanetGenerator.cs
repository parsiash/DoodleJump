using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class PlanetGenerator : CommonBehaviour
    {
        private UniversalCamera universalCamera => UniversalCamera.Instance;

        private List<Planet> _planets;
        private List<Planet> planets
        {
            get
            {
                _planets = _planets ?? new List<Planet>();
                return _planets;
            }
        }
        private float _lastCheckedY = 0f;

        private IWorld _world;

        public void Initialize(IWorld world)
        {
            Clear();

            _world = world;

            GeneratePlanets(0, 10f, Random.Range(1, 3));
            _lastCheckedY = 10f;
        }

        public void Clear()
        {
            foreach(var planet in planets)
            {
                if(planet)
                {
                    planet.Destroy();
                }
            }

            planets.Clear();
        }

        [SerializeField] private Color[] planetColors;

        void Update()
        {
            if(_world == null)
            {
                return;
            }

            var cameraBox = universalCamera.CameraBox;

            foreach(var planet in planets)
            {
                if(planet)
                {
                    if(planet.box.TopY < cameraBox.BottomY - 20f)
                    {
                        planet.Destroy();
                    }
                }
            }
            planets.RemoveAll(p => !p);



            var currentCheckY = cameraBox.TopY + 10;

            if(currentCheckY - _lastCheckedY > 5)
            {
                int planetCount = Random.Range(0, 2);
                GeneratePlanets(_lastCheckedY, currentCheckY, planetCount);
                _lastCheckedY = currentCheckY;
            }
        }

        private void GeneratePlanets(float startY, float endY, int planetCount)
        {
            for(int i = 0; i < planetCount; i++)
            {
                var planet = _world.EntityFactory.CreateEntity<Planet>();
                
                float planetScale = Random.Range(0.2f, 1f);
                var planetPosition = new Vector2(Random.Range(-3, 3), Random.Range(startY, endY));

                Color baseColor = planetColors[Random.Range(0, planetColors.Length)];
                baseColor = Color.HSVToRGB(Random.value, Random.Range(0f, 0.3f), Random.Range(0.1f, 0.2f));
                var secondColor = baseColor * 0.5f;
                secondColor.a = 1f;

                planet.Init(planetPosition, planetScale, Random.Range(20, 90), baseColor, secondColor);
                planets.Add(planet);
            }
        }
    }
}