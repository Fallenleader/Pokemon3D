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
            {
                var watchedFile = (WatchedFile)_watchedResources[filePath];
                watchedFile.RemoveHandler(eventHandler);

                if (!watchedFile.HasHandles)
                    _watchedResources.Remove(filePath);
            }
        }

        public void StartDirectoryObserve(string directoryPath, string fileExtension, WatchedDirectoryChangeEventHandler eventHandler)
        {
            directoryPath = directoryPath.ToLowerInvariant();

            string key = directoryPath + fileExtension.ToLowerInvariant();

            if (_watchedResources.ContainsKey(key))
            {
                ((WatchedDirectory)_watchedResources[key]).AddHandler(eventHandler);
            }
            else
            {
                _watchedResources.Add(key, new WatchedDirectory(directoryPath, fileExtension, eventHandler));
            }
        }

        public void StopDirectoryObserve(string directoryPath, string fileExtension, WatchedDirectoryChangeEventHandler eventHandler)
        {
            directoryPath = directoryPath.ToLowerInvariant();

            string key = directoryPath + fileExtension.ToLowerInvariant();

            if (_watchedResources.ContainsKey(key))
            {
                var watchedDirectory = (WatchedDirectory)_watchedResources[key];
                watchedDirectory.RemoveHandler(eventHandler);

                if (!watchedDirectory.HasHandles)
                    _watchedResources.Remove(key);
            }
        }
    }
}
