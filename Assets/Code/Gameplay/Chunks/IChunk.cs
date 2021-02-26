using System.Collections.Generic;
using DoodleJump.Common;

namespace DoodleJump.Gameplay.Chunks
{
    /// <summary>
    /// The abstraction for chunk. 
    /// Each chunk is a portion of environment, generated dynamically at runtime.
    /// </summary>
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
