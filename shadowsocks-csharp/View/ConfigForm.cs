using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using howto_ownerdraw_image_and_text;
using Shadowsocks.Controller;
using Shadowsocks.LipP2P;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using Shadowsocks.Util;

namespace Shadowsocks.View
{
    public partial class ConfigForm : Form
    {
        private ShadowsocksController controller;
        private ProxyForm proxyForm;
        private VersionControl upgradeForm;
        private int check_vip_times = 0;

        public ConfigForm(ShadowsocksController controller)
        {
            Font = SystemFonts.MessageBoxFont;
            InitializeComponent();

            // a dirty hack
            PerformLayout();

            Icon = Icon.FromHandle(Resources.ssw128.GetHicon());

            this.controller = controller;
        }

        private SynchronizationContext _syncContext = null;
        private Thread progressThread = null;

        private void ConfigForm_Load_1(object sender, EventArgs e)
        {
            // Make a font for the item text.
            Font font = new Font("Times New Roman", 14);
            Random ran = new Random();
            int n = ran.Next(100, 1000);

            // Make image and text data.
            ImageAndText[] planets =
            {
                new ImageAndText(Resources.us, "America" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.sg, "Singapore" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.br, "Brazil" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.de, "Germany" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.fr, "France" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.kr, "Korea" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.jp, "Japan" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.ca, "Canada" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.au, "Australia" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.hk, "Hong Kong" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.in1, "India" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.gb, "England" + '\n' + ran.Next(100, 1000) + " nodes", font),
                new ImageAndText(Resources.cn, "China" + '\n' + ran.Next(100, 1000) + " nodes", font),
            };

            cboPlanets.DisplayImagesAndText(planets);
            int select_idx = 0;
            for (int i = 0;i < P2pLib.GetInstance().now_countries_.Count; ++i)
            {
                if (P2pLib.GetInstance().now_countries_[i] == P2pLib.GetInstance().choose_country_)
                {
                    select_idx = i;
                    break;
                }
            }
            cboPlanets.SelectedIndex = select_idx;

            this.button2.Text = I18N.GetString("UPGRADE");
            this.button4.Text = I18N.GetString("SETTINGS");
            this.label7.Text = I18N.GetString("Tenon p2p network protecting your ip and privacy.");
            this.label6.Text = I18N.GetString("Balance");
            this.label9.Text = I18N.GetString("");
            this.label10.Text = I18N.GetString("waiting server...");

            label3.Text = P2pLib.GetInstance().account_id_.Substring(0, 8).ToUpper() +
                "..." +
                P2pLib.GetInstance().account_id_.Substring(P2pLib.GetInstance().account_id_.Length - 8, 8).ToUpper();
            label4.Text = "-- Tenon";
            label5.Text = "--$";

            _syncContext = SynchronizationContext.Current;
            progressThread = new Thread(() =>
            {
                while (true)
                {
                    _syncContext.Post(ResetBalance, null);
                    Thread.Sleep(1000);
                }
            });
            progressThread.IsBackground = true;
            progressThread.Start();
        }

        public void ResetBalance(object obj)
        {
            if (!P2pLib.GetInstance().server_status.Equals("ok"))
            {
                if (P2pLib.GetInstance().server_status.Equals("cni"))
                {
                    this.label8.Text = I18N.GetString("Agent service is not supported in your country or region.");
                }

                if (P2pLib.GetInstance().server_status.Equals("cnn"))
                {
                    this.label8.Text = I18N.GetString("Connect p2p vpn server failed.");
                }

                if (P2pLib.GetInstance().server_status.Equals("bwo"))
                {
                    this.label8.Text = I18N.GetString("free 100m used up, buy tenon or use tomorrow.");
                }

                if (P2pLib.GetInstance().server_status.Equals("oul"))
                {
                    this.label8.Text = I18N.GetString("Your account is logged in elsewhere.");
                }
                SynchronizationContext syncContext = SynchronizationContext.Current;
                if (P2pLib.GetInstance().connectSuccess)
                {
                    if (!P2pLib.GetInstance().disConnectStarted)
                    {
                        P2pLib.GetInstance().disConnectStarted = true;

                        Thread tmp_thread = new Thread(() =>
                        {
                            controller.Stop();
                            controller.ToggleEnable(false);
                            P2pLib.GetInstance().connectSuccess = false;
                            P2pLib.GetInstance().connectStarted = false;
                            syncContext.Post(ConnectButton.ThreadRefresh, null);
                            P2pLib.GetInstance().disConnectStarted = false;
                        });
                        tmp_thread.IsBackground = true;
                        tmp_thread.Start();
                    }
                }
            }

            long balance = P2pLib.GetInstance().Balance();
            if (balance >= 0)
            {
                P2pLib.GetInstance().now_balance = balance;
                label4.Text = balance + " Tenon";
                label5.Text = Math.Round(balance * 0.002, 3) + "$";
                this.label10.Text = balance + " Tenon";
            }

            if (check_vip_times < 10)
            {
                long tm = P2pLib.GetInstance().CheckVIP();
                if (P2pLib.GetInstance().payfor_timestamp == 0 || tm != long.MaxValue)
                {
                    if (tm != long.MaxValue && tm != 0)
                    {
                        check_vip_times = 11;
                    }
                    P2pLib.GetInstance().payfor_timestamp = tm;
                }
                ++check_vip_times;
            }
            else
            {
                P2pLib.GetInstance().PayforVpn();
            }

            if (P2pLib.GetInstance().vip_left_days == -1 &&
                    P2pLib.GetInstance().now_balance != -1 &&
                    P2pLib.GetInstance().payfor_timestamp == long.MaxValue)
            {
                this.label9.Text = I18N.GetString("Free 100M/DAY.");
            }

            if (P2pLib.GetInstance().vip_left_days >= 0)
            {
                this.label9.Text = I18N.GetString("Due in ") + P2pLib.GetInstance().vip_left_days + I18N.GetString("days");
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            this.label8.Text = I18N.GetString("");
            P2pLib.GetInstance().server_status = "ok";
            SynchronizationContext syncContext = SynchronizationContext.Current;
            if (P2pLib.GetInstance().connectSuccess)
            {
                if (P2pLib.GetInstance().disConnectStarted)
                {
                    return;
                }
                P2pLib.GetInstance().disConnectStarted = true;

                Thread tmp_thread = new Thread(() =>
                {
                    controller.Stop();
                    controller.ToggleEnable(false);
                    P2pLib.GetInstance().connectSuccess = false;
                    P2pLib.GetInstance().connectStarted = false;
                    syncContext.Post(ConnectButton.ThreadRefresh, null);
                    P2pLib.GetInstance().disConnectStarted = false;
                });
                tmp_thread.IsBackground = true;
                tmp_thread.Start();
            }
            else
            {
                if (P2pLib.GetInstance().connectStarted)
                {
                    return;
                }

                P2pLib.GetInstance().connectStarted = true;
                P2pLib.GetInstance().use_smart_route_ = this.switch_WOC1.IsOn;
                int country_idx = cboPlanets.SelectedIndex;
                Thread tmp_thread = new Thread(() =>
                {
                    P2pLib.GetInstance().choose_country_ = P2pLib.GetInstance().now_countries_[country_idx];
                    Configuration.RandomNode();
                    controller.ToggleEnable(true);
                    controller.ToggleGlobal(true);
                    P2pLib.GetInstance().connectSuccess = true;
                    P2pLib.GetInstance().connectStarted = false;
                    syncContext.Post(this.ConnectButton.ThreadRefresh, null);
                });
                tmp_thread.IsBackground = true;
                tmp_thread.Start();
            }
        }

        private void switch_WOC1_StateChanged(object sender, EventArgs e)
        {
            P2pLib.GetInstance().use_smart_route_ = this.switch_WOC1.IsOn;
            if (!P2pLib.GetInstance().connectSuccess)
            {
                return;
            }

            if (P2pLib.GetInstance().connectStarted || P2pLib.GetInstance().disConnectStarted)
            {
                return;
            }

            P2pLib.GetInstance().connectSuccess = false;
            P2pLib.GetInstance().connectStarted = true;
            int country_idx = cboPlanets.SelectedIndex;
            SynchronizationContext syncContext = SynchronizationContext.Current;
            Thread tmp_thread = new Thread(() =>
            {
                P2pLib.GetInstance().choose_country_ = P2pLib.GetInstance().now_countries_[country_idx];
                Configuration.RandomNode();
                controller.ToggleEnable(true);
                controller.ToggleGlobal(true);
                P2pLib.GetInstance().connectSuccess = true;
                P2pLib.GetInstance().connectStarted = false;
                syncContext.Post(this.ConnectButton.ThreadRefresh, null);
            });
            tmp_thread.IsBackground = true;
            tmp_thread.Start();

        }

        private void cboPlanets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (P2pLib.GetInstance().connectStarted || P2pLib.GetInstance().disConnectStarted)
            {
                return;
            }

            int country_idx = cboPlanets.SelectedIndex;
            if (P2pLib.GetInstance().choose_country_ == P2pLib.GetInstance().now_countries_[country_idx])
            {
                return;
            }

            P2pLib.GetInstance().connectSuccess = false;
            P2pLib.GetInstance().connectStarted = true;
            P2pLib.GetInstance().use_smart_route_ = this.switch_WOC1.IsOn;

            SynchronizationContext syncContext = SynchronizationContext.Current;
            Thread tmp_thread = new Thread(() =>
            {
                P2pLib.GetInstance().choose_country_ = P2pLib.GetInstance().now_countries_[country_idx];
                Configuration.RandomNode();
                controller.ToggleEnable(true);
                controller.ToggleGlobal(true);
                P2pLib.GetInstance().connectSuccess = true;
                P2pLib.GetInstance().connectStarted = false;
                syncContext.Post(this.ConnectButton.ThreadRefresh, null);
            });
            tmp_thread.IsBackground = true;
            tmp_thread.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (proxyForm != null)
            {
                proxyForm.Activate();
            }
            else
            {
                proxyForm = new ProxyForm(controller);
                proxyForm.Show();
                proxyForm.Activate();
                proxyForm.FormClosed += tmp_proxyForm_FormClosed;
            }
        }

        void tmp_proxyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            proxyForm.Dispose();
            proxyForm = null;
            Utils.ReleaseMemory(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ver = P2pLib.GetInstance().GetLatestVer();
            if (ver.IsNullOrEmpty())
            {
                MessageBox.Show(I18N.GetString("Already the latest version."));
            }
            else
            {
                bool has_windows = false;
                string[] ver_split = ver.Split(',');
                for (int i = 0; i < ver_split.Length; ++i)
                {
                    string[] item = ver_split[i].Split(';');
                    if (item.Length < 3)
                    {
                        continue;
                    }

                    if (item[0].Equals("windows"))
                    {
                        if (String.Compare(item[1], P2pLib.kCurrentVersion) <= 0)
                        {
                            MessageBox.Show(I18N.GetString("Already the latest version."));
                            return;
                        }
                        has_windows = true;
                        break;
                    }
                }

                if (!has_windows)
                {
                    MessageBox.Show(I18N.GetString("Already the latest version."));
                    return;
                }

                if (upgradeForm != null)
                {
                    upgradeForm.Activate();
                }
                else
                {
                    upgradeForm = new VersionControl(ver);
                    upgradeForm.Show();
                    upgradeForm.Activate();
                    upgradeForm.FormClosed += tmp_upgradeForm_FormClosed;
                }
            }
        }

        void tmp_upgradeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            upgradeForm.Dispose();
            upgradeForm = null;
            Utils.ReleaseMemory(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (proxyForm != null)
            {
                proxyForm.Activate();
            }
            else
            {
                proxyForm = new ProxyForm(controller);
                proxyForm.Show();
                proxyForm.Activate();
                proxyForm.FormClosed += tmp_proxyForm_FormClosed;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // goto brower
            System.Diagnostics.Process.Start("http://39.105.125.37:7744/chongzhi/" + P2pLib.GetInstance().account_id_);
        }
    }
}
