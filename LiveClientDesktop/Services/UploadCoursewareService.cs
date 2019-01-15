using LiveClientDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class UploadCoursewareService
    {
        private readonly ICollection<UploadTaskInfo> _taskList;
        public UploadCoursewareService()
        {
            _taskList = new List<UploadTaskInfo>();
        }

        
        public bool AddUploadTask(UploadTaskInfo task)
        {
            _taskList.Add(task);
            return true;
        }
    }
}
