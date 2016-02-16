﻿using System.Collections.Generic;
using System.IO;
using System.Threading;
using CETech.Utils;

namespace CETech
{
    public static class ResourceManager
    {
        public delegate object ResourceLoader(Stream input);

        public delegate void ResourceOffline(object data);

        public delegate void ResourceOnline(object data);

        public delegate void ResourceUnloader(object data);

        public static bool _autoReload;

        private static readonly Dictionary<long, ResourceLoader> _loader_map =
            new Dictionary<long, ResourceLoader>();

        private static readonly Dictionary<long, ResourceUnloader> _unloader_map =
            new Dictionary<long, ResourceUnloader>();

        private static readonly Dictionary<long, ResourceOnline> _online_map =
            new Dictionary<long, ResourceOnline>();

        private static readonly Dictionary<long, ResourceOffline> _offline_map =
            new Dictionary<long, ResourceOffline>();

        private static readonly Dictionary<long, int> _types_map = new Dictionary<long, int>();

        private static readonly List<Dictionary<long, object>> _data_map =
            new List<Dictionary<long, object>>();

        private static readonly List<Dictionary<long, int>> _ref_map = new List<Dictionary<long, int>>();

        private static SpinLock _add_lock = new SpinLock();

        public static bool AutoReload
        {
            get { return _autoReload; }
            set { _autoReload = value; }
        }

        public static void RegisterType(long type, ResourceLoader loader, ResourceUnloader unloader,
            ResourceOnline online, ResourceOffline offline)
        {
            var idx = _data_map.Count;
            _data_map.Add(new Dictionary<long, object>());
            _ref_map.Add(new Dictionary<long, int>());

            _types_map[type] = idx;

            _loader_map[type] = loader;
            _unloader_map[type] = unloader;
            _online_map[type] = online;
            _offline_map[type] = offline;
        }

        public static object[] Load(long type, long[] names)
        {
            var data = new object[names.Length];

            for (var i = 0; i < names.Length; i++)
            {
                Log.Debug("resource_manager", "Loading resource {0:x}{1:x}", type, names[i]);

                var input = FileSystem.Open("build", string.Format("{0:x}{1:x}", type, names[i]),
                    FileSystem.OpenMode.Read);
                data[i] = _loader_map[type](input);
            }

            return data;
        }

        public static void AddLoaded(object[] loaded_data, long type, long[] names)
        {
            var online = _online_map[type];
            var idx = _types_map[type];

            var gotLock = false;
            try
            {
                _add_lock.Enter(ref gotLock);

                for (var i = 0; i < names.Length; i++)
                {
                    incRef(idx, names[i]);

                    _data_map[idx][names[i]] = loaded_data[i];

                    online(_data_map[idx][names[i]]);
                }
            }
            finally
            {
                if (gotLock) _add_lock.Exit();
            }
        }

        public static void LoadNow(long type, long[] names)
        {
            var loaded_data = Load(type, names);
            AddLoaded(loaded_data, type, names);
        }

        public static bool CanGet(long type, long[] names)
        {
            var idx = _types_map[type];
            for (var i = 0; i < names.Length; i++)
            {
                if (!_data_map[idx].ContainsKey(names[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static void Unload(long type, long[] names)
        {
            var offline = _offline_map[type];
            var unloader = _unloader_map[type];

            var idx = _types_map[type];

            for (var i = 0; i < names.Length; i++)
            {
                offline(_data_map[idx][names[i]]);
            }

            for (var i = 0; i < names.Length; i++)
            {
                if (decRef(idx, names[i]))
                {
                    unloader(_data_map[idx][names[i]]);

                    _data_map[idx].Remove(names[i]);
                    _ref_map[idx].Remove(names[i]);
                }
            }
        }

        private static void incRef(int type_idx, long name)
        {
            int counter;

            if (!_ref_map[type_idx].TryGetValue(name, out counter))
            {
                counter = 0;
            }

            _ref_map[type_idx][name] = counter + 1;
        }

        private static bool decRef(int type_idx, long name)
        {
            _ref_map[type_idx][name] -= 1;

            return _ref_map[type_idx][name] == 0;
        }

        public static T Get<T>(long type, long name)
        {
            long[] names = {name};

            if (_autoReload && !CanGet(type, names))
            {
                LoadNow(type, names);
            }

            var idx = _types_map[type];
            return (T) _data_map[idx][name];
        }
    }
}