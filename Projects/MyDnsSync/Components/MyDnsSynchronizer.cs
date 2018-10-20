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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AdysTech.CredentialManager;
using AngleSharp.Parser.Html;
using MyDnsSync.Properties;

namespace MyDnsSync.Components
{
    /// <summary>
    /// MyDNSにアドレスの同期を行うコンポーネントクラスです
    /// </summary>
    public partial class MyDnsSynchronizer : Component
    {
        // クラス変数宣言
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string MyDnsLoginUrl = Resources.MyDnsLoginUrl;
        private static readonly string SuccessMessagePattern = Resources.MyDnsSyncSuccessMessagePattern;



        /// <summary>
        /// MyDnsSynchronizer のインスタンスを初期化します
        /// </summary>
        /// <param name="container">このコンポーネントを保持するコンテナ</param>
        /// <exception cref="ArgumentNullException">container が null です</exception>
        public MyDnsSynchronizer(IContainer container)
        {
            // コンテナに自身を追加して、フォームデザイナの初期化関数を叩く
            (container ?? throw new ArgumentNullException(nameof(container))).Add(this);
            InitializeComponent();
        }


        /// <summary>
        /// 指定された資格情報を用いて、MyDNSにアドレスの同期を非同期で行います
        /// </summary>
        /// <param name="credential">MyDNSにログインするログイン情報</param>
        /// <returns>同期に成功した場合は true を、失敗した場合は false を返す非同期のタスクを返します</returns>
        public async Task<bool> DoSynchronizeAsync(NetworkCredential credential)
        {
            // ログインするためのリクエストを用意
            var authValue = credential.GetBasicAuthString();
            var authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
            var request = new HttpRequestMessage(HttpMethod.Get, MyDnsLoginUrl);
            request.Headers.Authorization = authHeader;


            // 非同期でログインを行い成功以外のステータスコードを返された場合は
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                // 失敗を返す
                return false;
            }


            // レスポンスのボディからHtmlパースをされたドキュメントを取得する
            var responseBody = await response.Content.ReadAsStringAsync();
            var htmlParser = new HtmlParser();
            var document = await htmlParser.ParseAsync(responseBody);


            // 文字列リーダを生成する
            using (var reader = new StringReader(document.Body.TextContent))
            {
                // 末尾に到達するまで、1行ずつ読み込む
                var lineText = default(string);
                while ((lineText = await reader.ReadLineAsync()) != null)
                {
                    // 前後の無駄な余白を削除して、成功パターン文字列と一致するなら
                    lineText = lineText.Trim();
                    if (lineText == SuccessMessagePattern)
                    {
                        // 同期に成功したことを返す
                        return true;
                    }
                }
            }


            // ループから抜けてきたということは、成功文字列パターンが見つからなかったということで、失敗を返す
            return false;
        }
    }
}
