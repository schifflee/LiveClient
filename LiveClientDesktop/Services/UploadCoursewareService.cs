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
        private readonly List<UploadTaskInfo> _taskList;
        private bool isUploading;
        public UploadCoursewareService()
        {
            _taskList = new List<UploadTaskInfo>();
        }

        public bool AddUploadTask(UploadTaskInfo task)
        {
            _taskList.Add(task);
            return true;
        }

        private void Upload()
        {
            if (isUploading) return;
            for (int i = 0; i < _taskList.Count; i++)
            {
                _taskList[i].IsUpload
            }
        }
    }
}
