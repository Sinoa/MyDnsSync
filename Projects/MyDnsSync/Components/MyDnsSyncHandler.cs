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
using log4net;
using Microsoft.Win32;

namespace MyDnsSync.Components
{
    /// <summary>
    /// MyDnsSynchronizer を使ったMyDNSへのアドレス通知を行うコンポーネントクラスです
    /// </summary>
    public partial class MyDnsSyncHandler : Component
    {
        // メンバ変数定義
        private ILog logger;
        private MyDnsSynchronizer synchronizer;



        /// <summary>
        /// MyDnsSyncHandler のインスタンスを初期化します
        /// </summary>
        public MyDnsSyncHandler()
        {
            // フォームデザイナが生成した初期化関数を叩いて、共通初期化も呼ぶ
            InitializeComponent();
            InitializeCommon();
        }


        /// <summary>
        /// MyDnsSyncHandler のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを格納するコンテナ</param>
        public MyDnsSyncHandler(IContainer container)
        {
            // コンテナに自身を追加して、フォームデザイナが生成した初期化関数を叩いて、共通初期化も呼ぶ
            container.Add(this);
            InitializeComponent();
            InitializeCommon();
        }


        /// <summary>
        /// 共通の初期化を行います
        /// </summary>
        private void InitializeCommon()
        {
            // ロガーの取得
            logger = LogManager.GetLogger(GetType());
            logger.Debug("MyDNS同期ハンドラを作成しています");


            // MyDNS同期のインスタンスを生成してタイマーをセットアップする
            synchronizer = new MyDnsSynchronizer();
            SetupNextTimerEvent();


            // システムの時刻が修正されたときのイベントを設定する
            SystemEvents.TimeChanged += OnSystemTimeChanged;


            // Disposedイベントを設定する
            Disposed += OnDisposed;
        }


        /// <summary>
        /// コンポーネントが破棄されたときのハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベントの内容</param>
        private void OnDisposed(object sender, EventArgs e)
        {
            // イベントの解除を行う
            SystemEvents.TimeChanged -= OnSystemTimeChanged;
            Disposed -= OnDisposed;
            logger.Debug("MyDNS同期ハンドラが破棄されました");
        }


        /// <summary>
        /// システムの時刻が修正されたときのハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private void OnSystemTimeChanged(object sender, EventArgs e)
        {
            // 次に発生させるタイマーのイベントをセットアップする
            logger.Debug("システムの時刻が変更されました");
            SetupNextTimerEvent();
        }


        /// <summary>
        /// タイマーのチックが発生したときのハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private async void OnTimerTick(object sender, EventArgs e)
        {
            // 認証情報を取得するが、取得ができなかった場合は
            var credential = ApplicationUtility.GetCredential(false);
            if (credential == null)
            {
                // タイマーの再設定だけして終了
                logger.Error("MyDNSログイン情報が取得出来ませんでした");
                SetupNextTimerEvent();
                return;
            }


            // 同期を行って失敗したら
            var result = await synchronizer.DoSynchronizeAsync(credential);
            if (!result)
            {
                // タイマーの再設定をして終了
                logger.Error("MyDNSとの同期に失敗しました");
                SetupNextTimerEvent();
                return;
            }


            // タイマーの再設定をして終了
            logger.Info("MyDNSとの同期に成功しました");
            SetupNextTimerEvent();
        }


        /// <summary>
        /// 次に発生させるべきタイマーのイベントをセットアップします。
        /// また、タイマーは再起動されます。
        /// </summary>
        private void SetupNextTimerEvent()
        {
            // タイマーを停止する
            timer.Stop();


            // 現在の日時から1日加算して時分秒をクリアすることで次に発生させるべきイベントの時間（ミリ秒）を求めて
            // タイマーが次に発生するべきイベントの間隔を設定
            timer.Interval = (int)(DateTime.Now.AddDays(1.0).Date - DateTime.Now).TotalMilliseconds;


            // タイマーを起動する
            timer.Start();
            logger.Info("次回の同期時間が設定されました");
        }
    }
}
