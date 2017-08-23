using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;

namespace WSWD.WmallPos.POS.FX.Shared.FileHandler
{
    [Serializable]
    public class ETCFileSystemInfo
    {
        public virtual bool IsFile { get { return false; } }

        public virtual bool IsDirectory { get { return false; } }

        public ETCFileSystemInfo()
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

        public static implicit operator ETCFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            ETCFileSystemInfo vkFileSystemInfo;
            if (fileSystemInfo is FileInfo)
            {
                vkFileSystemInfo = new ETCFileInfo();
            }
            else if (fileSystemInfo is DirectoryInfo)
            {
                vkFileSystemInfo = new ETCDirectoryInfor();
            }
            else
            {
                vkFileSystemInfo = new ETCFileSystemInfo();
            }
            vkFileSystemInfo.FullName = fileSystemInfo.FullName;
            vkFileSystemInfo.CreationTime = fileSystemInfo.CreationTime;
            vkFileSystemInfo.LastModifiedTime = fileSystemInfo.LastWriteTime;
            return vkFileSystemInfo;
        }
    }

    [Serializable]
    public class ETCFileInfo : ETCFileSystemInfo
    {
        public override bool IsFile { get { return true; } }
    }

    [Serializable]
    public class ETCDirectoryInfor : ETCFileSystemInfo
    {
        public override bool IsDirectory { get { return true; } }
    }
}
