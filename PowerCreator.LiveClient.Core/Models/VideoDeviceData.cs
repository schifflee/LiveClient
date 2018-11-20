namespace PowerCreator.LiveClient.Core.Models
{
    public struct VideoDeviceData
    {
        public int Data { get; }
        public int DataLength { get; }
        public VideoDeviceData(int data, int dataLength)
        {
            Data = data;
            DataLength = dataLength;
        }
    }
}
