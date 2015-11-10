using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// A file, watched by the <see cref="FileObserver"/> class.
    /// </summary>
    class WatchedFile : WatchedResource
    {
        DateTime _lastRead = DateTime.MinValue;
        event FileSystemEventHandler _watcherEvent;

        public WatchedFile(string filePath, FileSystemEventHandler eventHandler)
            : base(filePath)
        {
            string fileName = Path.GetFileName(ResourcePath);
            string directoryPath = Path.GetDirectoryName(ResourcePath);

            _watcher = new FileSystemWatcher(directoryPath, fileName);
            _watcher.Changed += OnWatcherEvent;
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.EnableRaisingEvents = true;

            _watcherEvent += eventHandler;
        }

        private void OnWatcherEvent(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(ResourcePath);

            if (lastWriteTime != _lastRead)
            {
                _lastRead = lastWriteTime;

                // wait for a few milliseconds for handles to close:
                Thread.Sleep(100);

                _watcherEvent(this, e);
            }
        }

        public void AddHandler(FileSystemEventHandler eventHandler)
        {
            _watcherEvent += eventHandler;
        }

        public void RemoveHandler(FileSystemEventHandler eventHandler)
        {
            _watcherEvent -= eventHandler;
        }
    }
}
