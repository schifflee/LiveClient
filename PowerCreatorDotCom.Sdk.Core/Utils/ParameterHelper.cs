using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreatorDotCom.Sdk.Core.Utils
{
    public class ParameterHelper
    {
        private const string ISO8601_DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        public static string FormatTypeToString(FormatType? formatType)
        {
            if (FormatType.XML == formatType)
            {
                return "application/xml";
            }
            if (FormatType.JSON == formatType)
            {
                return "application/json";
            }
            if (FormatType.FORM == formatType)
            {
                return "application/x-www-form-urlencoded";
            }
            return "application/octet-stream";
        }

        public static MethodType? StringToMethodType(string method)
        {
            method = (method.ToUpper());
            switch (method)
            {
                case "GET":
                    {
                        return MethodType.GET;
                    }
                case "PUT":
                    {
                        return MethodType.PUT;
                    }
                case "POST":
                    {
                        return MethodType.POST;
                    }
                case "DELETE":
                    {
                        return MethodType.DELETE;
                    }
                case "HEAD":
                    {
                        return MethodType.HEAD;
                    }
                case "OPTIONS":
                    {
                        return MethodType.OPTIONS;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
