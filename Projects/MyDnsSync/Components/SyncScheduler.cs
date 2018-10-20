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

namespace MyDnsSync.Components
{
    /// <summary>
    /// 特定のタイミングで同期を行うための、日時のスケジューリングを行うコンポーネントクラスです
    /// </summary>
    public partial class SyncScheduler : Component
    {
        // メンバ変数定義
        private List<DateTime> scheduleList;



        /// <summary>
        /// SyncScheduler のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを保持するコンテナ</param>
        /// <exception cref="ArgumentNullException">container が null です</exception>
        public SyncScheduler(IContainer container)
        {
            // null を渡されたら
            if (container == null)
            {
                // 管理をして下さい
                throw new ArgumentNullException(nameof(container));
            }


            // コンテナに自身を追加して、フォームデザイナの初期化関数を叩く
            container.Add(this);
            InitializeComponent();


            // スケジュールリストを生成
            scheduleList = new List<DateTime>();
        }


        /// <summary>
        /// 監視タイマーのチックのハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private void OnWatchTimerTick(object sender, EventArgs e)
        {
            // もしスケジュールリストが空なら
            if (scheduleList.Count == 0)
            {
                // 監視タイマーを停止して終了
                watchTimer.Stop();
                return;
            }
        }


        /// <summary>
        /// 指定された日付をスケジュールします
        /// </summary>
        /// <param name="eventDateTime">スケジュールする日付</param>
        public void Schedule(DateTime eventDateTime)
        {
            // リストに追加してソートする
            scheduleList.Add(eventDateTime);
            scheduleList.Sort();


            // 監視タイマーを起こす
            watchTimer.Start();
        }
    }
}
