using System;
using System.Runtime.InteropServices;
using AOT;
using Apple.Core.Runtime;
using UnityEngine;

namespace Apple.CloudKit
{
    public static class NSUbiquitousKeyValueStore
    {
        static NSUbiquitousKeyValueStore()
        {
            RegisterDidChangeExternally();
        }

        #region Set

        #region SetInt32

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetInt32(string key, int value);
        #endregion

        #region Public Interface
        public static void SetInt32(string key, int value)
        {
            NSUbiquitousKeyValueStore_SetInt32(key, value);
        }
        #endregion
        #endregion

        #region SetInt64

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetInt64(string key, long value);
        #endregion

        #region Public Interface
        public static void SetInt64(string key, long value)
        {
            NSUbiquitousKeyValueStore_SetInt64(key, value);
        }
        #endregion
        #endregion

        #region SetDouble

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetDouble(string key, double value);
        #endregion

        #region Public Interface
        public static void SetDouble(string key, double value)
        {
            NSUbiquitousKeyValueStore_SetDouble(key, value);
        }
        #endregion
        #endregion

        #region SetString

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetString(string key, string value);
        #endregion

        #region Public Interface
        public static void SetString(string key, string value)
        {
            NSUbiquitousKeyValueStore_SetString(key, value);
        }
        #endregion
        #endregion

        #region SetBool

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetBool(string key, bool value);
        #endregion

        #region Public Interface
        public static void SetBool(string key, bool value)
        {
            NSUbiquitousKeyValueStore_SetBool(key, value);
        }
        #endregion
        #endregion

        #region SetBinaryData

        #region Interop
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_SetBinaryData(string key, IntPtr data, int dataLength);
        #endregion

        #region Public Interface
        public static void SetBinaryData(string key, byte[] value)
        {
            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);

            NSUbiquitousKeyValueStore_SetBinaryData(key, handle.AddrOfPinnedObject(), value.Length);

            handle.Free();
        }
        #endregion
        #endregion

        #endregion

        #region Get

        #region GetInt32
        [DllImport(InteropUtility.DLLName)]
        private static extern int NSUbiquitousKeyValueStore_GetInt32(string key);

        public static int GetInt32(string key)
        {
            return NSUbiquitousKeyValueStore_GetInt32(key);
        }
        #endregion

        #region GetInt64
        [DllImport(InteropUtility.DLLName)]
        private static extern long NSUbiquitousKeyValueStore_GetInt64(string key);
        
        public static long GetInt64(string key)
        {
            return NSUbiquitousKeyValueStore_GetInt64(key);
        }
        #endregion

        #region GetDouble
        [DllImport(InteropUtility.DLLName)]
        private static extern double NSUbiquitousKeyValueStore_GetDouble(string key);

        public static double GetDouble(string key)
        {
            return NSUbiquitousKeyValueStore_GetDouble(key);
        }

        #endregion

        #region GetString
        [DllImport(InteropUtility.DLLName)]
        private static extern string NSUbiquitousKeyValueStore_GetString(string key);
        
        public static string GetString(string key)
        {
            return NSUbiquitousKeyValueStore_GetString(key);
        }
        #endregion

        #region GetBool
        [DllImport(InteropUtility.DLLName)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool NSUbiquitousKeyValueStore_GetBool(string key);
        
        public static bool GetBool(string key)
        {
            return NSUbiquitousKeyValueStore_GetBool(key);
        }

        #endregion

        #region GetBinaryData
        [DllImport(InteropUtility.DLLName)]
        private static extern InteropStructArray NSUbiquitousKeyValueStore_GetBinaryData(string key);
        public static byte[] GetBinaryData(string key)
        {
            var response = NSUbiquitousKeyValueStore_GetBinaryData(key);
            return response.ToArray<byte>();
        }
        #endregion

        #endregion
        
        #region Synchronize
        [DllImport(InteropUtility.DLLName)]
        private static extern bool NSUbiquitousKeyValueStore_Synchronize();

        public static bool Synchronize()
        {
            return NSUbiquitousKeyValueStore_Synchronize();
        }

        #endregion

        #region DidChangeExternallyNotification
        public static EventHandler<DidChangeExternallyEventArgs> DidChangeExternally;

        internal delegate void DidChangeExternallyDelegate(string jsonResponse);

        [DllImport(InteropUtility.DLLName)]
        private static extern void NSUbiquitousKeyValueStore_RegisterDidChangeExternallyNotification(DidChangeExternallyDelegate callback);

        internal static void RegisterDidChangeExternally()
        {
            NSUbiquitousKeyValueStore_RegisterDidChangeExternallyNotification(OnExternallyChanged);
        }

        [MonoPInvokeCallback(typeof(DidChangeExternallyDelegate))]
        public static void OnExternallyChanged(string jsonResponse)
        {
            try
            {
                var response = JsonUtility.FromJson<DidChangeExternallyResponse>(jsonResponse);
                DidChangeExternally?.Invoke(null, new DidChangeExternallyEventArgs(response));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"NSUbiquitousKeyValueStore: OnExternallyChanged JSON parse error: {e}");
            }
        }
        #endregion
    }
}