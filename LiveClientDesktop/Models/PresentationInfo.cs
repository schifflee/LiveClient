using LiveClientDesktop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Models
{
    public class PresentationInfo
    {
        public string Name { get; set; }

        public string FileFullPath { get; set; }

        public DemoFileType DemoFileType { get; set; }

        public DateTime CreateTime { get; set; }

        public DemoType DemoType { get; set; }
    }
}
