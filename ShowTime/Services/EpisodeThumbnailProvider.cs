using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using System.Diagnostics;

namespace ShowTime.Services
{
    public interface IEpisodeThumbnailFilenameProvider
    {
        IEpisodeThumbnailFilename GetThumbnailFilenameForEpisode(Episode episode);
    }

    public interface IEpisodeThumbnailGenerator
    {
        IEpisodeThumbnailFilenameProvider FilenameProvider { get; }
        void GenerateThumbnailForEpisode(Episode episode);
    }

    public interface IEpisodeThumbnailFilename
    {
        string DesiredFilename { get; }
        string NotExistFilename { get; }
        bool DesiredFilenameExists { get; }
        string ActualFilename { get;}
    }

    public class MockEpisodeThumbnailFilenameProvider : IEpisodeThumbnailFilenameProvider
    {
        public class MockEpisodeThumbnailFilename : IEpisodeThumbnailFilename
        {
            public string DesiredFilename { get; private set; }
            public string NotExistFilename { get; private set; }
            public bool DesiredFilenameExists
            {
                get { return System.IO.File.Exists(DesiredFilename); }
            }
            public string ActualFilename { get { return DesiredFilenameExists ? DesiredFilename : NotExistFilename; } }

            public MockEpisodeThumbnailFilename(string desiredFilename, string notExistsFilename)
            {
                DesiredFilename = desiredFilename;
                NotExistFilename = notExistsFilename;
            }
        }

        public IEpisodeThumbnailFilename GetThumbnailFilenameForEpisode(Episode episode)
        {
            string thumbnailOutputFilename = @"C:\ShowTimeData\Thumbnails\" + System.IO.Path.GetFileNameWithoutExtension(episode.Filename) + ".jpg";
            return new MockEpisodeThumbnailFilename(thumbnailOutputFilename, @"C:\ShowTimeData\Thumbnails\NotFound.jpg");
        }
    }

    public class FFMpegEpisodeThumbnailGenerator : IEpisodeThumbnailGenerator
    {
        public IEpisodeThumbnailFilenameProvider FilenameProvider { get; private set; }

        public FFMpegEpisodeThumbnailGenerator(IEpisodeThumbnailFilenameProvider thumbnailFilenameProvider)
        {
            this.FilenameProvider = thumbnailFilenameProvider;
        }

        public void GenerateThumbnailForEpisode(Episode episode)
        {
            IEpisodeThumbnailFilename thumbnailFilename = FilenameProvider.GetThumbnailFilenameForEpisode(episode);
            if (!thumbnailFilename.DesiredFilenameExists)
            {
                GenerateVideoThumbnail(episode.Filename, thumbnailFilename.DesiredFilename);
            }
        }

        string ffExe = @"C:\ShowTimeData\ffmpeg.exe";
        string ffArgsTemplate = "-i \"{0}\" -an -ss 00:00:05 -t 00:00:01 -r 1 -s 320x240 \"{1}\"";
        private void GenerateVideoThumbnail(string inVideoFilename, string outImageFilename)
        {
            var ffArgs = string.Format(ffArgsTemplate, inVideoFilename, outImageFilename);
            ProcessStartInfo startInfo = new ProcessStartInfo(ffExe, ffArgs);
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
        }
    }
}
