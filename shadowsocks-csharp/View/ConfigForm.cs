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
            cboPlanets.SelectedIndex = 0;

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
            long balance = P2pLib.GetInstance().Balance();
            if (balance <= 0)
            {
                return;
            }
            label4.Text = balance + " Tenon";
            label5.Text = Math.Round(balance * 0.002, 3)  + "$";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Connect_Click(object sender, EventArgs e)
        {
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
                    syncContext.Post(Connect.ThreadRefresh, null);
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
                P2pLib.GetInstance().use_smart_route_ = switch_WOC1.IsOn;
                int country_idx = cboPlanets.SelectedIndex;
                Thread tmp_thread = new Thread(() =>
                {
                    P2pLib.GetInstance().choose_country_ = P2pLib.GetInstance().now_countries_[country_idx];
                    Configuration.RandomNode();
                    controller.ToggleEnable(true);
                    controller.ToggleGlobal(true);
                    P2pLib.GetInstance().connectSuccess = true;
                    P2pLib.GetInstance().connectStarted = false;
                    syncContext.Post(this.Connect.ThreadRefresh, null);
                });
                tmp_thread.IsBackground = true;
                tmp_thread.Start();
            }
        }

        private void switch_WOC1_StateChanged(object sender, EventArgs e)
        {
            P2pLib.GetInstance().use_smart_route_ = switch_WOC1.IsOn;
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
                syncContext.Post(this.Connect.ThreadRefresh, null);
            });
            tmp_thread.IsBackground = true;
            tmp_thread.Start();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void flatButton_WOC1_Click(object sender, EventArgs e)
        {

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
            P2pLib.GetInstance().use_smart_route_ = switch_WOC1.IsOn;

            SynchronizationContext syncContext = SynchronizationContext.Current;
            Thread tmp_thread = new Thread(() =>
            {
                P2pLib.GetInstance().choose_country_ = P2pLib.GetInstance().now_countries_[country_idx];
                Configuration.RandomNode();
                controller.ToggleEnable(true);
                controller.ToggleGlobal(true);
                P2pLib.GetInstance().connectSuccess = true;
                P2pLib.GetInstance().connectStarted = false;
                syncContext.Post(this.Connect.ThreadRefresh, null);
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
    }
}
