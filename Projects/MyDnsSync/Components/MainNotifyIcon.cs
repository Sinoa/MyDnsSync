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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MyDnsSync.Properties;

namespace MyDnsSync.Components
{
    /// <summary>
    /// アプリケーションのメイン通知アイコンクラスです
    /// </summary>
    public partial class MainNotifyIcon : Component
    {
        /// <summary>
        /// アイコンの表示パターンを表します
        /// </summary>
        private enum IconPattern
        {
            /// <summary>
            /// 通常アイコン
            /// </summary>
            Normal,

            /// <summary>
            /// 同期がOFF
            /// </summary>
            SyncOff,

            /// <summary>
            /// 同期エラー
            /// </summary>
            SyncError,
        }



        // メンバ変数定義
        private Dictionary<IconPattern, Icon> iconTable;
        private MyDnsSynchronizer synchronizer;
        private SyncScheduler scheduler;



        /// <summary>
        /// MainNotifyIcon のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを格納するコンテナ</param>
        /// <exception cref="ArgumentNullException">container, synchronizer, scheduler のいずれかが null です</exception>
        public MainNotifyIcon(IContainer container, MyDnsSynchronizer synchronizer, SyncScheduler scheduler)
        {
            // コンテナに自信を追加してフォームデザイナの初期化関数を叩く
            (container ?? throw new ArgumentNullException(nameof(container))).Add(this);
            InitializeComponent();


            // 参照を受け取る
            this.synchronizer = synchronizer ?? throw new ArgumentNullException(nameof(synchronizer));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));


            // アイコンテーブルを初期化する
            iconTable = new Dictionary<IconPattern, Icon>()
            {
                { IconPattern.Normal, Icon.ExtractAssociatedIcon(Application.ExecutablePath) },
                { IconPattern.SyncOff, Resources.SyncOffIcon },
                { IconPattern.SyncError, Resources.SyncErrorIcon },
            };


            // 通知アイコンのアイコンを通常に設定
            notifyIcon.Icon = iconTable[IconPattern.Normal];


            // 各種イベントの登録をする
            exitMenuItem.Click += OnExitMenuItemClick;
            immediateSyncMenuItem.Click += OnImmediateSyncMenuItemClick;
            removeAccountMenuItem.Click += OnRemoveAccountMenuItemClick;
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
        /// 直ちに同期するメニューのクリックハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを起こしたオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private async void OnImmediateSyncMenuItemClick(object sender, EventArgs e)
        {
            // ログインダイアログ表示付きの認証情報取得をする
            var credential = ApplicationUtility.GetCredential(true);


            // もし認証情報の取得ができなかった場合は
            if (credential == null)
            {
                // ログイン情報が無いから同期ができないエラーを表示して終了
                MessageBox.Show("ログイン情報が入力されていないため、同期に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // 同期を直ちに行い失敗したら
            if (!await synchronizer.DoSynchronizeAsync(credential))
            {
                // 同期に失敗したことを表示して終了
                MessageBox.Show("同期に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // 同期が出来たことを表示
            MessageBox.Show("同期に成功しました", "同期完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// アカウントを削除するメニューのクリックハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを起こしたオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private void OnRemoveAccountMenuItemClick(object sender, EventArgs e)
        {
            // 本当に削除しても良いか確認をして、いいえなら
            var message = "MyDNSへのログイン情報を本当に削除しますか？\n再び手動同期を行う際にログインを求められ\n自動同期が行われなくなります。";
            var title = "警告";
            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                // ここでやめる
                return;
            }


            // 認証情報を削除して削除したことを伝える
            ApplicationUtility.RemoveCredential();
            MessageBox.Show("MyDNSへのログイン情報を削除しました", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
