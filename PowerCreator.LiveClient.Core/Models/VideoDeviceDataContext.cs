namespace PowerCreator.LiveClient.Core.Models
{
    public struct VideoDeviceDataContext
    {
        public int Data { get; }
        public int DataLength { get; }
        public VideoDeviceDataContext(int data, int dataLength)
        {
            Data = data;
            DataLength = dataLength;
        }
    }
}
