﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shortcuts
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        
        delegate void ClipboardCallback(string s);

        static Shortcuts shortcuts;
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            //first read all shortcuts from the user
            ConfigKeysReader reader = new ConfigKeysReader();// "config.json");
            shortcuts = reader.Entries;
            //register shortcuts from config
            ShortcutsUtils.RegisterHotKeys(shortcuts);
            //listen to an event
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);

            // Hide the console window
            ShowWindow(handle, SW_HIDE);
            
            // cause the console to wait and not end
            Console.ReadLine();
        }

        static void SetClipboard(string text, ClipboardCallback callback)
        {
            Thread staThread = new Thread(x =>
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch (Exception ex)
                {
                }
                callback("");
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }
        static string GetClipboard(ClipboardCallback callback)
        {
            string res = "";
            Thread staThread = new Thread(x =>
            {
                try
                {
                    res = Clipboard.GetText();
                }
                catch (Exception ex)
                {
                    res = "";
                }
                callback(res);
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            return res;
        }
        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            EvalKeyPressed(e.Key);
        }
        static void EvalKeyPressed(Keys key)
        {
            //map between key and shortcust
            foreach (var s in shortcuts.Keys)
            {
                if(s.Key==key.ToString())
                {
                    SendText(s.Value);
                    break;
                }
            }
        }
        private static void SendText(string val)
        {
            GetClipboard((c)=>
            {
                SetClipboard(val, M =>
                {
                    Thread.Sleep(30);
                    SendKeys.SendWait("^v");
                    //reset clipboard
                    SetClipboard(c,W=>{ });
                });
            });
        }
    }
}
