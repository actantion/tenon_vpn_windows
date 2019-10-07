using Shadowsocks.Properties;
using WinFormAnimation;

namespace Shadowsocks.View
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPlanets = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.switch_WOC1 = new LimitlessUI.Switch_WOC();
            this.Connect = new LimitlessUI.Button_WOC();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Location = new System.Drawing.Point(102, 234);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 0);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(206)))), ((int)(((byte)(159)))));
            this.label1.Location = new System.Drawing.Point(44, 503);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "Account";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 549);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "F65AD56...F3DA78F";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 549);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "10000 Tenon";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(319, 549);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "200.00$";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(206)))), ((int)(((byte)(159)))));
            this.label2.Location = new System.Drawing.Point(248, 503);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Balance";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cboPlanets
            // 
            this.cboPlanets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPlanets.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboPlanets.DropDownHeight = 400;
            this.cboPlanets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlanets.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlanets.ForeColor = System.Drawing.SystemColors.Window;
            this.cboPlanets.FormattingEnabled = true;
            this.cboPlanets.IntegralHeight = false;
            this.cboPlanets.Location = new System.Drawing.Point(19, 52);
            this.cboPlanets.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboPlanets.Name = "cboPlanets";
            this.cboPlanets.Size = new System.Drawing.Size(370, 66);
            this.cboPlanets.TabIndex = 18;
            this.cboPlanets.SelectedIndexChanged += new System.EventHandler(this.cboPlanets_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(222, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 19);
            this.label6.TabIndex = 22;
            this.label6.Text = "Smart Route";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::Shadowsocks.Properties.Resources.user_set;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(19, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 23;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // switch_WOC1
            // 
            this.switch_WOC1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.switch_WOC1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.switch_WOC1.ForeColor = System.Drawing.Color.Black;
            this.switch_WOC1.IsOn = false;
            this.switch_WOC1.Location = new System.Drawing.Point(330, 8);
            this.switch_WOC1.Name = "switch_WOC1";
            this.switch_WOC1.OffColor = System.Drawing.Color.Gainsboro;
            this.switch_WOC1.OffText = "OF";
            this.switch_WOC1.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(184)))), ((int)(((byte)(170)))));
            this.switch_WOC1.OnText = "On";
            this.switch_WOC1.Size = new System.Drawing.Size(59, 20);
            this.switch_WOC1.TabIndex = 21;
            this.switch_WOC1.TextEnabled = true;
            this.switch_WOC1.StateChanged += new System.EventHandler(this.switch_WOC1_StateChanged);
            // 
            // Connect
            // 
            this.Connect.Angle = 360;
            this.Connect.BackLineThikness = 5F;
            this.Connect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(254)))), ((int)(((byte)(238)))));
            this.Connect.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(204)))), ((int)(((byte)(190)))));
            this.Connect.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.Connect.Font1 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Connect.Font2 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Connect.Font3 = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Connect.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Connect.IgnoreHeight = true;
            this.Connect.Location = new System.Drawing.Point(86, 151);
            this.Connect.Name = "Connect";
            this.Connect.Offset = new System.Drawing.Point(0, 0);
            this.Connect.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(224)))), ((int)(((byte)(208)))));
            this.Connect.OnHoverButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(184)))), ((int)(((byte)(170)))));
            this.Connect.OnHoverTextColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Connect.ProgressBackColor = System.Drawing.Color.Silver;
            this.Connect.ProgressColor = System.Drawing.Color.Lime;
            this.Connect.ProgressLineThikness = 9F;
            this.Connect.Size = new System.Drawing.Size(240, 240);
            this.Connect.Style = LimitlessUI.Button_WOC.StyleEnum.Style3;
            this.Connect.TabIndex = 19;
            this.Connect.Text = "Connect";
            this.Connect.Text1Color = System.Drawing.SystemColors.ControlText;
            this.Connect.Text2Color = System.Drawing.SystemColors.ControlText;
            this.Connect.Text3Color = System.Drawing.SystemColors.ControlText;
            this.Connect.TextColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Connect.Value = 290F;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(406, 639);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.switch_WOC1);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.cboPlanets);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TenonVPN";
            this.Load += new System.EventHandler(this.ConfigForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPlanets;
        private LimitlessUI.Button_WOC Connect;
        private LimitlessUI.Switch_WOC switch_WOC1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
    }
}

