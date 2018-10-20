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
        /// <summary>
        /// スケジュール内容を保持した構造体です
        /// </summary>
        private struct ScheduleContent
        {
            // メンバ変数定義
            private Action<object> callback;
            private object state;



            /// <summary>
            /// スケジュールされた日時
            /// </summary>
            public DateTime ScheduleDateTime { get; private set; }



            /// <summary>
            /// ScheduleContent インスタンスを初期化します
            /// </summary>
            /// <param name="scheduleDateTime">callback を呼ぶ日時</param>
            /// <param name="callback">スケジュールされた日時が来た時に呼び出すコールバック関数</param>
            /// <param name="state">コールバック関数に渡す状態変数</param>
            public ScheduleContent(DateTime scheduleDateTime, Action<object> callback, object state)
            {
                // 受け取る
                this.callback = callback;
                this.state = state;
                ScheduleDateTime = scheduleDateTime;
            }


            /// <summary>
            /// コーバックを呼びます
            /// </summary>
            public void Invoke()
            {
                // コールバックを呼ぶ
                callback(state);
            }
        }



        // メンバ変数定義
        private List<ScheduleContent> scheduleList;



        /// <summary>
        /// SyncScheduler のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを保持するコンテナ</param>
        /// <exception cref="ArgumentNullException">container が null です</exception>
        public SyncScheduler(IContainer container)
        {
            // コンテナに自身を追加して、フォームデザイナの初期化関数を叩く
            (container ?? throw new ArgumentNullException(nameof(container))).Add(this);
            InitializeComponent();


            // スケジュールリストを生成
            scheduleList = new List<ScheduleContent>();
        }


        /// <summary>
        /// 監視タイマーのチックのハンドリングを行います
        /// </summary>
        /// <param name="sender">イベントを送信したオブジェクト</param>
        /// <param name="e">イベント内容</param>
        private void OnWatchTimerTick(object sender, EventArgs e)
        {
            // スケジュールリストの末尾から回る
            for (int i = scheduleList.Count - 1; i >= 0; --i)
            {
                // もし実行すべき日時を迎えていないなら
                if (scheduleList[i].ScheduleDateTime > DateTime.Now)
                {
                    // ループから抜ける
                    break;
                }


                // イベントを起こして削除する
                scheduleList[i].Invoke();
                scheduleList.RemoveAt(i);
            }


            // もしスケジュールリストが空なら
            if (scheduleList.Count == 0)
            {
                // 監視タイマーを停止して終了
                watchTimer.Stop();
                return;
            }
        }


        /// <summary>
        /// 指定されたコールバックを実行する為の、日付をスケジュールします
        /// </summary>
        /// <param name="eventDateTime">スケジュールする日付</param>
        /// <param name="callback">実行するコールバック</param>
        /// <param name="state">コールバックに渡す状態変数</param>
        public void Schedule(DateTime eventDateTime, Action<object> callback, object state)
        {
            // リストに追加して日付の降順でソートする
            scheduleList.Add(new ScheduleContent(eventDateTime, callback, state));
            scheduleList.Sort((a, b) => DateTime.Compare(a.ScheduleDateTime, b.ScheduleDateTime) * -1);


            // 監視タイマーを起こす
            watchTimer.Start();
        }
    }
}
