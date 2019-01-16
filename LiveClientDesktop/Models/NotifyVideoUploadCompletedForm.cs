using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class NotifyVideoUploadCompletedForm
    {
        public NotifyVideoUploadCompletedForm()
        {
            Storages = new List<CreateFormTempFileResult>();
        }
        public List<CreateFormTempFileResult> Storages { get; set; }
        public int RecordID { get; set; }

        public int videoIndex { get; set; }

        public int Duration { get; set; }
    }
}
