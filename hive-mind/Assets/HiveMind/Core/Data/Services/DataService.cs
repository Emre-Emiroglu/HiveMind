using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace HiveMind.Core.Data.Services
{
    public static class DataService
    {
        #region Constants
        private const string KEY = "PXlIJranwuols3EdSXzX00xdJfBYmGYNeN1Qopos/jo=";
        private const string IV = "v+qZgd11TV7AObiv0VyERQ==";
        #endregion

        #region Getters
        private static string GetPath(string relativePath) => Application.persistentDataPath + "/" + relativePath;
        private static bool GetFileExists(string path) => File.Exists(path);
        #endregion

        #region SaveLoad
        public static bool SaveData<T>(T data, string relativePath, bool encrypted)
        {
            string path = GetPath(relativePath);

            try
            {
                bool fileExists = GetFileExists(path);

                CheckOldFileForCreating(path, fileExists);
                CreateFile(data, path, encrypted);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        }
        public static T LoadData<T>(string relativePath, bool encrypted, T defaultData)
        {
            string path = GetPath(relativePath);
            bool fileExists = GetFileExists(path);

            CheckOldFileForLoading(path, fileExists);

            try
            {
                T data = LoadFile<T>(encrypted, path);

                return data;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load data due to: {e.Message} {e.StackTrace}" + ". Created new one!");
                CreateFile(defaultData, path, encrypted);
                return defaultData;
            }
        }
        #endregion

        #region Executes
        private static void CheckOldFileForCreating(string path, bool fileExists)
        {
            string message = fileExists ? "Data Finded And Deleted" : "Data Cannot Finded";

            if (fileExists)
                File.Delete(path);

            Debug.Log(message);
        }
        private static void CheckOldFileForLoading(string path, bool fileExists)
        {
            string message = fileExists ? "Data Finded And Loaded" : "$\"Cannot Load Data at {path}. Data Not Finded \"";

            if (!fileExists)
            {
                Debug.Log($"Cannot Load Data at {path}. Data Not Finded");
            }

            Debug.Log(message);
        }
        private static void CreateFile<T>(T data, string path, bool encrypted)
        {
            string message = encrypted ? "Creating Encrypted File" : "Creating File";

            using FileStream fileStream = File.Create(path);
            Formatting formatting = Formatting.Indented;
            JsonSerializerSettings settings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            if (encrypted)
            {
                using Aes aes = Aes.Create();
                aes.Key = Convert.FromBase64String(KEY);
                aes.IV = Convert.FromBase64String(IV);

                using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                using CryptoStream cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write);

                cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data, formatting, settings)));
            }
            else
            {
                fileStream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data, formatting, settings));
            }

            Debug.Log(message);
        }
        private static T LoadFile<T>(bool encrypted, string path)
        {
            T data;

            string message = encrypted ? "Loading Encrypted File" : "Loading File";

            if (encrypted)
            {
                byte[] fileBytes = File.ReadAllBytes(path);

                using Aes aes = Aes.Create();
                aes.Key = Convert.FromBase64String(KEY);
                aes.IV = Convert.FromBase64String(IV);

                using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream deCryptionStream = new MemoryStream(fileBytes);
                using CryptoStream cryptoStream = new CryptoStream(deCryptionStream, cryptoTransform, CryptoStreamMode.Read);
                using StreamReader reader = new StreamReader(cryptoStream);

                string result = reader.ReadToEnd();

                data = JsonConvert.DeserializeObject<T>(result);

                Debug.Log($"Decrypted result: {result}");
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }

            Debug.Log(message);

            return data;
        }
        #endregion
    }
}
