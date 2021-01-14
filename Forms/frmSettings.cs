/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2014-2015 Anthony Birkett
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudFlareDDNS.Classes.JsonObjects.Cloudflare;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Settings form - basic configuration
    /// </summary>
    public partial class frmSettings : Form
    {
        bool loading_zones = false;
        /// <summary>
        /// Form entry point
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();
        }//end frmSettings()

        /// <summary>
        /// Load the saved values on open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettings_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Program.settingsManager.getSetting("APIUrl").ToString()))
            {
                cloudflare_api_url_input.Text = "https://api.cloudflare.com/client/v4";
                Program.settingsManager.setSetting("APIUrl", cloudflare_api_url_input.Text);
            }

            txtEmailAddress.Text = Program.settingsManager.getSetting("EmailAddress").ToString();
            txtAPIKey.Text = Program.settingsManager.getSetting("APIKey").ToString();
            txtFetchTime.Text = Program.settingsManager.getSetting("FetchTime").ToString();
            cbEventLog.Checked = Program.settingsManager.getSetting("UseEventLog").ToBool();
            IPV6UpdateURL.Text = Program.settingsManager.getSetting("IPV6UpdateURL").ToString();
            IPV4UpdateURL.Text = Program.settingsManager.getSetting("IPV4UpdateURL").ToString();
            StartMinimized.Checked = Program.settingsManager.getSetting("StartMinimized").ToBool();
            cloudflare_api_url_input.Text = Program.settingsManager.getSetting("APIUrl").ToString();
            UseInternalIP_input.Checked = Program.settingsManager.getSetting("UseInternalIP").ToBool();
            HideSRV_input.Checked = Program.settingsManager.getSetting("HideSRV").ToBool();
            show_local_ipv4.Text = NetworkInterfaceManager.GetDefaultIP();
            show_local_ipv6.Text = NetworkInterfaceManager.GetDefaultIP(System.Net.Sockets.AddressFamily.InterNetworkV6);
            enable_network_interface_change.Checked = Program.settingsManager.getSetting("EnableNetworkInterfaceChange").ToBool();
            network_interface_selector_reset_btn.Enabled = enable_network_interface_change.Checked;
            network_interface_selector.Enabled = enable_network_interface_change.Checked;
            
            load_network_interfaces();
        }//end frmSettings_Load()

        private void load_network_interfaces()
        {
            string selectedInterface = NetworkInterfaceManager.GetCurrentDefaultInterface().Name;
            foreach (var item in NetworkInterfaceManager.GetAllNetworkInterfaces())
            {
                network_interface_selector.Items.Add(item.Name);
                if(selectedInterface != "" && selectedInterface == item.Name)
                {
                    network_interface_selector.SelectedItem = item.Name;
                }
            }
           
        }

        private void load_Zones(bool error =true)
        {
            try
            {
                
                GetZoneListResponse response = new GetZoneListResponse();
                if (!string.IsNullOrEmpty(Program.settingsManager.getSetting("EmailAddress").ToString()) && !string.IsNullOrEmpty(Program.settingsManager.getSetting("APIKey").ToString()))
                    response = Program.cloudFlareAPI.getCloudFlareZones();
                if (response != null)
                {
                    ZoneUpdateList.Invoke(new MethodInvoker(delegate ()
                    {
                        ZoneUpdateList.Items.Clear();
                        foreach (Result rs in response.result)
                        {
                            ListViewItem row = new ListViewItem();
                            row.SubItems.Add(rs.name.ToString());
                            row.SubItems.Add(rs.id.ToString());
                            ZoneUpdateList.Items.Add(row);

                        }
                        if (!string.IsNullOrEmpty(Program.settingsManager.getSetting("SelectedZones").ToString()))
                        {
                            string[] selectedZone = Program.settingsManager.getSetting("SelectedZones").ToString().Split(';');
                            ListView.ListViewItemCollection items;
                            items = ZoneUpdateList.Items;
                            loading_zones = true;
                            foreach (ListViewItem lvt in items)
                            {
                                string subitemtext;
                                subitemtext = lvt.SubItems[2].Text;
                                int pos = Array.IndexOf(selectedZone, subitemtext);

                                if (pos > -1)
                                {
                                    lvt.Checked = true;
                                }
                                else
                                {
                                    lvt.Checked = false;
                                }
                            }
                            loading_zones = false;
                        }
                    }));
                }

            }
            catch (Exception)
            {
                if (error)
                    Logger.log("Failed to load Zones");

                loading_zones = false;
            }
        }
        #region Click buttons

        /// <summary>
        /// Save the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (cloudflare_api_url_input.Text.EndsWith("/"))
                cloudflare_api_url_input.Text = cloudflare_api_url_input.Text.Substring(0, cloudflare_api_url_input.Text.Length - 1);

            string selectedZone = "";
            foreach (ListViewItem ListViewItem in ZoneUpdateList.CheckedItems)
            {
                if (!string.IsNullOrEmpty(ListViewItem.SubItems[2].Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(selectedZone.Trim()))
                        selectedZone += ";";

                    selectedZone += ListViewItem.SubItems[2].Text;
                }
            }

            Program.settingsManager.setSetting("SelectedZones", selectedZone);
            Program.settingsManager.setSetting("EmailAddress", txtEmailAddress.Text);
            Program.settingsManager.setSetting("APIKey", txtAPIKey.Text);
            Program.settingsManager.setSetting("FetchTime", txtFetchTime.Text);
            Program.settingsManager.setSetting("UseEventLog", cbEventLog.Checked.ToString());
            Program.settingsManager.setSetting("IPV6UpdateURL", IPV6UpdateURL.Text);
            Program.settingsManager.setSetting("IPV4UpdateURL", IPV4UpdateURL.Text);
            Program.settingsManager.setSetting("StartMinimized", StartMinimized.Checked.ToString());
            Program.settingsManager.setSetting("APIUrl", cloudflare_api_url_input.Text);
            Program.settingsManager.setSetting("UseInternalIP", UseInternalIP_input.Checked.ToString());
            Program.settingsManager.setSetting("HideSRV", HideSRV_input.Checked.ToString());
            Program.settingsManager.setSetting("EnableNetworkInterfaceChange", enable_network_interface_change.Checked.ToString());

            Program.settingsManager.saveSettings();
        }//end btnApply_Click()

        private void txtAPIKey_TextChanged(object sender, EventArgs e)
        {
            Program.settingsManager.setSetting("APIKey", txtAPIKey.Text);
            Program.settingsManager.saveSettings();
            ZoneUpdateList.Items.Clear();
            Thread t = new Thread(() => load_Zones(false));
            t.Start();
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//end btnClose_Click()

        private void IPV4RESET_Click(object sender, EventArgs e)
        {
            IPV4UpdateURL.Text = "https://api.ipify.org";
        }

        private void IPV6RESET_Click(object sender, EventArgs e)
        {
            IPV6UpdateURL.Text = "https://api6.ipify.org";
        }

        private void cloudflare_api_url_default_button_Click(object sender, EventArgs e)
        {
            cloudflare_api_url_input.Text = "https://api.cloudflare.com/client/v4";
        }

        #endregion Click buttons

        private void ZoneUpdateList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!loading_zones)
            {
                string selectedZone = "";
                foreach (ListViewItem ListViewItem in ZoneUpdateList.Items)
                {

                    if (!string.IsNullOrEmpty(ListViewItem.SubItems[2].Text.Trim()))
                    {
                        if ((ListViewItem.SubItems[2].Text.Trim() == ZoneUpdateList.Items[e.Index].SubItems[2].Text.Trim()))
                        {
                            if (e.NewValue == CheckState.Checked)
                            {
                                if (!string.IsNullOrEmpty(selectedZone.Trim()))
                                    selectedZone += ";";

                                selectedZone += ListViewItem.SubItems[2].Text;
                            }
                        }
                        else
                        {
                            if (ListViewItem.Checked)
                            {
                                if (!string.IsNullOrEmpty(selectedZone.Trim()))
                                    selectedZone += ";";

                                selectedZone += ListViewItem.SubItems[2].Text;
                            }
                        }
                    }
                }

                Program.settingsManager.setSetting("SelectedZones", selectedZone);
                Program.settingsManager.saveSettings();
            }
        }

        private void network_interface_selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = (string)network_interface_selector.SelectedItem;
            Console.WriteLine("New Interface selected: " + value);
            Program.settingsManager.setSetting("DefaultInterface", value);
            Program.settingsManager.saveSettings();
        }

        private void network_interface_selector_reset_btn_Click(object sender, EventArgs e)
        {
            var value = NetworkInterfaceManager.GetDeviceDefaultInterface().Name;
            network_interface_selector.SelectedItem = value;
            Console.WriteLine("New Interface selected: " + value);
            Program.settingsManager.setSetting("DefaultInterface", value);
            Program.settingsManager.saveSettings();
            show_local_ipv4.Text = NetworkInterfaceManager.GetDefaultIP();
            show_local_ipv6.Text = NetworkInterfaceManager.GetDefaultIP(System.Net.Sockets.AddressFamily.InterNetworkV6);
        }

        private void enable_network_interface_change_CheckedChanged(object sender, EventArgs e)
        {
            network_interface_selector.Enabled = enable_network_interface_change.Checked;
            network_interface_selector_reset_btn.Enabled = enable_network_interface_change.Checked;
        }
    }//end class
}//end namespace