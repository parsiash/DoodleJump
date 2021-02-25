using System.Collections.Generic;
using DoodleJump.Common;

namespace DoodleJump.Gameplay.Chunks
{
    public interface IChunk
    {
        float Length { get; }
        Box BoundingBox { get; }
        void Initialize();
        void Dispose();
        bool IsActive { get; }
        IEnumerable<IEntity> Entities { get; }
        void Update();
    }

    public interface IPlatformChunk : IChunk
    {

    }
}
