using UnityEngine;

namespace DoodleJump.Common
{
    public struct Box
    {
        public Vector2 _min;
        public Vector2 _max;

        public Vector2 Min => _min;
        public Vector2 Max => _max;

        public float BottomY => Min.y;
        public float TopY => Max.y;
        public float LeftX => Min.x;
        public float RightX => Max.x;

        public Vector2 Size => _max - _min;
        public Vector2 Position => (_min + _max) / 2f;
        public float Width => Size.x;
        public float Height => Size.y;

        public Box(Vector2 min, Vector2 max)
        {
            _min = min;
            _max = max;
        }

        public static Box CreateByPosition(Vector2 position, Vector2 size)
        {
            return new Box(
                position - size / 2f,
                position + size / 2f
            );
        }
    }
}
