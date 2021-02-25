using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class Planet : Entity
    {
        private Parallax parallax => GetCachedComponent<Parallax>();

        private Material _material;
        private Material material
        {
            get
            {
                if(!_material)
                {
                    var renderer = GetCachedComponent<Renderer>();
                    _material = Object.Instantiate(renderer.sharedMaterial);
                    renderer.material = _material;

                }

                return _material;
            }
        }

        public void Init(Vector2 position, float scale, float zIndex, Color baseColor, Color secondColor)
        {
            Position = position;
            transform.localScale = Vector3.one * scale;
            var parallaxFactor = Mathf.Lerp(0.1f, 1f, zIndex / 100);
            parallax.Init(parallaxFactor, zIndex);

            // baseColor.a = 1 - parallaxFactor;
            material.SetColor("_BaseColor", baseColor);
            material.SetColor("_SecondColor", secondColor);
        }
    }
}