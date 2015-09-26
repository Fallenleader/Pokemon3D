using System;
using System.Reflection;

namespace Pokémon3D
{
    /// <summary>
    /// A base class for a thread-safe singleton pattern implementation.
    /// The type that uses this class must not have a public constructor.
    /// </summary>
    abstract class Singleton<T> where T : class
    {
        private static volatile T _instance;
        private static object _syncRoot = new object();
        
        /// <summary>
        /// The instance of this singleton class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    CreateInstance();

                return _instance;
            }
        }

        private static void CreateInstance()
        {
            lock (_syncRoot)
            {
                if (_instance == null)
                {
                    Type t = typeof(T);

                    //Ensure there are no public constructors...
                    ConstructorInfo[] ctors = t.GetConstructors();
                    if (ctors.Length > 0)
                    {
                        throw new InvalidOperationException(string.Format("{0} has at least one accessibly constructor, making it impossible to enforce singleton behaviour.", t.Name));
                    }

                    _instance = (T)Activator.CreateInstance(t, true);
                }
            }
        }
    }
}
