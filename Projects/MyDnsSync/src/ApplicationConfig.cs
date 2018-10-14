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
using System.IO;
using System.Xml.Serialization;

namespace MyDnsSync
{
    /// <summary>
    /// アプリケーションの設定を管理、制御を行うクラスです
    /// </summary>
    internal static class ApplicationConfig
    {
        // クラス変数宣言
        private static FileInfo configFileInfo;



        /// <summary>
        /// アプリケーションの汎用的な設定を保持しています
        /// </summary>
        public static GeneralConfig General { get; private set; }



        /// <summary>
        /// ApplicationConfig のクラスを初期化します
        /// </summary>
        static ApplicationConfig()
        {
            // 各種設定の初期生成を行う
            General = new GeneralConfig();


            // 設定を保存するディレクトリパスを用意して、アプリケーションファイルへのファイル情報インスタンスを生成する
            var configDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyDnsSync");
            configFileInfo = new FileInfo(Path.Combine(configDirectoryPath, "appconfig.xml"));
        }


        /// <summary>
        /// アプリケーションの設定を読み込みます
        /// </summary>
        public static void Load()
        {
            // ファイル情報を更新して、ファイルが存在を確認できないなら
            configFileInfo.Refresh();
            if (!configFileInfo.Exists)
            {
                // ロード自体出来ないので終了
                return;
            }


            // XMLシリアライザを生成してファイルを開く
            var serializer = new XmlSerializer(typeof(GeneralConfig));
            using (var fileStream = File.OpenRead(configFileInfo.FullName))
            {
                // 設定ファイルからデシリアライズをする
                General = (GeneralConfig)serializer.Deserialize(fileStream);
            }


            // 設定の修正も念の為行う
            General.ValidationAndRepair();
        }


        /// <summary>
        /// アプリケーションの設定を保存します
        /// </summary>
        public static void Save()
        {
            // ファイル情報を更新して、ディレクトリがない場合は
            configFileInfo.Refresh();
            if (!configFileInfo.Directory.Exists)
            {
                // ディレクトリを作る
                configFileInfo.Directory.Create();
            }


            // XMLシリアライザを生成してファイルを開く
            var serializer = new XmlSerializer(typeof(GeneralConfig));
            using (var fileStream = File.OpenWrite(configFileInfo.FullName))
            {
                // 設定ファイルに設定をシリアライズをする
                serializer.Serialize(fileStream, General);
            }
        }
    }



    /// <summary>
    /// 汎用的なアプリケーション情報を保持した設定クラスです
    /// </summary>
    internal class GeneralConfig
    {
        // メンバ変数定義
        private bool enableAutoSync;



        /// <summary>
        /// 自動同期が有効かどうか
        /// </summary>
        public bool EnableAutoSync
        {
            get { return enableAutoSync; }
            set { enableAutoSync = value; ValidationAndRepair(); }
        }



        /// <summary>
        /// 各種設定の値が正常かどうか確認し、問題があれば訂正します
        /// </summary>
        public void ValidationAndRepair()
        {
        }
    }
}