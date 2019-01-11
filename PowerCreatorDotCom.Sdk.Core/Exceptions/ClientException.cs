using System;

namespace PowerCreatorDotCom.Sdk.Core.Exceptions
{
    public class ClientException : Exception
    {
        public string ErrorMessage { get; set; }

        public ClientException(string message)
            : base(message)
        {
            ErrorMessage = message;
        }
    }
}
