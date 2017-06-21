[TOC]

### 版本号数据VersionInfo

	public class VersionInfo
	{
		public string version;
		public int packVer;
		
		public VersionInfo(string version,int packVer)
		{
			this.version = version;
			this.packVer = packVer;
		}
	}

### 热更新filelistItemInfo
	
	public class FilelistItemInfo
	{
		public string path;
		public long crc32;
		public long size;
	}

### 热更逻辑HotUpdate
	
	public class HotUpdate
	{
		public static string resourcesPath = "file:///H:/CDN/android";
		public static int MaxLoadingNum = 5;
		protected VersionInfo _serverVerInfo;
		protected VersionInfo _localVerInfo;
		protected string _strTxtServerVer = "";
		protected string _strTxtServerFileList = "";
		protected string _strTxtTempFileList = "";
		protected StringBuilder _sbTempFileList;
		protected long _needDownloadKB = 0;
		protected long _totalDownloadedKB = 0;
		protected long _tempSaveBytes = 0;
		protected int _needDownloadFileCount;
		protected int _totalDownloadedFileCount;
		protected Dictionary<string,FilelistItemInfo> _serverFileListItemInfoDic = new Dictionary<string,FilelistItemInfo>();
		protected Dictionary<string,FilelistItemInfo> _localFileListItemInfoDic = new Dictionary<string,FilelistItemInfo>();
		protected Dictionary<string,FilelistItemInfo> _needFileListItemInfoDic = new Dictionary<string,FilelistItemInfo>();
		protected Dictionary<string,FilelistItemInfo> _tempFileListItemInfoDic = new Dictionary<string,FilelistItemInfo>();
		
		public Action<VersionInfo,VersionInfo> hotUpdateFinished;

		protected static string persistentPath = Application.persistentDataPath;
		protected static string persistentVerPath = persistentPath + "/android/version.txt";
		protected static string persistentFilelistPath = persistentPath + "/android/filelist.txt";
		protected static string tempPersistentFilelistPath = persistentPath + "android/tempFilelist.txt";
	
		public void InitVer()
		{
			ResourcesManager.Instance.DownLoadAsset(resourcesPath + "version.txt",(loaderItem)=>
			{
				VersionDownLoaded(www);
			});
		}

		protected virtual void VersionDownLoaded(WWW www)
		{
			_strTxtServerVer = www.text;
			UnityEngine.Debug.Log("下载version文件：" + _strTxtServerVer);
			_serverVerInfo = HotUpdateUtil.ParseVersionInfo(_strTxtServerVer);
		}

		protected virtual void ServerFileListDownloaded(WWW www)
		{
			string localFilelistPath = "";
			_needFileListItemInfoDic.Clear();
			_needDownloadKB = 0;
			_totalDownloadedKB = 0;
			_tempSaveBytes = 0;
			_sbTempFilelist = new StringBuilder();
			_strTxtServerFileList = www.text;
			_serverFileListItemInfoDic = HotUpdateUtil.GetFileListMap(_strTxtServerFileList);
		}

		protected void AddToDownloadDic(string key,FilelistItemInfo filelistItemInfo)
		{
			_needDownloadKB += filelistItemInfo.size;
			_needFileListItemInfoDic.Add(key,filelistItemInfo);
		}

		protented void BeginDownloadAsset()
		{
			int loadingCount = 0;
			List<string> loadingList = new List<string>();
			var iter = _needFileListItemInfoDic.GetEnumerator();
			while(iter.MoveNext())
			{
				loadingList.Add(iter.Current.Key);
				var filelistItemInfo = iter.Current.Value;
				UnityEngine.Debug.Log("开始下载文件:" + resourcesPath + filelistItemInfo.path);
				HotUpdateUtil.DownLoad(resourcesPath + filelistItemInfo.path,OnFileDownloaded);
				++loadingCount;
				if(loadingCount >= MaxLoadingNum) break;
			}
			for(int i = 0; i < loadingList.Count;i++)
			{
				_needFileListItemInfoDic.Remove(loadingList[i]);
			}
		}

		protected virtual void OnFileDownloaded(WWW www)
		{
			
		}

		protected virtual void HotUpdateFinished()
		{
			UnityEngine.Debug.Log("更新完成，更新文件个数：" + _needDownloadFileCount);
		}

	}

### Android热更部分
	
	public class AndroidHotUpdate : HotUpdate
	{
		protected override void VersionDownLoaded(WWW wwww)
		{
			base.VersionDownLoaded(www);
			string localVerFilePath = persistentVerPath;
			if(File.Exists(localVerFilePath))
				_localVerInfo = ParseVersionInfo(File.ReadAllText(localVerFilePath));
			else
				_localVerInfo = ParseVersionInfo(Resources.Load<TextAsset>("version").text);

			_strTxtServerVer = www.text;
			LogUtil.LogNormal("下载文件内容：" + _strTxtServerVer);
			_serverVerInfo = ParseVersionInfo(_strTxtServerVer);
			int updateType = CompareVer(_serverVerInfo,_localVerInfo);
			if(updateType == HotUpdateType.DonotHotUpdate)//不更新
			{
				HotUpdateFinished();
			}
			else if(updateType == HotUpdateType.HotUpdate)//热更
			{
				HotUpdateUtil.DownLoad(resourcesPath + "filelist.txt",ServerFileListDownloaded);
			}
			else if(udpateType == HotUpdateType.PackUpdate)//整包更新
			{

			}
			www.Dispose();
			www = null;
		}
		
		protected override void ServerFileListDownLoaded(WWW www)
		{
			base.ServerFileListDownLoaded(www);
			string localFilelistPath = "";
			if(File.Exists(persistentFilelistPath))
			{
				localFilelistPath = persistentFilelistPath;
			}
			else
			{
				localFilelistPath = Application.streamingAssetsPath + "/android/filelist.txt";
			}
			_localFileListItemInfoDic = HotUpdateUtil.GetFileListMap(File.ReadAllText(localFilelistPath));
			
			if(File.Exists(tempPersistentFilelistPath))
			{
				_strTxtTempFileList = File.ReadAllText(tempPersistentFilelistPath);
				_tempFileListItemInfoDic = HotUpdateUtil.GetFileListMap(_strTxtTempFileList);
			}
			//已经下载到本地的放进_localFileListItemInfoDic里面（合并中断已经下载了的资源包）
			var tempFilelistDicIter = _tempFileListItemInfoDic.GetEnumerator();
			while(tempFilelistDicIter.MoveNext())
			{
				var path = tempFilelistDicIter.Current.Key;	
				var fileItemInfo = tempFilelistDicIter.Current.Value;
				if(!_localFileListItemInfoDic.ContainsKey(path))
					_localFileListItemInfoDic.Add(path,fileItemInfo);
				else if(_localFileListItemInfoDic[path].crc32 != fileItemInfo.crc32)
					_localFileListItemInfoDic[path] = fileItemInfo;
				_sbTempFileList.Append(string.Format("{0}\t{1}\t{2}\n",path,fileItemInfo.crc32,fileItemInfo.size));
			}
			var iter = _serverFileListItemInfoDic.GetEnumerator();
			while(iter.MoveNext())
			{
				var path = iter.Current.Key;
				var fileItemInfo = iter.Current.Value;
				if(_localFileListItemInfoDic.ContainsKey(path))
				{
					if(fileItemInfo.crc32 != _localFileListItemInfoDic[path].crc32)
						AddToDownloadDic(path,fileItemInfo);
				}
				else
				{
					AddToDownloadDic(path,fileItemInfo);
				}
			}
			_needDownloadFileCount = _needFileListItemInfoDic.Count;
			BeginDownloadAsset();
			www.Dispose();		
			www = null;
		}
		
		protected override void OnFileDownloaded(WWW www)
		{
			base.OnFileDownloaded(www);
			string assetRelativePath = www.url.Replace(resourcesPath,"");
			string rootPath = Application.persistentDataPath + "/android/";
			string path = rootPath + assetRelativePath;
			string dir = Path.GetDirectoryName(path);
			if(Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			UnityEngine.Debug.Log("写入文件：" + path);
			File.WriteAllBytes(path,www.bytes);
			++_totalDownloadedFileCount;
			FileListItemInfo fileItemInfo = _serverFileListItemInfoDic[assetRelativePath];
			long curFileSize = fileItemInfo.size;
			_totalDownloadedKB += curFileSize;
			_tempSaveBytes += curFileSize;
			_sbTempFilelist.Append(string.Format("{0}\t{1}\t{2}\n",fileItemInfo.path,fileItemInfo.crc32,fileItemInfo.size));	
			
			if(_tempSaveBytes >= 512)
			{
				string tempContent = _sbTempFilelist.ToString();
				File.WriteAllText(tempPersistentFileListPath,tempContent);
				_tempSaveBytes = 0;
				if(File.Exists(tempPersistentFileListPath))
					File.Delete(tempPersistentFileListPath);
			}
			
			if(_totalDownloadedFileCount == _needDownloadFileCount)
			{
				HotUpdateFinished();
			}
			else
			{
				string nexteLoading = "";
				var iter = _needFileListItemInfoDic.GetEnumerator();
				while(iter.MoveNext())
				{
					nextLoading = iter.Current.Key;
					var filelistItemInfo = iter.Current.Value;
					HotUpdateUtil.DownLoad(resourcesPath + filelistItemInfo.path,OnFileDownloaded);
					break;
				}
				if(_needFileListItemInfoDic.ContainsKey(nextLoading))
					_needFileListItemInfoDic.Remove(nextLoading);
			}
			www.Dispose();
			www = null;
		}

		protected override void HotUpdateFinished()
		{
			base.HotUpdateFinished();
			if(!string.IsNullOrEmpty(_strTxtServerVer))
				File.WriteAllText(persistentVerPath,_strTxtServerVer);
			if(!string.IsNullOrEmpty(_strTxtServerFileList))
				File.WriteAllText(persistentFilelistPath,_strTxtServerFileList);
			if(hotUpdateFinished != null)
				hotUpdateFinished(_serverVerInfo,_localVerInfo);
		}

	}


### 热更新函数

	public Enum HotUpdateType
	{
		DonotHotUpdate = 0,
		HotUpdate = 1,
		packUdate = 2,
	}
		
	public class HotUpdateUtil
	{
		public static VersionInfo ParseVersionInfo(string content)
		{
			string ver = "";
			int packVer = 0;
			VersionInfo versionInfo = null;
			string[] versionInfos = content.Split('\n');
			if(versionInfos.Length != 2)
			{
				UnityEngine.Debug.Log("解析资源版本出错，解析内容：" + content);
				return null;
			}		
			ver = versionInfos[0];
			if(!int.TryParse(versionInfos[1],out packVer))
			{
				UnityEngine.Debug.Log("解析资源版本出错，解析内容：" + versionInfos[1]);
				return null;
			}	
			versionInfo = new VersionInfo(ver,packVer);
			return versionInfo;
		}

		public static Dictionary<string,FilelistItemInfo> GetFileListMap(string strTxt)
		{
			Dictionary<string,FilelistItemInfo> ret = new Dictionary<string,FilelistItemInfo>();
			if(string.IsNullOrEmpty(strTxt)) return ret;
			string[] strItems = strTxt.Split('\n');
			for(int i = 0; i < strItems.Length;i++)
			{
				string item = strItems[i];
				string[] itemInfos = item.Split('\t');
				if(itemInfos.Length != 3)
				{
					Debug.Log("解析单个资源出错:" + item);
					continue;
				}
				string path = string.Intern(itemInfos[0]);
				long crc32 = long.Parse(itemInfos[1]);
				long size = long.Parse(itemInfos[2]);
				FilelistItemInfo filelistItemInfo = new FileListItemInfo();
				filelistItemInfo.path = path;
				filelistItemInfo.crc32 = crc32;
				filelistItemInfo.size = size;
				if(!ret.ContainsKey(path))
					ret.Add(path,filelistItemInfo);
				else
					Debug.LogError("重复资源：" + item);
			}
			return ret;
		}

		public static HotUpdateType CompareVer(VersionInfo serverVerInfo,VersionInfo localVerInfo)
		{
			HotUpdateType ret = HotUpdateType.DonotHotUpdate;
			if(sreverVerInfo.packVer > localVerInfo.packVer) return HotUpdateType.PackUpdate;
			string serverTxtVer = serverVerInfo.version;
			string localTxtVer = localVerInfo.version;
			string[] serverVerInfos = serverTxtVer.Split('.');
			string[] localVerInfos = localTxtVer.Split('.');
			for(int i = 0; i < serverVerInfos.Length;i++)
			{
				if(int.Parse(serverVerInfos[i]) > int.Parse(localVerInfos[i]))
					return HotUpdateType.HotUpdate;
			}
			return ret;
		}

		public static void DownLoad(string url,Action<WWW> callback)
		{
			HotUpdateMain.Instance.StartCoroutine(DownloadByWWW(url,callback));
		}
		
		private static IEnumerator DownloadByWWW(string path,Action<WWW> callback)
		{
			WWW www = new WWW(path);
			yield return www;
			if(www.isDone)
			{
				if(callback != null)
					callback(www);
			}
		}
	}


### 热更新逻辑主入口

	public class HotUpdateMain : MonoBehaviour
	{
		public static HotUpdateMain Instance;
		private HptUpdate hotUpdate;
		 
		void Awake()
		{
			Instance = this;
			hotUpdate = new AndroidHotUpdate();
			hotUpdate.hotUpdateFinished = OnHotUpdateFinished;
			hotUpdate.InitVer();
		}
	}
		
	private void OnHotUpdateFinishFinished(VersionInfo serverInfo,VersionInfo localInfo)
	{
		HotUpdateType hotUpdateType = HotUpdateUtil.CompareVer(serverInfo,localInfo);
		AddMainScript();
	}
	
	private void AddMainScript()
	{
		string assemblyPath = string.Format("{0}/Assembly-CSharp.bytes","android");
		string path = "";
		path = File.Exists(Application.persistentDataPath + "/" + assemblyPath) ? 
					"file:///" + Application.persistentDataPath + "/" + assemblyPath : 
					"file:///" + Application.streamingAssetsPath + "/" + assemblyPath;
		HotUpdateUtil.DownLoad(path,LoadedMainAssembly);
	}

	private void LoadedMainAssembly(WWW www)
	{
		Assembly assembly = Assembly.Load(www.bytes);
		var type = assembly.GetType("Main",true);
		gameObject.AddComponent(type);
		www.Dispose();
	}