﻿// zlib/libpng License
//
// Copyright (c) 2018 Sinoa
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using MyDnsSync.Components;
using MyDnsSync.Properties;

namespace MyDnsSync
{
    /// <summary>
    /// アプリケーションメインクラスです。
    /// アプリケーションの起動、初期化、終了を行います。
    /// </summary>
    internal sealed class ApplicationMain : IDisposable
    {
        // メンバ変数定義
        private Container components;



        /// <summary>
        /// アプリケーションのエントリポイントです
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // アプリケーションメインのインスタンスを生成
            using (var main = new ApplicationMain())
            {
                // アプリケーションを開始する
                main.Run();
            }
        }


        /// <summary>
        /// ApplicationMain のインスタンスを初期化します
        /// </summary>
        private ApplicationMain()
        {
            // ビジュアルスタイルを有効にしてGID+を切る
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // アプリケーションコンフィグをロードする
            ApplicationConfig.Load();


            // ログシステム、コンポーネントの初期化
            InitializeLogSystemConfig();
            InitializeComponents();
        }


        /// <summary>
        /// ログシステムの設定を初期化します
        /// </summary>
        private void InitializeLogSystemConfig()
        {
            // ファイルアペンダを生成して初期化をする
            var fileAppender = new FileAppender();
            fileAppender.Layout = new PatternLayout(Resources.LogTextFormat);
            fileAppender.File = Resources.DefaultOutputLogFilePath;
            fileAppender.Encoding = new UTF8Encoding(false);
            fileAppender.ImmediateFlush = true;
            fileAppender.ActivateOptions();


            // ファイルアペンダで設定を行う
            BasicConfigurator.Configure(fileAppender);
        }


        /// <summary>
        /// アプリケーションで使用するコンポーネントの初期化をします
        /// </summary>
        private void InitializeComponents()
        {
            // コンポーネントを格納するコンテナを生成
            components = new Container();


            // MyDNS同期コンポーネントを生成する
            var myDnsSynchronizer = new MyDnsSynchronizer(components);


            // スケジューラコンポーネントを生成する
            var scheduler = new SyncScheduler(components);


            // メイン通知アイコンを生成して表示する
            var mainNotifyIcon = new MainNotifyIcon(components, myDnsSynchronizer, scheduler);
            mainNotifyIcon.Show();
        }


        /// <summary>
        /// ApplicationMain のリソースを解放します
        /// </summary>
        public void Dispose()
        {
            // 生成したコンポーネントすべて破棄する
            components.Dispose();


            // アプリケーションコンフィグをセーブする
            ApplicationConfig.Save();
        }


        /// <summary>
        /// アプリケーションの動作を開始します
        /// </summary>
        private void Run()
        {
            // Windowsメインループを開始する
            Application.Run();
        }
    }
}