using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// A directory watched by the <see cref="FileObserver"/> class.
    /// </summary>
    class WatchedDirectory : WatchedResource
    {
        event WatchedDirectoryChangeEventHandler _watcherEvent;
        private int _handleCount = 0;

        public bool HasHandles
        {
            get { return _handleCount > 0; }
        }

        public WatchedDirectory(string directoryPath, string fileExtension, WatchedDirectoryChangeEventHandler eventHandler)
            : base(directoryPath)
        {
            _watcher = new FileSystemWatcher(directoryPath, fileExtension);
            _watcher.Changed += OnWatcherEvent;
            _watcher.Created += OnWatcherEvent;
            _watcher.Deleted += OnWatcherEvent;
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            _watcher.EnableRaisingEvents = true;

            AddHandler(eventHandler);
        }

        private void OnWatcherEvent(object sender, FileSystemEventArgs e)
        {
            bool isFile = !File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory);

            WatchedDirectoryEventArgs args = new WatchedDirectoryEventArgs(e.ChangeType, isFile, e.FullPath);
            _watcherEvent(this, args);
        }

        public void AddHandler(WatchedDirectoryChangeEventHandler eventHandler)
        {
            _watcherEvent += eventHandler;
            _handleCount++;
        }

        public void RemoveHandler(WatchedDirectoryChangeEventHandler eventHandler)
        {
            _watcherEvent -= eventHandler;
            _handleCount--;
        }
    }
}
