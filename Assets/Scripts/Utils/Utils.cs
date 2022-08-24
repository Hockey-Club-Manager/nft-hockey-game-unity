using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Utils
{
    public static class ImageLoader
    {
        private static string _path = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar.ToString()}Images";

        // retrieves the filename of url without GET params
        private static string GetFilenameFromUrl(string url)
        {
            return url.Split("/").Last().Split("&").First();
        }

        static ImageLoader()
        {
            Directory.CreateDirectory(_path);
        }

        private static string GetPathWithinFilename(string filename)
        {
            return $"{_path}{Path.DirectorySeparatorChar.ToString()}{filename}";
        }
        
        private static void SaveTexture2D(Texture2D texture2D, string path)
        {
            File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }

        private static string SaveImage(Image image, string filename)
        {
            string path = GetPathWithinFilename(filename);
            SaveTexture2D(image.sprite.texture, path);
            return path;
        }

        private static Texture2D LoadTexture2D(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"No file {path} was found");
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(fileData);
            return texture2D;
        }

        private static IEnumerator DownloadTexture2D(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            try
            {
                Texture texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                SaveTexture2D((Texture2D) texture, GetPathWithinFilename(GetFilenameFromUrl(url)));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw new ApplicationException($"Invalid link {url}");
            }
        }

        public static IEnumerator LoadImage(Image image, string url)
        {
            string path = GetPathWithinFilename(GetFilenameFromUrl(url));
            if (File.Exists(path))
            {
                Texture2D texture = LoadTexture2D(path);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            else
            {
                yield return DownloadTexture2D(url);
                Texture2D texture = LoadTexture2D(path);
                bool previousActiveness = image.gameObject.activeSelf;
                image.gameObject.SetActive(true);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.gameObject.SetActive(previousActiveness);
            }
        }
    }
    
    public static class Utils 
    {
        public static IEnumerator LoadImage(Image image, string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            try
            {
                Texture texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                image.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            catch
            {
                Debug.Log("Invalid link");
            }
        }

        public static string ConvertPosition(string position)
        {
            return position switch
            {
                "LeftWing" => "LW",
                "RightWing" => "RW",
                "Center" => "C",
                "LeftDefender" => "LD",
                "RightDefender" => "RD",
                "GoaliePos" => "G",
                "Goalie" => "G",
                _ => position
            };
        }

        public static int GetSliderValueIceTimePriority(string iceTimePriority)
        {
            return iceTimePriority switch
            {
                "SuperLowPriority" => 1,
                "LowPriority" => 2,
                "Normal" => 3,
                "HighPriority" => 4,
                "SuperHighPriority" => 5,
            };
        }

        public static string GetIceTimePriority(int value)
        {
            return value switch
            {
                1 => "SuperLowPriority",
                2 => "LowPriority",
                3 => "Normal",
                4 => "HighPriority",
                5 => "SuperHighPriority",
            };
        }

        public static string GetTactics(int value)
        {
            return value switch
            {
                0 => throw new SwitchExpressionException("Tactics not chosen"),
                1 => "Safe",
                2 => "Defensive",
                3 => "Neutral",
                4 => "Offensive",
                5 => "Aggressive"
            };
        }

        public static int GetFieldPlayerPositionId(string position)
        {
            return position switch
            {
                "LeftWing" => 0,
                "Center" => 1,
                "RightWing" => 2,
                "LeftDefender" => 3,
                "RightDefender" => 4,
            };
        }
    }
}