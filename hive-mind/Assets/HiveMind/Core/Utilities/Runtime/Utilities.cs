using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HiveMind.Core.Utilities.Runtime
{
    public static class Utilities
    {
#if UNITY_EDITOR
        public static void CreateTag(string tag)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
            if (asset != null)
            { // sanity checking
                var so = new SerializedObject(asset);
                var tags = so.FindProperty("tags");

                var numTags = tags.arraySize;
                // do not create duplicates
                for (int i = 0; i < numTags; i++)
                {
                    var existingTag = tags.GetArrayElementAtIndex(i);
                    if (existingTag.stringValue == tag) return;
                }

                tags.InsertArrayElementAtIndex(numTags);
                tags.GetArrayElementAtIndex(numTags).stringValue = tag;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
#endif

        public static IEnumerator SetCanvasGroupAlpha(CanvasGroup canvasGroup, float targetValue, float duration = 1f)
        {
            float t = 0f;
            float startValue = canvasGroup.alpha;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);
                yield return null;
            }
        }
        
        public static Vector3 WorldToScreenPointForUICamera(Vector3 worldPos, Camera gameCamera, Canvas screenCanvas)
        {
            Vector3 canvasPos;
            Vector3 screenPos = gameCamera.WorldToScreenPoint(worldPos);

            if (screenCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                canvasPos = screenPos;
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                screenCanvas.transform as RectTransform, screenPos, screenCanvas.worldCamera, out var posRect2D);
                canvasPos = screenCanvas.transform.TransformPoint(posRect2D);
            }

            return canvasPos;
        }

        public static List<T> Shuffle<T>(List<T> ts)
        {
            var newList = ts;
            var count = newList.Count;
            var last = count - 1;
            for (int i = 0; i < last; i++)
            {
                var r = Random.Range(i, count);
                (newList[r], newList[i]) = (newList[i], newList[r]);
            }
            return newList;
        }

        public static IList<int> BubbleSort(IList<int> ts)
        {
            var newList = ts;
            int count = newList.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1; j++)
                {
                    if (newList[j] > newList[j + 1])
                    {
                        int tmp = newList[j];
                        newList[j] = newList[j + 1];
                        newList[j + 1] = tmp;
                    }
                }
            }
            return newList;
        }

        public static Matrix4x4 IsoMatrix(Quaternion rotate) => Matrix4x4.Rotate(rotate);
    }
}
