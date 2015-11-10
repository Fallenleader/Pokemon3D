using System;
using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    public class WatchedDirectoryEventArgs : EventArgs
    {
        /// <summary>
        /// The type of change that was done on the object.
        /// </summary>
        public WatcherChangeTypes ChangeType { get; private set; }

        /// <summary>
        /// If the change was done to a file rather than a directory.
        /// </summary>
        public bool IsFileChange { get; private set; }

        /// <summary>
        /// The path to the file or directory.
        /// </summary>
        public string ResourcePath { get; private set; }

        public WatchedDirectoryEventArgs(WatcherChangeTypes changeType, bool isFileChange, string path)
        {
            ChangeType = changeType;
            IsFileChange = isFileChange;
            ResourcePath = path;
        }
    }
}
