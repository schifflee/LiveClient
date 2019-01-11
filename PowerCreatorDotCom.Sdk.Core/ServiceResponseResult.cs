namespace PowerCreatorDotCom.Sdk.Core
{
    public class ServiceResponseResult<T> : ServiceResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Value { get; set; }
    }
}
