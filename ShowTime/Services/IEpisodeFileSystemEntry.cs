using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Services
{
    public interface IEpisodeFileSystemEntry
    {
        string FullFilename { get; }
        string ShortFilename { get; }
        string NameWithoutExtension { get; }
        string ParentDirectoryName { get; }
        string GrandParentDirectoryName { get; }
    }

    public class SystemIOEpisodeFileSystemEntry : IEpisodeFileSystemEntry
    {
        private readonly System.IO.FileInfo file;

        public string FullFilename
        {
            get { return file.FullName; }
        }

        public string ShortFilename
        {
            get { return file.Name; }
        }

        public string NameWithoutExtension
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(FullFilename); }
        }

        public string ParentDirectoryName
        {
            get { return file.Directory.Name; }
        }

        public string GrandParentDirectoryName
        {
            get { return file.Directory.Parent.Name; }
        }

        public SystemIOEpisodeFileSystemEntry(string fullFilename)
        {
            file = new System.IO.FileInfo(fullFilename);
        }
    }
}
