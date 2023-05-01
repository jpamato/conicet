using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CacheManager : MonoBehaviour
{
    private static string cacheKey = "ContentCached";
    private static string cacheKeySeparator = "_";

    public enum DataType {
        bundle,
        sheet
    }

    public List<CachedElement> cachedElements;
    [Serializable]
    public class CachedElement {
        public DataType type;
        public Data.langs lang;
        public bool cached;

        public void Load() {
            int cached = PlayerPrefs.GetInt(GetKey(), 0);
            this.cached = cached > 0 ? true : false;
        }

        public void SetCached() {
            cached = true;
            PlayerPrefs.SetInt(GetKey(), 1);
        }

        public void PersistSheet(string filename, string sheet) {
            string path = System.IO.Path.Combine(Application.persistentDataPath, GetFilemane(filename) + ".txt");
            Debug.Log("#PersistSheet " + path);
            System.IO.File.WriteAllText(path, sheet);
        }

        public string GetSheet(string filename) {
            string path = System.IO.Path.Combine(Application.persistentDataPath, GetFilemane(filename) + ".txt");
            Debug.Log("#GetSheet " + path);
            return System.IO.File.ReadAllText(path);
        }

        string GetKey() {
            return CacheManager.cacheKey + CacheManager.cacheKeySeparator + type.ToString() + CacheManager.cacheKeySeparator + lang.ToString();
        }

        string GetFilemane(string filename) {
            return filename + CacheManager.cacheKeySeparator + type.ToString() + CacheManager.cacheKeySeparator + lang.ToString();
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        //foreach (DataType dataType in Enum.GetValues(typeof(DataType))) {
        foreach (CachedElement ce in cachedElements)
            ce.Load();
    }
    
    public void SetBundleCached() {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.bundle && x.lang == Data.Instance.lang);
        if (ce != null) 
            ce.SetCached();
    }

    public bool IsBundleCached() {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.bundle && x.lang == Data.Instance.lang);
        if (ce != null)
            return ce.cached;
        else
            return false;
    }

    public void SetSheetCached() {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.sheet && x.lang == Data.Instance.lang);
        if (ce != null)
            ce.SetCached();
    }

    public IEnumerator SaveSheetCached(string filename, string sheet, System.Action callback) {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.sheet && x.lang == Data.Instance.lang);
        if (ce != null)
            ce.PersistSheet(filename,sheet);

        if (callback != null)
            callback();
        yield return null;
    }

    public bool IsSheetCached() {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.sheet && x.lang == Data.Instance.lang);
        if (ce != null)
            return ce.cached;
        else
            return false;
    }

    public string GetSheet(string filename) {
        CachedElement ce = cachedElements.Find(x => x.type == DataType.sheet && x.lang == Data.Instance.lang);
        if (ce != null)
            return ce.GetSheet(filename);
        else
            return "";
    }
}
