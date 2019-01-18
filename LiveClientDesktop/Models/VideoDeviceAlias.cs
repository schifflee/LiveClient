namespace LiveClientDesktop.Models
{
    public class VideoDeviceAlias
    {
        public string DeviceName { get; set; }
        public string DeviceNoteName { get; set; }
        public override string ToString()
        {
            return $"{DeviceNoteName}";
        }
    }
}
