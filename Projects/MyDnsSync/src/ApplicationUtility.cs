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

using System.Net;
using AdysTech.CredentialManager;
using MyDnsSync.Properties;

namespace MyDnsSync
{
    /// <summary>
    /// アプリケーションで使用する便利な共通関数がまとめられたクラスです
    /// </summary>
    internal static class ApplicationUtility
    {
        /// <summary>
        /// アプリケーションが利用する資格情報を取得します
        /// </summary>
        /// <param name="showPromptIfNotAvailable">資格情報が取得できない時、ユーザーに資格情報の入力をしてもらうときは true</param>
        /// <returns>資格情報の取得ができた場合は、資格情報のインスタンスを返しますが、取得する資格情報がない場合は null を返します</returns>
        public static NetworkCredential GetCredential(bool showPromptIfNotAvailable)
        {
            // 資格情報を取得して、資格情報が見つからない かつ ユーザー入力を求めるなら
            var credential = CredentialManager.GetCredentials(Resources.CredentialTargetName);
            if (credential == null && showPromptIfNotAvailable)
            {
                // 資格情報入力ダイアログを出して資格情報を受け取る
                credential = ShowCredentialPrompt();
            }


            // 結果を返す
            return credential;
        }


        /// <summary>
        /// 資格情報の入力をするためのダイアログを表示します
        /// </summary>
        /// <returns>入力された資格情報を返しますが、入力がキャンセルされた場合は null を返します</returns>
        public static NetworkCredential ShowCredentialPrompt()
        {
            // ダイアログに出すための情報の取得と各種処理のパラメータを準備
            var targetName = Resources.CredentialTargetName;
            var titleText = Resources.CredentialDialogTitle;
            var messageText = Resources.CredentialDialogMessage;
            var credential = CredentialManager.GetCredentials(targetName);
            var defaultName = credential != null ? credential.UserName : string.Empty;
            var isSave = true;


            // 資格情報入力ダイアログを表示する
            credential = CredentialManager.PromptForCredentials(targetName, ref isSave, messageText, titleText);


            // 入力情報が受け取れてかつ保存をするなら
            if (credential != null && isSave)
            {
                // 資格情報の保存をする
                CredentialManager.SaveCredentials(targetName, credential);
            }


            // 結果を返す
            return credential;
        }


        /// <summary>
        /// 保存されている資格情報を削除します
        /// </summary>
        public static void RemoveCredential()
        {
            // 資格情報が最初から存在しないなら
            if (GetCredential(false) == null)
            {
                // 何もしない
                return;
            }


            // 資格情報を削除する
            CredentialManager.RemoveCredentials(Resources.CredentialTargetName);
        }
    }
}