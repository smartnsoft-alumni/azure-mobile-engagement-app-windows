﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Microsoft.Azure.Engagement;
using Windows.UI.Xaml.Input;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.ApplicationModel.DataTransfer;
using Azme.Resources;

namespace Azme.Pages
{
  public sealed partial class GetDeviceIdPage : EngagementPage
  {
    private string deviceId;

    public GetDeviceIdPage()
    {
      this.InitializeComponent();

      DataTransferManager.GetForCurrentView().DataRequested += GetDeviceIdPage_DataRequested;

      var deviceInformations = new EasClientDeviceInformation();
      this.deviceId = EngagementAgent.Instance.GetDeviceId();
    }

    protected override string GetEngagementPageName()
    {
      return "get_device_id";
    }

    private void ButtonShare_Tapped(object sender, TappedRoutedEventArgs e)
    {
      EngagementAgent.Instance.SendEvent(Constants.Engagement.EVENT_DEVICE_ID_SHARE);
      if (deviceId != null)
      {
        DataTransferManager.ShowShareUI();
      }
    }

    private void ButtonCopy_Tapped(object sender, TappedRoutedEventArgs e)
    {
      EngagementAgent.Instance.SendEvent(Constants.Engagement.EVENT_DEVICE_ID_COPY);
      if (deviceId != null)
      {
        var dataPackage = new DataPackage();
        dataPackage.RequestedOperation = DataPackageOperation.Copy;
        dataPackage.SetText(deviceId);
        Clipboard.SetContent(dataPackage);

        TextBlockCopyDone.Text = string.Format(Strings.Get("GetDeviceIDNotifyCopyDone"), deviceId);
        StoryboardCopyDoneNotification.Begin();
      }
    }

    private void GetDeviceIdPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
    {
      args.Request.Data.Properties.Title = "Device ID";
      args.Request.Data.SetText(deviceId);
    }
  }
}
