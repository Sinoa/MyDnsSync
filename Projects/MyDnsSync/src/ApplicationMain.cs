// zlib/libpng License
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
using System.Windows.Forms;
using log4net;
using MyDnsSync.Components;

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


            // ログシステムの初期化
            ApplicationUtility.InitializeLogSystemConfig();


            // コンポーネントを格納するコンテナを生成
            components = new Container();


            // メイン通知アイコンを生成して表示する
            var mainNotifyIcon = new MainNotifyIcon(components);
            mainNotifyIcon.Show();


            // MyDNS同期コンポーネントを生成する
            new MyDnsSyncHandler(components);
        }


        /// <summary>
        /// ApplicationMain のリソースを解放します
        /// </summary>
        public void Dispose()
        {
            // 生成したコンポーネントすべて破棄する
            components.Dispose();
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