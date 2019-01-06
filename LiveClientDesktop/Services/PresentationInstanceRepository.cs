using System.Collections.Generic;
using POWERPOINT = Microsoft.Office.Interop.PowerPoint;

namespace LiveClientDesktop.Services
{
    public class PresentationInstanceRepository
    {
        public static PresentationInstanceRepository Instance { get; } = new PresentationInstanceRepository();
        private Dictionary<string, POWERPOINT.Presentation> _dictionary { get; set; }
        private PresentationInstanceRepository()
        {
            _dictionary = new Dictionary<string, POWERPOINT.Presentation>();
        }
        public POWERPOINT.Presentation this[string name]
        {
            get
            {
                POWERPOINT.Presentation value = null;
                _dictionary.TryGetValue(name, out value);
                return value;
            }
            set { _dictionary[name] = value; }
        }
        public Dictionary<string, POWERPOINT.Presentation> GetAll()
        {
            return _dictionary;
        }
        public void Clear()
        {
            _dictionary.Clear();
        }
    }
}
