using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WSWD.WmallPos.POS.AppStarter.Net
{
    public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);

    public class DownloadProgressChangedEventArgs : EventArgs
    {
        private long _BytesReceived = 0;

        public long BytesReceived
        {
            get { return _BytesReceived; }
            set { _BytesReceived = value; }
        }

        private long _TotalBytesToReceive = 0;

        public long TotalBytesToReceive
        {
            get { return _TotalBytesToReceive; }
            set { _TotalBytesToReceive = value; }
        }
    }

    public class ConnectionFailedEventArgs : EventArgs
    {
        public bool Retry { get; set; }
    }

    public class POSFtpTranferInfo
    {
        public string ServerPath;
        public string LocalPath;
        public bool IsDirectory;
        public int SourceLength;
        public DateTime SourceLastModified;

        public POSFtpTranferInfo(string serverPath, string localPath, bool isDirectory, string file)
        {
            ServerPath = serverPath;
            LocalPath = localPath;
            IsDirectory = isDirectory;
        }
        public POSFtpTranferInfo(string serverPath, string localPath, bool isDirectory)
        {
            ServerPath = serverPath;
            LocalPath = localPath;
            IsDirectory = isDirectory;
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public string ServerFile;
        public string LocalFile;
        public int ThreadId;
        public int Percentage;
        public int TotalPercentage;

        public ProgressEventArgs(string serverFile, string localfile, int threadId, int percentage, int totalPercentage)
        {
            ServerFile = serverFile;
            LocalFile = localfile;
            ThreadId = threadId;
            Percentage = percentage;
            TotalPercentage = totalPercentage;
        }
    }

    [Serializable]
    public class POSFileSystemInfo
    {
        public virtual bool IsFile { get { return false; } }

        public virtual bool IsDirectory { get { return false; } }

        public POSFileSystemInfo()
        {

        }

        private string m_FullName = string.Empty;
        public string FullName
        {
            get { return m_FullName; }
            set { m_FullName = value; }
        }

        public string Name
        {
            get { return System.IO.Path.GetFileName(m_FullName); }
            set
            {
                int fullLength = m_FullName.Length;
                int nameLength = Name.Length;
                m_FullName = m_FullName.Remove(fullLength - nameLength, nameLength) + value;
            }
        }

        public string Path
        {
            get { return System.IO.Path.GetDirectoryName(m_FullName); }
            set
            {
                m_FullName = value + Name;
            }
        }

        public string Ext
        {
            get { return System.IO.Path.GetExtension(m_FullName); }
        }

        int m_Length = 0;
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        string m_Permission = string.Empty;
        public string Permission
        {
            get { return m_Permission; }
            set { m_Permission = value; }
        }


        DateTime m_CreationTime;
        public DateTime CreationTime
        {
            get { return m_CreationTime; }
            set { m_CreationTime = value; }
        }

        DateTime m_LastModifiedTime;
        public DateTime LastModifiedTime
        {
            get { return m_LastModifiedTime; }
            set { m_LastModifiedTime = value; }
        }

        public static implicit operator POSFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            POSFileSystemInfo vkFileSystemInfo;
            if (fileSystemInfo is FileInfo)
            {
                vkFileSystemInfo = new POSFileInfo();
            }
            else if (fileSystemInfo is DirectoryInfo)
            {
                vkFileSystemInfo = new POSDirectoryInfor();
            }
            else
            {
                vkFileSystemInfo = new POSFileSystemInfo();
            }

            vkFileSystemInfo.FullName = fileSystemInfo.FullName;
            vkFileSystemInfo.CreationTime = fileSystemInfo.CreationTime;
            vkFileSystemInfo.LastModifiedTime = fileSystemInfo.LastWriteTime;
            return vkFileSystemInfo;
        }
    }

    [Serializable]
    public class POSFileInfo : POSFileSystemInfo
    {
        public override bool IsFile { get { return true; } }
    }

    [Serializable]
    public class POSDirectoryInfor : POSFileSystemInfo
    {
        public override bool IsDirectory { get { return true; } }
    }

    public class UpdateProgressEventArgs : EventArgs
    {
        public string StatusMessage { get; set; }

        public string ProgressMessage { get; set; }

        public int Percentage { get; set; }

        public int TotalPercentage { get; set; }
    }
}
