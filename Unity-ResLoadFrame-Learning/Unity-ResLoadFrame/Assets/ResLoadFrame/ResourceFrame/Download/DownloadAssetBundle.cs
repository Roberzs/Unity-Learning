using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAssetBundle : DownloadItem
{
    private UnityWebRequest m_WebRequest;

    public DownloadAssetBundle(string url, string path) : base(url, path)
    {
    }

    public override IEnumerator Download(Action cb = null)
    {
        m_WebRequest = UnityWebRequest.Get(m_Url);
        m_StartDownload = true;
        m_WebRequest.timeout = 30;
        yield return m_WebRequest.SendWebRequest();

        if (m_WebRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Download Error:" + m_WebRequest.error);
        }
        else
        {
            byte[] bytes = m_WebRequest.downloadHandler.data;
            FileTool.CreateFile(m_SaveFilePath, bytes);
            if (cb != null)
            {
                cb();
            }
        }
    }

    public override void Destroy()
    {
        if (m_WebRequest != null)
        {
            m_WebRequest.Dispose();
            m_WebRequest = null;
        }
    }

    public override long GetCurLength()
    {
        if (m_WebRequest != null)
        {
            return (long)m_WebRequest.downloadedBytes;
        }
        return 0;
    }

    public override long GetLength()
    {
        return 0;
    }

    public override float GetProcess()
    {
        if (m_WebRequest != null)
        {
            return (long)m_WebRequest.downloadProgress;
        }
        return 0;
    }
}
