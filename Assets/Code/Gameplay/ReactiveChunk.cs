using System.Collections.Generic;

namespace DoodleJump.Gameplay
{
    public class ReactiveChunk : PrefabChunk
    {
        private List<ReactivePlatform> _reactivePlatforms;
        private List<ReactivePlatform> reactivePlatforms
        {
            get
            {
                if(_reactivePlatforms == null)
                {
                    _reactivePlatforms = new List<ReactivePlatform>();
                    _reactivePlatforms.AddRange(GetComponentsInChildren<ReactivePlatform>(true));
                    _reactivePlatforms.ForEach(rp => rp.OnJump += OnJump);
                }

                return _reactivePlatforms;
            }
        }

        public override void Init(IWorld world)
        {
            base.Init(world);

            var platforms = reactivePlatforms;
        }

        public void OnJump()
        {
            foreach(var reactivePlatform in reactivePlatforms)
            {
                if(reactivePlatform)
                {
                    reactivePlatform.React();
                }
            }
        }
    }
}
