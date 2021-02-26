namespace DoodleJump.Common
{
    /// <summary>
    /// A Templated singleton for non-behaviour types.!--
    /// For unity behaviour singletons, see SingletonBehaviour<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                instance = instance ?? new T();
                return instance;
            }
        }

        protected Singleton()
        {

        }
    }
}