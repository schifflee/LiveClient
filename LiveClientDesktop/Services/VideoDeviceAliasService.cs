using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Infrastructure;
using PowerCreator.LiveClient.Log;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public class VideoDeviceAliasService
    {
        private readonly ILoggerFacade _logger;
        private readonly string _savePath;
        private const string _saveFileName = "VideoDeviceAlias";
        public VideoDeviceAliasService(SystemConfig config, ILoggerFacade logger)
        {
            _logger = logger;
            _savePath = config.AllDataSavePath;
        }

        public bool Save(List<VideoDeviceAlias> videoDeviceAlias)
        {
            if (!videoDeviceAlias.Any()) return true;
            ICollection<string> contents = new List<string>();
            foreach (var item in videoDeviceAlias)
            {
                contents.Add(JsonHelper.SerializeObject(item));
            }
            try
            {
                FileHelper.DeleteFile(_savePath, _saveFileName);
                FileHelper.WriteAllLines(_savePath, _saveFileName, contents);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
            return true;
        }
        public List<VideoDeviceAlias> GetDeviceAliasList()
        {
            try
            {
                return JsonHelper.DeserializeObject<List<VideoDeviceAlias>>(FileHelper.ReadFileContent(_savePath, _saveFileName));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return new List<VideoDeviceAlias>();
        }
    }
}
