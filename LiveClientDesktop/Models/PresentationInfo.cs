using LiveClientDesktop.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Models
{
    public class PresentationInfo
    {
        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.FileFullPath);
            }
        }

        public string FileFullPath { get; set; }

        public DemoFileType DemoFileType
        {
            get
            {
                return GetDemoFileType();
            }
        }

        public DateTime CreateTime { get; set; }

        public DemoType DemoType { get; set; }

        private DemoFileType GetDemoFileType()
        {
            switch (Path.GetExtension(FileFullPath).ToLower())
            {
                case ".mp4":
                case ".avi":
                case ".mkv":
                case ".wav":
                case ".flv":
                case ".asf": return DemoFileType.Video;
                case ".jpg":
                case ".png":
                case ".bmp": return DemoFileType.Image;
                case ".ppt":
                case ".pptx": return DemoFileType.PPT;
                case ".m4a":
                case ".mp3": return DemoFileType.Audio;
                default: return DemoFileType.None;
            }
        }

        public override string ToString()
        {
            return $"{Name},{DemoType},{DemoFileType}";
        }
    }
}
