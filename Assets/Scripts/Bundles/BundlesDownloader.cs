using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Bundles
{
    public class BundlesDownloader : MonoBehaviour
    {
        private BundlesDownloader m_instance;
        [SerializeField] Text progress, info;
        [SerializeField] Image progressBar;
        [SerializeField] GameObject panelRetry;
        private Canvas canvas;

        private static string Adress { get; } = "91.228.48.18/FIRST/"
#if UNITY_ANDROID
        + "Android/";
#elif  UNITY_IOS
 + "IOS/";
#else
         + "Win/";
#endif
        private static string Manifest { get; } =
#if UNITY_ANDROID
         "Android";
#elif UNITY_IOS
         "IOS";
#else
         "Win";
#endif

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
            DontDestroyOnLoad(gameObject);
            panelRetry.SetActive(false);
            canvas = GetComponent<Canvas>();
        }
        private void Start()
        {
            DownloadUpdate();
        }

        public void DownloadUpdate()
        {
            panelRetry.SetActive(false);
            StartCoroutine(Initial());
        }
        private IEnumerator Initial()
        {
         //   info.text = LocalizationManager.Instance.GetText("Update.Connecting");
            progress.text = "";
            progressBar.fillAmount = 0.0f;
            UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(Adress + Manifest); // assetBundle also have data.

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                panelRetry.SetActive(true);
                yield break;
            }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
          
            //  bundle.Unload(false);
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //    info.text = LocalizationManager.Instance.GetText("Update.Checking");
            while (!Caching.ready) yield return null;
            ulong totalBytesDownload = 0;
          

            string[] _bundles = manifest.GetAllAssetBundles();
            for (int i = 0; i < _bundles.Length; i++)
            {
                float _p = (i + 1) / (float)_bundles.Length;
                progress.text = $"{(_p * 100.0f).ToString("0.0")}%";
                progressBar.fillAmount = _p;
                if (Caching.IsVersionCached(Adress + _bundles[i], manifest.GetAssetBundleHash(_bundles[i])))
                {
                    continue;
                }
                Caching.ClearAllCachedVersions(_bundles[i]);

                UnityWebRequest uwr = UnityWebRequest.Head(Adress + _bundles[i]);
                yield return uwr.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    panelRetry.SetActive(true);
                    yield break;
                }

                string size = uwr.GetResponseHeader("Content-Length");
                int.TryParse(size, out int result);

                totalBytesDownload += (ulong)result;

            }

          //  info.text = LocalizationManager.Instance.GetText("Update.Downloading");
            BundlesManager.Clear();
            ulong bytesDownload = 0;
            for (int i = 0; i < _bundles.Length; i++)
            {

                UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(Adress + _bundles[i], manifest.GetAssetBundleHash(_bundles[i]));
                unityWebRequest.SendWebRequest();
                while (!unityWebRequest.isDone)
                {
                  //  progress.text = BytesFormat.SizeSuffix(bytesDownload + unityWebRequest.downloadedBytes) + "/" + BytesFormat.SizeSuffix(totalBytesDownload);
                    progressBar.fillAmount = (float)((bytesDownload + unityWebRequest.downloadedBytes) / (double)totalBytesDownload);
                    yield return null;
                }
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(unityWebRequest.error);
                    panelRetry.SetActive(true);
                    yield break;
                }
                bytesDownload += unityWebRequest.downloadedBytes;
                AssetBundle content = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
                BundlesManager.Add(content);
            }
            canvas.enabled = false;
              bundle.Unload(false);
            BundlesManager.LoadingComplete();
        }
    }
}