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
using System.Drawing;
using System.Windows.Forms;

namespace MyDnsSync.Components
{
    /// <summary>
    /// アプリケーションのメイン通知アイコンクラスです
    /// </summary>
    public partial class MainNotifyIcon : Component
    {
        /// <summary>
        /// MainNotifyIcon のインスタンスを初期化します
        /// </summary>
        public MainNotifyIcon()
        {
            // フォームデザイナが生成した初期化関数を叩く
            InitializeComponent();


            // 共通の初期化関数を叩く
            InitializeCommon();
        }


        /// <summary>
        /// MainNotifyIcon のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを格納するコンテナ</param>
        public MainNotifyIcon(IContainer container)
        {
            // コンテナに自信を追加してフォームデザイナの初期化関数を叩く
            container.Add(this);
            InitializeComponent();


            // 共通の初期化関数を叩く
            InitializeCommon();
        }


        /// <summary>
        /// 共通のインスタンス初期化を行います
        /// </summary>
        private void InitializeCommon()
        {
            // 通知アイコンのアイコンを雑に設定
            notifyIcon.Icon = SystemIcons.Application;


            // 各種イベントの登録をする
            exitMenuItem.Click += OnExitMenuItemClick;
        }


        /// <summary>
        /// 終了メニューのクリックハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを起こしたオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private void OnExitMenuItemClick(object sender, EventArgs e)
        {
            // アプリケーションの終了を呼ぶ
            Application.Exit();
        }


        /// <summary>
        /// 通知アイコンを表示します
        /// </summary>
        public void Show()
        {
            // 通知アイコンを表示する
            notifyIcon.Visible = true;
        }
    }
}
