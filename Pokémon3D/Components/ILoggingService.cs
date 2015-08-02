namespace Pokémon3D.Components
{
    /// <summary>
    /// An interface for a logging service.
    /// </summary>
    interface ILoggingService
    {
        /// <summary>
        /// Logs a message with a message type.
        /// </summary>
        void Log(Debug.MessageType messageType, string message);
    }
}