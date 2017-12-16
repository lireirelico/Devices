﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Management; 

namespace IIPU_lab4_GUI
{
	
	class Controller
	{
		Form1 form;	
		//Timer updateTimer;
		public List<Device> deviceList = new List<Device>();
		

		public Controller(Form1 formArg)
		{
			form = formArg;
			/*
			updateTimer = new Timer();
			updateTimer.Interval = 1000; //ms
			updateTimer.Enabled = true;
			updateTimer.Tick += UpdateUsbInfo;
			*/
			UpdateUsbInfo(null, null);
		}


		private void UpdateUsbInfo(object sender, EventArgs e)
		{
			//deviceList = newDeviceList;

			form.cb_List.Items.Clear();



			deviceList = new List<Device>();
			var devices = new ManagementObjectSearcher("SELECT * FROM Win32_PNPEntity");
      
			
			
			      
			foreach (ManagementObject devObj in devices.Get())
			{

			  var device=new Device
				{
					Name = devObj["Name"]                 != null ? devObj["Name"].ToString()         : "",
					ClassGuid = devObj["ClassGuid"]       != null ? devObj["ClassGuid"].ToString()    : "",
					HardwareID = devObj["HardwareID"]     != null ? (string[])devObj["HardwareID"]    : null,
					Manufacturer = devObj["Manufacturer"] != null ? devObj["Manufacturer"].ToString() : "",
					DeviceID = devObj["DeviceID"]         != null ? devObj["DeviceID"].ToString()     : "",
					Enabled = devObj["Status"].ToString() == "OK"
				};



				foreach (var sys in devObj.GetRelated("Win32_SystemDriver"))
				{
					device.AddSysFile(sys["PathName"].ToString(), sys["Description"].ToString());
					}
				deviceList.Add(device);
			}
 



			if (deviceList.Count == 0)
			{
				form.cb_List.Text = "<none>";
			}
			else
			{
			  foreach(Device device in deviceList)
				{
					form.cb_List.Items.Add(device.Name);
				}

				//Updating combobox text if nessesary.
				var selectedDeviceDisconnected=true;
	
				if (form.cb_List.Text!="")
				{
					foreach(Device device in deviceList)
					{
						if (false)//(!device.IsWPD && (device.Letter[0]) == (form.cb_List.Text[0]))
						{
							selectedDeviceDisconnected = false;
							break;
						}
					}
				}
	
				if (selectedDeviceDisconnected)
				{
					//form.cb_List.Text=deviceList[0].Letter + " " + deviceList[0].Name;
				}
				//Updating combobox text if nessesary.

			}


			form.comboBox1_SelectedIndexChanged(null, null);

		}
		
		private List<Device> GetConnectedUsbDevices()
		{
			var listBuf = new List<Device>();
			return listBuf;
		}

	}
}
