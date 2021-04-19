using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;

namespace CloudContentDelivery
{
    public class AddressableConfigController
    {
        public static ParametersBucket pb = ParametersBucket.GetParametersBucket();
        public static ParametersAddressableConfig pac = ParametersAddressableConfig.GetParametersAddressableConfig();

        public static string GetRemoteLoadPath(string badgeName)
        {
            if (!Util.checkCosKey() || string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return "";
            }

            if (string.IsNullOrEmpty(badgeName) || pac.useLatest)
            {
                badgeName = "latest";
            }

            string host = pac.useContentServer ? Parameters.proxyHost : Parameters.apiHost;
            string remoteLoadPath = host + "client_api/v1/buckets/" + pb.selectedBucketUuid + "/release_by_badge/" + badgeName + "/entry_by_path/content/?path=";
            return remoteLoadPath;
        }

        public static void SetRemoteLoadPath(string badgeName)
        {
            if (!Util.checkCosKey() || string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return;
            }

            if (string.IsNullOrEmpty(badgeName) || pac.useLatest)
            {
                badgeName = "latest";
            }

            string host = pac.useContentServer ? Parameters.proxyHost : Parameters.apiHost;
            pac.remoteLoadUrl = host + "client_api/v1/buckets/" + pb.selectedBucketUuid + "/release_by_badge/" + badgeName + "/entry_by_path/content/?path=";
            AddressableUtil.setRemoteLoadPath(pac.remoteLoadUrl);
        }
    }
}