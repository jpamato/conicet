using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conicet.AssetsBundle
{
    public class AssetsBundleManager : MonoBehaviour
    {
        public bool forceServerDownload;

        static AssetsBundleManager mInstance = null;
        public AssetsBundleLoader assetsBundleLoader;
        public static AssetsBundleManager Instance  { get  {  return mInstance; }  }
        System.Action<string> OnDone;

        bool esp_loaded = false;
        bool qom_loaded = false;

        void Awake()
        {
            if (!mInstance)
                mInstance = this;
            else
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            Events.LoadAssetBundles += LoadAssetBundles;
        }
        private void OnDestroy()
        {
            Events.LoadAssetBundles -= LoadAssetBundles;
        }
        void LoadAssetBundles(System.Action<string> OnDone)
        {
            if (Data.Instance.lang == Data.langs.QOM)
            {
                if (qom_loaded)
                    OnDone("ok");
                qom_loaded = true;
            } else
            {
                if (esp_loaded)
                    OnDone("ok");
                esp_loaded = true;
            }
            this.OnDone = OnDone;
            if(Data.Instance.mode == Data.modes.OFFLINE)
            {
                LoadFromServer(Data.Instance.url);
                return;
            }
#if UNITY_EDITOR
            if (forceServerDownload)
            {
                LoadFromServer(Data.Instance.url);
                return;
            }
            LoadLocal();
#elif UNITY_WEBGL
            LoadFromServer("../");
#elif UNITY_ANDROID || UNITY_STANDALONE
            LoadFromServer(Data.Instance.url);
#endif
        }
        void LoadLocal()
        {
            StartCoroutine(assetsBundleLoader.DownloadAll(Application.dataPath + "/AssetBundles/", AllLoaded));
        }
        void LoadFromServer(string _url)
        {
            StartCoroutine(assetsBundleLoader.DownloadAll(_url, AllLoaded));
        }
        void AllLoaded(string _url)
        {
            OnDone(_url);
        }
        public Sprite GetSprite(string folder, string asset)
        {
            string lang = "";
            if (Data.Instance.lang == Data.langs.ESP || Data.Instance.lang == Data.langs.L1)
                lang = "esp";
            else
                lang = "qom";
            Texture2D tex = assetsBundleLoader.GetAssetAsTexture2D(folder +"." + lang, asset);
            if (tex == null)
                tex = assetsBundleLoader.GetAssetAsTexture2D(folder + ".generic", asset);
            //if (tex == null)
            //    tex = assetsBundleLoader.GetAssetAsTexture2D(folder + ".esp", asset);
            if (tex == null)
            {
                Debug.Log("[ERROR] No hay imagen para " + folder + "." + lang + " (o.generic)/ " + asset);
                return null;
            }
            else
            {
                Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                return mySprite;
            }
        }
        public AudioClip GetAudioClip(string folder, string asset)
        {
            string lang = "";
            if (Data.Instance.lang == Data.langs.ESP || Data.Instance.lang == Data.langs.L1)
                lang = "esp";
            else
                lang = "qom";
            AudioClip a = assetsBundleLoader.GetAssetAsAudioClip(folder + "." + lang, asset);
            if (a == null)
                a = assetsBundleLoader.GetAssetAsAudioClip(folder + ".generic", asset);
            if (a == null)
            {
                Debug.Log("[ERROR] No hay audio para " + folder + "." + lang + "(o.generic)/" + asset);
                return null;
            }
            else
            {
                return a;
            }
        }
    }    
}