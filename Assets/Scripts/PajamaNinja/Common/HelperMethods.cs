using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PajamaNinja.Common
{
    /// <summary>
    /// Methods that are not related to any game
    /// </summary>
    public static class HelperMethods
    {
        /// <summary>
        /// Checks if the current platform is mobile
        /// </summary>
        /// <returns>true if mobile device is used</returns>
        public static bool IsMobile()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Input.touchCount != 0)
            {
                return true;
            }
            if ((Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) && Input.touchCount != 0)
            {
                //   Debug.Log("Using Unity Remote");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a list with all values of a specified Enum
        /// </summary>
        /// <typeparam name="T">the Enum to construct list from</typeparam>
        /// <returns></returns>
        public static List<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }


        /// <summary>
        /// Simple way to convert a point from 3D coordinates to 2D screen coordinates
        /// </summary>
        /// <param name="point3D">a point in a 3D world</param>
        /// <param name="_camera">the camera that sees the above point</param>
        /// <returns>a point in 2D screen space</returns>
        public static Vector2 WorldToScreen(Vector3 point3D, Camera _camera)
        {
            return _camera.WorldToScreenPoint(point3D);
        }


        /// <summary>
        /// Simple way to convert a point from 2D screen coordinate int 3D point
        /// </summary>
        /// <param name="point2D">a point in 2D screen space</param>
        /// <param name="_camera">a camera will see the 3D point</param>
        /// <returns>a point in 3D space</returns>
        public static Vector3 ScreenToWorld(Vector2 point2D, Camera _camera)
        {
            return _camera.ScreenToWorldPoint(new Vector3(point2D.x, point2D.y, -_camera.transform.position.z));
        }

        /// <summary>
        /// Picks a random item from IEnumerable
        /// </summary>
        /// <returns>random item</returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        /// <summary>
        /// Picks a number of random item from IEnumerable
        /// </summary>
        /// <param name="count">number of items</param>
        /// <returns>list of random items</returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        /// <summary>
        /// Shuffle an IEnumerable
        /// </summary>
        /// <returns>shuffled list</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        /// <summary>
        /// Enumerate enumerable
        /// </summary>
        /// <returns>item and index</returns>
        public static IEnumerable<(T item, int index)> Enumerate<T>(this IEnumerable<T> input, int start = 0)
        {
            int i = start;
            foreach (var t in input)
            {
                yield return (t, i++);
            }
        }

        public static IEnumerable<TSource> GetDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer)
        {
            var hash = new HashSet<TKey>(comparer);
            return source.Where(item => !hash.Add(selector(item))).ToList();
        }

        public static IEnumerable<TSource> GetDuplicates<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return source.GetDuplicates(x => x, comparer);
        }

        public static IEnumerable<TSource> GetDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.GetDuplicates(selector, null);
        }

        public static IEnumerable<TSource> GetDuplicates<TSource>(this IEnumerable<TSource> source)
        {
            return source.GetDuplicates(x => x, null);
        }

        /// <summary>
        /// Simple way to invoke action after a time
        /// </summary>
        /// <param name="time">seconds</param>
        /// <param name="action">action to invoke</param>
        public static IEnumerator WaitAndDo(float time, Action action)
        {
            yield return new WaitForSeconds(time);

            action?.Invoke();
        }

        /// <summary>
        /// Simple way to invoke action after a time
        /// </summary>
        /// <param name="time">seconds</param>
        /// <param name="action">action to invoke</param>
        public static IEnumerator WaitUpdateAndDo(Action action)
        {
            yield return new WaitForFixedUpdate();

            action?.Invoke();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Get all instances of scriptable objects with given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllInstances<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList()
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToList();
        }
#endif

        public static void SetVsync1()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
        }

        public static void SetVsync2()
        {
            QualitySettings.vSyncCount = 2;
            Application.targetFrameRate = -1;
        }

        public static void SetVsync0_Default()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
        }

        public static void SetVsync0_60FPS()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        public static void SetVsync0_120FPS()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }

        public static string CurrencyFormat(this long num)
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            var ci = new CultureInfo(currentCulture)
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = "," ,
                    NumberGroupSeparator = " "
                },
            };

            var tempString = num switch
            {
                >= 10000000000000 => String.Format(ci, "<mspace=0.6em>{0:0.#}</mspace>T", num / 1000000000000D),
                >= 1000000000000 => String.Format(ci, "<mspace=0.6em>{0:0.##}</mspace>T", num / 1000000000000D),
                >= 10000000000 => String.Format(ci, "<mspace=0.6em>{0:0.#}</mspace>B", num / 1000000000D),
                >= 1000000000 => String.Format(ci, "<mspace=0.6em>{0:0.##}</mspace>B", num / 1000000000D),
                >= 10000000 => String.Format(ci, "<mspace=0.6em>{0:0.#}</mspace>M", num / 1000000D),
                >= 1000000 => String.Format(ci, "<mspace=0.6em>{0:0.##}</mspace>M", num / 1000000D),
                >= 10000 => String.Format(ci, "<mspace=0.6em>{0:0.#}</mspace>K", num / 1000D),
                _ => String.Format(ci, "<mspace=0.6em>{0:#,#}</mspace>", num),
            };

            tempString = tempString.Replace(ci.NumberFormat.NumberDecimalSeparator, $"</mspace>{ci.NumberFormat.NumberDecimalSeparator}<mspace=0.6em>");
            tempString = tempString.Replace(ci.NumberFormat.NumberGroupSeparator, $"</mspace>{ci.NumberFormat.NumberGroupSeparator}<mspace=0.6em>");

            return tempString;
        }

        public static string CurrencyFormatNoTag(this long num)
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            var ci = new CultureInfo(currentCulture)
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = "," ,
                    NumberGroupSeparator = " "
                },
            };

            var tempString = num switch
            {
                >= 10000000000000 => String.Format(ci, "{0:0.#}T", num / 1000000000000D),
                >= 1000000000000 => String.Format(ci, "{0:0.##}T", num / 1000000000000D),
                >= 10000000000 => String.Format(ci, "{0:0.#}B", num / 1000000000D),
                >= 1000000000 => String.Format(ci, "{0:0.##}B", num / 1000000000D),
                >= 10000000 => String.Format(ci, "{0:0.#}M", num / 1000000D),
                >= 1000000 => String.Format(ci, "{0:0.##}M", num / 1000000D),
                >= 10000 => String.Format(ci, "{0:0.#}K", num / 1000D),
                _ => String.Format(ci, "{0:#,#}", num),
            };

            return tempString;
        }
    }
}