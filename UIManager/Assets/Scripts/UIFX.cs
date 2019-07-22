using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIFrameWork.FX
{
    public class UIFX : MonoBehaviour
    {
        private static UIFX INS;
        internal static UIFX _S
        {
            get
            {
                if (INS == null)
                {
                   INS = new GameObject().AddComponent<UIFX>();
                    INS.name = "UIFX";
                    DontDestroyOnLoad(INS.gameObject);
                }
                return INS;
            }

        }

        public void FadeInFX(GameObject Panel, float updatetime, float speed, Action CallBack)
        {
            StartCoroutine(FadeInCanvasGroup(Panel, updatetime, speed, CallBack));
        }
        public void FadeOutFX(GameObject Panel, float updatetime, float speed, Action CallBack)
        {
            StartCoroutine(FadeOutCanvasGroup(Panel, updatetime, speed, CallBack));
        }
        public static IEnumerator FadeInCanvasGroup(GameObject Panel, float updatetime, float speed, Action CallBack)
        {

            if (Panel == null)
            {
                Debug.LogErrorFormat("无法更新{0}的淡入效果，它为空", Panel);
                yield return null;
            }
            CanvasGroup canvasGroup = null;
            try
            {
                canvasGroup = Panel.GetComponent<CanvasGroup>();
            }
            catch
            {
                canvasGroup = Panel.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
            WaitForSeconds seconds = new WaitForSeconds(updatetime);
            while (canvasGroup.alpha < 1)
            {

                yield return seconds;
                canvasGroup.alpha += speed;
            }
            CallBack?.Invoke();
        }
        public static IEnumerator FadeOutCanvasGroup(GameObject Panel, float updatetime, float speed, Action CallBack)
        {

            if (Panel == null)
            {
                Debug.LogErrorFormat("无法更新{0}的淡入效果，它为空", Panel);
                yield return null;
            }
            CanvasGroup canvasGroup = null;
            try
            {
                canvasGroup = Panel.GetComponent<CanvasGroup>();
            }
            catch
            {
                canvasGroup = Panel.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1;
            WaitForSeconds seconds = new WaitForSeconds(updatetime);
            while (canvasGroup.alpha > 0)
            {
                yield return seconds;
                canvasGroup.alpha -= speed;
            }
            CallBack?.Invoke();
        }
    }
}

