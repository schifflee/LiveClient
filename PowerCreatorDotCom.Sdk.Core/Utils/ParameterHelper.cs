﻿using PowerCreatorDotCom.Sdk.Core.Http;
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
            if (FormatType.MultipartFormData == formatType)
            {
                return "multipart/form-data; boundary=" + DateTime.Now.Ticks.ToString("x");
            }
            return "application/octet-stream";
        }

        public static FormatType? StingToFormatType(string format)
        {
            if (format.ToLower().Equals("application/xml") || format.ToLower().Equals("text/xml"))
            {
                return FormatType.XML;
            }
            if (format.ToLower().Equals("application/json"))
            {
                return FormatType.JSON;
            }
            if (format.ToLower().Equals("application/x-www-form-urlencoded"))
            {
                return FormatType.FORM;
            }
            return FormatType.RAW;
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
