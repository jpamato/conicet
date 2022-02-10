using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Conicet.AssetsBundle
{
    public class ForceAcceptAll : CertificateHandler
    {
         protected override bool ValidateCertificate(byte[] certificateData)
         {
             return true;
         }
    }

    public class AssetsBundleLoader : MonoBehaviour
    {
        states state;
        enum states
        {
            FIRST_BUNDLES,
            SECOND_BUNDLES
        }
        private float downloadProgress = 0.0f;
        private List<string> dataPaths;
        string loadedHashes;
        private float downloadedBytes = 0f;
        private string currentPack;
        public Dictionary<string, AssetBundle> bundles;
        public bool allLoaded;
        int totalFirstBundles = 0;
        int totalGirls = 0;
        string url;
        bool isFirstTime;

#if UNITY_IOS
    string mainBundlePath = "iOS";
#elif UNITY_WEBGL
    string mainBundlePath = "WebGL";
#elif UNITY_STANDALONE
    string mainBundlePath = "Standalone";
#else
    string mainBundlePath = "Android";
#endif

        public string CurrentPack
        {
            get => currentPack;
            set => currentPack = value;
        }

        public float Progress
        {
            get => downloadProgress;
        }
        public void ResetAll()
        {
            foreach (string assetName in dataPaths)
            {
                try
                {
                    AssetBundle ab = bundles[assetName];
                    if (ab != null)
                        ab.Unload(false);
                }
                catch
                {
                    Debug.Log("Dictionary empty");
                }

            }
        }
        public string GetHashFor(string key)
        {
            return manifest.GetAssetBundleHash(key).ToString();
        }
        public float DownloadedMegas()
        {
            return downloadedBytes / 1000000.0f;
        }
        AssetBundleManifest manifest;
        Action<string> onSuccess;
        AssetBundle mainBundle;



        public IEnumerator DownloadAll(string _url, Action<string> onSuccess)
        {
            if (dataPaths == null)
            {
                isFirstTime = true;
                bundles = new Dictionary<string, AssetBundle>();
                dataPaths = new List<string>();
            }
            else
            {
                dataPaths.Clear();
                isFirstTime = false;
            }

            Events.OnLoading("Bundles");
            this.url = _url + mainBundlePath + "/";
            

            Debug.Log("Vuelve a Bajar: " + isFirstTime + "   mainBundle " + mainBundle + "   dataPaths: " + dataPaths);
            this.onSuccess = onSuccess;

            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url + mainBundlePath))
            {
                var cert = new ForceAcceptAll();
                request.certificateHandler = cert;
                Debug.Log("Loading from url : " + url + mainBundlePath);
                AsyncOperation op = request.SendWebRequest();
                while (!op.isDone)
                {
                    downloadProgress = request.downloadProgress;
                    downloadedBytes = request.downloadedBytes;
                    Events.OnLoadingProgress(request.downloadProgress);
                    yield return new WaitForEndOfFrame();
                }
                Debug.Log("Loading Manifest done");
                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(request.error);
                    onSuccess("error");
                }
                else
                {
                    Debug.Log("_________________________Manifest LOADED");
                    mainBundle = DownloadHandlerAssetBundle.GetContent(request);
                    manifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

                    foreach (string bundleName in manifest.GetAllAssetBundles())
                    {
                        bool addIT;
                        if (Data.Instance.lang == Data.langs.QOM)
                        {
                            if (bundleName.Contains("qom"))
                                addIT = true;
                        }
                        else
                        {
                            addIT = true;
                            if (bundleName.Contains("qom"))
                                addIT = false;
                        }
                        dataPaths.Add(bundleName);
                    }

                    totalFirstBundles = dataPaths.Count;

                    StartCoroutine(LoadBundlesFromManifest());
                    mainBundle.Unload(false);
                }
            }

        }


        public IEnumerator LoadBundlesFromManifest()
        {
            int bundleID = 0;
            foreach (string key in dataPaths)
            {
                Hash128 hash = manifest.GetAssetBundleHash(key);
                if (isFirstTime)
                    loadedHashes += hash.ToString() + "_";
                else
                {
                    if (CheckIfHashIsNew(hash.ToString(), bundleID))
                    {
                        StopAllCoroutines();
                        // Data.Instance.ResetAll();
                    }
                }
                bundleID++;
                if (isFirstTime)
                {
                    CurrentPack = key;
                    yield return DownloadAndCacheAssetBundle(key, hash, OnLoaded);
                }
            }
            if (!isFirstTime)
                onSuccess("nothing new!");
        }
        bool CheckIfHashIsNew(string hash, int id)
        {
            string[] arr = loadedHashes.Split("_"[0]);
            if (id <= arr.Length)
            {
                //Debug.Log("saved hash: " + arr[id] + "   the hash: " + hash);
                if (arr[id] == hash)
                    return false;
            }
            return true;
        }

        int loadedParts = 0;
        void OnLoaded(bool isLoaded)
        {
            loadedParts++;
            if (loadedParts >= dataPaths.Count)
                onSuccess("ok");
        }
        IEnumerator DownloadAndCacheAssetBundle(string uri, Hash128 hash, System.Action<bool> OnLoaded)
        {
            Events.OnLoading(uri);
            string realURL = url + uri;
            Debug.Log("Load: " + realURL);
            UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(realURL, hash);

            var cert = new ForceAcceptAll();
            uwr.certificateHandler = cert;

            using (uwr)
            {
                var operation = uwr.SendWebRequest();
          
                while (!operation.isDone)
                {
                    Events.OnLoadingProgress(uwr.downloadProgress);
                    yield return null;
                }
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Error downloading assetBundle: " + realURL);
                    onSuccess("error");
                    yield break;
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);                    

                    if (bundles == null)
                        bundles = new Dictionary<string, AssetBundle>();
                    bundles.Add(uri, bundle);

                    OnLoaded(true);
                   // bundle.Unload(false);
                }
            }
        }





        //public GameObject GetAsset(string bundleName, string asset)
        //{
        //   // Debug.Log("GET Asset  bundleName: " + bundleName + " asset: " + asset);
        //    AssetBundle assetBundle = bundles[bundleName];
        //    GameObject go = assetBundle.LoadAsset(asset) as GameObject;
        //    return go;
        //}
        //public TextAsset GetAssetAsText(string bundleName, string asset)
        //{
        //    //   print("GET Asset  bundleName: " + bundleName + " asset: " + asset);
        //    AssetBundle assetBundle = bundles[bundleName];
        //    TextAsset go = assetBundle.LoadAsset(asset) as TextAsset;
        //    return Instantiate(go);
        //}
        public AudioClip GetAssetAsAudioClip(string bundleName, string asset)
        {
            Debug.Log("Get AudioClip: " + bundleName + " asset: " + asset);
            if (!bundles.ContainsKey(bundleName)) return null;
            AssetBundle assetBundle = bundles[bundleName];
            AudioClip go = assetBundle.LoadAsset(asset) as AudioClip;
            return go;
        }
        public Texture2D GetAssetAsTexture2D(string bundleName, string asset)
        {
            Debug.Log("Get Sprite: " + bundleName + " asset: " + asset);
            if (!bundles.ContainsKey(bundleName)) return null;
            AssetBundle assetBundle = bundles[bundleName];
            Texture2D go = assetBundle.LoadAsset(asset) as Texture2D;
            return go;
        }
    }
}