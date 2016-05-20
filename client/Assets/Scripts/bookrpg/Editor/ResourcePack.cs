﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;
using UnityEditor;
using bookrpg.resource;

namespace bookrpg.Editor
{
    [Serializable]
    public class ResourcePack : IResourcePack
    {
        public string srcFile
        {
            get;
            set;
        }

        public string targetFile
        {
            get;
            set;
        }

        public int size
        {
            get;
            set;
        }

        public int version
        {
            get;
            set;
        }

        public string[] resources
        {
            get;
            set;
        }

        public string[] dependencies
        {
            get;
            set;
        }

        public bool beDependent
        {
            get;
            set;
        }

        public uint crc
        {
            get;
            set;
        }

        public string hash
        {
            get;
            set;
        }

        public string packType
        {
            get;
            set;
        }

        public string encryption
        {
            get;
            set;
        }

        public string compression
        {
            get;
            set;
        }

        public void createTargetFile(PackNamePattern namePattern)
        {
            switch (namePattern)
            {
                case PackNamePattern.FileName:
                    targetFile = getFileName();
                    break;
                case PackNamePattern.FileNameWithHash:
                    targetFile = getFileName() + "_" + hash;
                    break;
                case PackNamePattern.FileNameWithVersion:
                    targetFile = getFileName() + "_" + version.ToString();
                    break;
                case PackNamePattern.Hash:
                    targetFile = hash;
                    break;
                case PackNamePattern.PathName:
                    targetFile = srcFile;
                    break;
                case PackNamePattern.PathNameWithHash:
                    targetFile = srcFile + "_" + hash;
                    break;
                case PackNamePattern.PathNameWithVersion:
                    targetFile = srcFile + "_" + version.ToString();
                    break;
            }
        }

        private string getFileName()
        {
            if (resources == null || resources.Length == 0)
            {
                return srcFile;
            }

            string path = resources[0];
            var pos = path.LastIndexOf('/');
            return pos > -1 ? path.Substring(pos + 1) : path;
        }

        public AssetBundleBuild createBuild()
        {
            var ab = new AssetBundleBuild();
            var pos = srcFile.LastIndexOf('.');
            if (pos > 0)
            {
                ab.assetBundleName = srcFile.Substring(0, pos);
                ab.assetBundleVariant = srcFile.Substring(pos + 1);
            } else
            {
                ab.assetBundleName = srcFile;
            }

            ab.assetNames = resources;

            return ab;
        }

        public void fromJson(JsonData data)
        {
            srcFile = (string)data["srcFile"];
            version = (int)data["version"];
            hash = (string)data["hash"];
        }

        public string toReleaseJson()
        {
            var jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.PrettyPrint = true;

            jw.WritePropertyName("srcFile");
            jw.Write(srcFile);

            jw.WritePropertyName("targetFile");
            jw.Write(targetFile);

            jw.WritePropertyName("size");
            jw.Write(size);

            jw.WritePropertyName("version");
            jw.Write(version);

            jw.WritePropertyName("beDependent");
            jw.Write(beDependent);

            if (crc > 0)
            {
                jw.WritePropertyName("crc");
                jw.Write(crc);
            }

            if (!string.IsNullOrEmpty(packType))
            {
                jw.WritePropertyName("packType");
                jw.Write(packType);
            }

            if (!string.IsNullOrEmpty(compression))
            {
                jw.WritePropertyName("encryption");
                jw.Write(encryption);
            }

            if (!string.IsNullOrEmpty(compression))
            {
                jw.WritePropertyName("compression");
                jw.Write(compression);
            }

            if (resources != null && resources.Length > 0)
            {
                jw.WritePropertyName("resources");
                jw.WriteArrayStart();
                foreach (var item in resources)
                {
                    jw.Write(item);
                }
                jw.WriteArrayEnd();
            }

            if (dependencies != null && dependencies.Length > 0)
            {
                jw.WritePropertyName("dependencies");
                jw.WriteArrayStart();
                foreach (var item in dependencies)
                {
                    jw.Write(item);
                }
                jw.WriteArrayEnd();
            }

            jw.WriteObjectEnd();

            return jw.ToString();
        }
    }
}