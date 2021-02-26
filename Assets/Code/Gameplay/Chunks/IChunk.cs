using System.Collections.Generic;
using DoodleJump.Common;

namespace DoodleJump.Gameplay.Chunks
{
    public interface IChunk
    {
        IWorld World { get; }
        float Length { get; }
        Box BoundingBox { get; }
        IEnumerable<IEntity> Entities { get; }

        bool IsActive { get; }

        void OnUpdate();
        void Dispose();
    }

    public interface IPlatformChunk : IChunk
    {

    }
}
