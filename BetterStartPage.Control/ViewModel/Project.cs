﻿using System;
using System.IO;
using System.Runtime.Serialization;

namespace BetterStartPage.Control.ViewModel
{
    [DataContract]
    internal class Project : ViewModelBase
    {
        private Uri _fileInfo;
        private string _customName;

        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CustomName))
                {
                    return CustomName;
                }
                if (_fileInfo.IsFile)
                {
                    return Path.GetFileName(FullName);
                }
                return _fileInfo.Host;
            }
        }

        [DataMember]
        public string CustomName
        {
            get { return _customName; }
            set
            {
                if (value == _customName) return;
                _customName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DirectoryName));
            }
        }

        public string DirectoryName
        {
            get
            {
                // show full name if a custom name is set
                if (!string.IsNullOrWhiteSpace(CustomName))
                {
                    return FullName;
                }
                if (_fileInfo.IsFile)
                {
                    return Path.GetDirectoryName(FullName);
                }
                return _fileInfo.ToString();
            }
        }

        [DataMember]
        public string FullName
        {
            get
            {
                if (_fileInfo.IsFile)
                {
                    return _fileInfo.LocalPath;
                }
                return _fileInfo.ToString();
            }
            set
            {
                _fileInfo = new Uri(value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DirectoryName));
            }
        }

        public bool IsNormalFile
        {
            get
            {
                var extension = Path.GetExtension(FullName);
                if (extension == null) return true;
                if (extension.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase)) return false;
                if (extension.EndsWith("proj", StringComparison.InvariantCultureIgnoreCase)) return false;
                return true;
            }
        }

        public Project()
        {
        }

        public Project(string fullName)
        {
            FullName = fullName;
        }
    }
}
