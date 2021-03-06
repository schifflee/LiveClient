﻿using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public class PresentationsRepository
    {
        private const string _saveFileName = "Presentations";
        private readonly string _saveFilePath = string.Empty;
        private readonly ILoggerFacade _logger;
        private readonly SystemConfig _config;
        private readonly int _maximumSavedEntry;

        public PresentationsRepository(ILoggerFacade logger, SystemConfig config, int maximumSavedEntry)
        {
            _config = config ?? throw new ArgumentNullException("config");
            _saveFilePath = _config.AllDataSavePath;
            _logger = logger;
            _maximumSavedEntry = maximumSavedEntry;
        }
        public bool AddPersentation(PresentationInfo presentationInfo)
        {
            var presentations = GetAllPresentations();
            if (presentations != null && presentations.Any())
            {
                if (presentations.Where(p => p.Name == presentationInfo.Name && p.DemoType == presentationInfo.DemoType).Any())
                {
                    return true;
                }
            }
            if (presentations.Where(item => item.DemoType == presentationInfo.DemoType).Count() >= _maximumSavedEntry)
            {
                presentations.Remove(presentations.Where(item => item.DemoType == presentationInfo.DemoType).OrderBy(item => item.CreateTime).First());
            }
            presentationInfo.ID = Guid.NewGuid().ToString();
            presentations.Add(presentationInfo);
            return Save(presentations);

        }
        private bool Save(List<PresentationInfo> presentations)
        {
            if (!presentations.Any()) return true;
            ICollection<string> contents = new List<string>();
            foreach (var item in presentations)
            {
                contents.Add(JsonHelper.SerializeObject(item));
            }
            try
            {
                FileHelper.DeleteFile(_saveFilePath, _saveFileName);
                FileHelper.WriteAllLines(_saveFilePath, _saveFileName, contents);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Category.Exception, Priority.High);
                return false;
            }
            return true;

        }
        public List<PresentationInfo> GetAllPresentations(Func<PresentationInfo, bool> where = null)
        {
            try
            {
                var presentations = JsonHelper.DeserializeObject<List<PresentationInfo>>(FileHelper.ReadFileContent(_saveFilePath, _saveFileName));
                if (presentations != null)
                {
                    if (where == null) return presentations;

                    return presentations.Where(where).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, Category.Exception, Priority.High);
            }
            return new List<PresentationInfo>();
        }
    }
}
