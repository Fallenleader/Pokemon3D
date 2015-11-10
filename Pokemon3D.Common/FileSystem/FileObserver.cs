using System;
using System.Collections.Generic;
using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// Observes changes to files or directories and reports these changes back to the caller.
    /// </summary>
    public class FileObserver : Singleton<FileObserver>
    {
        private FileObserver() { }

        Dictionary<string, WatchedResource> _watchedResources = new Dictionary<string, WatchedResource>();

        /// <summary>
        /// Starts to watch a file.
        /// </summary>
        /// <param name="filePath">The absolute path to the file.</param>
        /// <param name="eventHandler">The handler method that gets called when the file undergoes changes.</param>
        public void StartFileObserve(string filePath, FileSystemEventHandler eventHandler)
        {
            filePath = filePath.ToLowerInvariant();

            if (_watchedResources.ContainsKey(filePath))
            {
                ((WatchedFile)_watchedResources[filePath]).AddHandler(eventHandler);
            }
            else
            {
                _watchedResources.Add(filePath, new WatchedFile(filePath, eventHandler));
            }
        }

        /// <summary>
        /// Stops to watch a file.
        /// </summary>
        public void StopFileObserve(string filePath, FileSystemEventHandler eventHandler)
        {
            filePath = filePath.ToLowerInvariant();

            if (_watchedResources.ContainsKey(filePath))
                ((WatchedFile)_watchedResources[filePath]).RemoveHandler(eventHandler);
        }
    }
}
