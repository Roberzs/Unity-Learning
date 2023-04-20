using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class DownloadItem
{
    /// <summary>
    /// 网络资源URL
    /// </summary>
    protected string m_Url;
    public string Url
    {
        get => m_Url;
    }
    /// <summary>
    /// 本地存储根目录
    /// </summary>

    protected string m_SavePath;
    public string SavePath
    {
        get => m_SavePath;
    }
    /// <summary>
    /// 不包含扩展名的文件名
    /// </summary>

    protected string m_FileNameWithoutExt;
    public string FileNameWithoutExt
    {
        get => m_FileNameWithoutExt;
    }
    /// <summary>
    /// 文件扩展名
    /// </summary>

    protected string m_FileExt;
    public string FileExt
    {
        get => m_FileExt;
    }
    /// <summary>
    /// 带扩展名的文件名
    /// </summary>

    protected string m_FileName;

    public string FileName
    {
        get => m_FileName;
    }

    protected string m_SaveFilePath;
    public string SaveFilePath
    {
        get => m_SaveFilePath;
    }
    /// <summary>
    /// 文件总大小
    /// </summary>
    protected long m_FileLength;
    public long FileLength
    {
        get => m_FileLength;
    }
    /// <summary>
    /// 当前已下载大小
    /// </summary>
    protected long m_CurLength;
    public long CurLength
    {
        get => m_CurLength;
    }
    /// <summary>
    /// 是否已经开始下载
    /// </summary>
    protected bool m_StartDownload;
    public bool StartDownload
    {
        get => m_StartDownload;
    }

    public DownloadItem(string url, string path)
    {
        m_Url = url;
        m_SavePath = path;
        m_StartDownload = false;
        m_FileNameWithoutExt = Path.GetFileNameWithoutExtension(m_Url);
        m_FileExt = Path.GetExtension(m_Url);
        m_FileName = string.Format("{0}{1}", m_FileNameWithoutExt, m_FileExt);
        m_SaveFilePath = string.Format("{0}{1}", m_SavePath, m_FileName);
    }

    public virtual IEnumerator Download(Action cb = null)
    {
        yield return null;
    }

    /// <summary>
    /// 获取当前下载进度
    /// </summary>
    /// <returns></returns>
    public abstract float GetProcess();
    /// <summary>
    /// 获取当前下载大小
    /// </summary>
    /// <returns></returns>
    public abstract long GetCurLength();
    /// <summary>
    /// 获取文件总大小
    /// </summary>
    /// <returns></returns>
    public abstract long GetLength();

    public abstract void Destroy();
}
