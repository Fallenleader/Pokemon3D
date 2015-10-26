using birdScript;

namespace Test.birdScript
{
    /// <summary>
    /// A factory to create <see cref="ScriptProcessor"/> instances.
    /// </summary>
    static class ScriptProcessorFactory
    {
        /// <summary>
        /// Creates a new <see cref="ScriptProcessor"/>.
        /// </summary>
        internal static ScriptProcessor GetNew()
        {
            return new ScriptProcessor();
        }

        /// <summary>
        /// Creates a new <see cref="ScriptProcessor"/> and runs the provided source code.
        /// </summary>
        internal static ScriptProcessor GetRun(string source)
        {
            var processor = new ScriptProcessor();

            processor.Run(source);

            return processor;
        }
    }
}
