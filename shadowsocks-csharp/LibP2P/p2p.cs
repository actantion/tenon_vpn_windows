using System;
using System.Runtime.InteropServices;
using Shadowsocks.Util;
using Shadowsocks.Controller;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;

namespace Shadowsocks.LipP2P {

    public class P2pLib {
        [DllImport("dll.dll", EntryPoint = "init_network", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr initNetwork(
                IntPtr local_ip,
                UInt16 local_port,
                IntPtr bootstrap,
                IntPtr conf_path,
                IntPtr log_path,
                IntPtr log_conf_path);
        [DllImport("dll.dll", EntryPoint = "get_socket", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getSocket();
        [DllImport("dll.dll", EntryPoint = "create_account", CallingConvention = CallingConvention.Cdecl)]
        private static extern void createAccount();
        [DllImport("dll.dll", EntryPoint = "get_vpn_nodes", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getNetworkNodes(IntPtr country, uint count, bool route);
        [DllImport("dll.dll", EntryPoint = "get_public_key", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getPublicKey();
        [DllImport("dll.dll", EntryPoint = "get_balance", CallingConvention = CallingConvention.Cdecl)]
        private static extern long getBalance();
        [DllImport("dll.dll", EntryPoint = "transactions", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Transactions(uint begin, uint len);
        [DllImport("dll.dll", EntryPoint = "check_version", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr checkVersion();

        private static P2pLib uniqueInstance;
        public string prikey_;
        public string pubkey_;
        public string account_id_;
        public string local_country_;
        public string choose_country_ = "US";
        public string str_vpn_ip_;
        public uint vpn_ip_ = 0;
        public ushort vpn_port_ = 0;
        public string seckey_;
        public bool use_smart_route_ = true;
        public string str_route_ip_;
        public uint route_ip_ = 0;
        public ushort route_port_ = 0;
        public int socket_id_;
        public string enc_method_ = "aes-128-cfb";

        public bool connectStarted = false;
        public bool connectSuccess = true;
        public bool disConnectStarted = false;

        private string old_vpn_ip = "";

        public List<string> now_countries_ = new List<string>() {
            "US", "SG", "BR","DE","FR","KR", "JP", "CA","AU","HK", "IN", "GB","CN"
        };

        public List<string> def_vpn_country = new List<string>() {
            "US", "SG", "IN", "GB"
        };

        private static readonly object locker = new object();

        private P2pLib() {}

        public long Balance()
        {
            return getBalance();
        }

        public static P2pLib GetInstance() {
            if (uniqueInstance == null) {
                lock (locker) {
                    if (uniqueInstance == null) {
                        uniqueInstance = new P2pLib();
                    }
                }
            }
            return uniqueInstance;
        }

        string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        public int InitNetwork() {
            string path = Utils.GetTempPath();
            IntPtr ip = Marshal.StringToHGlobalAnsi("0.0.0.0");
            IntPtr bootstarp = Marshal.StringToHGlobalAnsi("id_1:120.77.2.117:9001,id:47.105.87.61:9001,id:110.34.181.120:9001,id:98.126.31.159:9001");
            IntPtr conf = Marshal.StringToHGlobalAnsi(path + "\\lego.conf");
            IntPtr log = Marshal.StringToHGlobalAnsi(path + "\\lego.log");
            IntPtr log_conf = Marshal.StringToHGlobalAnsi(path + "\\log4cpp.properties");

            IntPtr ptrRet = initNetwork(
                    ip,
                    18993,
                    bootstarp,
                    conf,
                    log,
                    log_conf);
            string str = Marshal.PtrToStringAnsi(ptrRet);
            if (str.Equals("ERROR"))
            {
                Logging.Error("init network failed!");
                return 1;
            }

            string[] res_arr = str.Split(',');
            if (res_arr.Length != 3) {
                Logging.Error("init network failed, res data invalid!");
                return 1;
            }

            local_country_ = res_arr[0];
            account_id_ = res_arr[1];
            prikey_ = res_arr[2];
            socket_id_ = getSocket();
            IntPtr pubkeyRet = getPublicKey();
            pubkey_ = Marshal.PtrToStringAnsi(pubkeyRet);
            Logging.Debug($"local_country_: {local_country_}");
            Logging.Debug($"account_id_: {account_id_}");
            Logging.Debug($"prikey_: {prikey_}");
            Logging.Debug($"socket_id_: {socket_id_}");
            createAccount();
            return 0;
        }

        public int GetRemoteNode(ref string ip, ref ushort port) {
            if (use_smart_route_) {
                return GetOneRouteNode(ref ip, ref port);
            }
            ip = str_vpn_ip_;
            port = vpn_port_;
            return 0;
        }

        public int GetOneRouteNode(ref string ip, ref ushort port) {
            int res = GetOneRouteNode(local_country_, ref ip, ref port);
            if (res == 0) {
                return 0;
            }

            foreach (string country in def_vpn_country) {
                res = GetOneRouteNode(country, ref ip, ref port);
                if (res == 0) {
                    return 0;
                }
            }
            return 1;
        }

        public int ChooseOneVpnNode() {
            string ip = "";
            ushort port = 0;
            string passwd = "";
            int res = GetOneVpnNode(choose_country_, ref ip, ref port, ref passwd);
            if (res == 0) {
                for (int i = 0; i < 5; ++i) {
                    if (old_vpn_ip.Equals(ip)) {
                        break;
                    }

                    res = GetOneVpnNode(choose_country_, ref ip, ref port, ref passwd);
                }
                old_vpn_ip = ip;
                return res;
            }

            foreach (string country in def_vpn_country) {
                res = GetOneVpnNode(country, ref ip, ref port, ref passwd);
                if (res == 0) {
                    for (int i = 0; i < 5; ++i) {
                        if (old_vpn_ip.Equals(ip)) {
                            break;
                        }

                        res = GetOneVpnNode(choose_country_, ref ip, ref port, ref passwd);
                    }
                    old_vpn_ip = ip;
                    return 0;
                }
            }

            return 1;
        }

        public int GetOneRouteNode(string country, ref string ip, ref ushort port) {
            IntPtr tmp_country = Marshal.StringToHGlobalAnsi(country);
            IntPtr ptrRet = getNetworkNodes(tmp_country, 16, true);
            string str = Marshal.PtrToStringAnsi(ptrRet);
            if (str.Equals("ERROR")) {
                Logging.Error("get route network nodes failed!");
                return 1;
            }

            if (str.IsNullOrEmpty()) {
                Logging.Error("get route network nodes failed!");
                return 1;
            }

            if (str.Equals("ERROR")) {
                Logging.Error("get route network nodes failed!");
                return 1;
            }

            string[] node_arr = str.Split(',');
            if (node_arr.Length <= 0) {
                Logging.Error("get route network nodes failed!");
                return 1;
            }

            Random ran = new Random();
            int n = ran.Next(node_arr.Length);
            string[] node_info = (node_arr[n]).Split(':');
            if (node_info.Length < 5) {
                Logging.Error("get route network nodes failed!");
                return 1;
            }

            str_route_ip_ = node_info[0];
            route_ip_ = IpToInt(node_info[0]);
            route_port_ = ushort.Parse(node_info[2]);
            ip = node_info[0];
            port = route_port_;
            Logging.Debug($"route_ip_: {node_info[0]}, {route_ip_}");
            Logging.Debug($"route_port_: {route_port_}");
            return 0;
        }

        public int GetOneVpnNode(string country, ref string ip, ref ushort port, ref string passwd) {
            IntPtr tmp_country = Marshal.StringToHGlobalAnsi(country);
            IntPtr ptrRet = getNetworkNodes(tmp_country, 16, false);
            string str = Marshal.PtrToStringAnsi(ptrRet);
            if (str.IsNullOrEmpty()) {
                Logging.Error("get vpn network nodes failed! res empty.");
                return 1;
            }

            if (str.Equals("ERROR")) {
                Logging.Error("get vpn network nodes failed! res error.");
                return 1;
            }

            string[] node_arr = str.Split(',');
            if (node_arr.Length <= 0) {
                Logging.Error("get vpn network nodes failed! node empty.");
                return 1;
            }

            Random ran = new Random();
            int n = ran.Next(node_arr.Length);
            string[] node_info = (node_arr[n]).Split(':');
            if (node_info.Length < 5) {
                Logging.Error($"get vpn network nodes failed! node info error.{node_arr[n]}, {str}");
                return 1;
            }

            str_vpn_ip_ = node_info[0];
            vpn_ip_ = IpToInt(node_info[0]);
            vpn_port_ = ushort.Parse(node_info[1]);
            seckey_ = node_info[3];
            ip = node_info[0];
            port = vpn_port_;
            passwd = seckey_;
            Logging.Debug($"vpn_ip_: {node_info[0]}, {vpn_ip_}");
            Logging.Debug($"vpn_port_: {vpn_port_}");
            Logging.Debug($"seckey_: {seckey_}");
            return 0;
        }

        public static uint IpToInt(string ip) {
            string[] items = ip.Split('.');
            return uint.Parse(items[3]) << 24
                    | uint.Parse(items[2]) << 16
                    | uint.Parse(items[1]) << 8
                    | uint.Parse(items[0]);
        }

        public string Trans()
        {
            IntPtr ptrRet = Transactions(0, 64);
            return Marshal.PtrToStringAnsi(ptrRet);
        }

        public string GetLatestVer()
        {
            IntPtr ptrRet = checkVersion();
            return Marshal.PtrToStringAnsi(ptrRet);
        }
    }
}
