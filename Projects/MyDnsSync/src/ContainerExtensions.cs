// zlib/libpng License
//
// Copyright (c) 2019 Sinoa
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

using System.ComponentModel;

namespace MyDnsSync
{
    /// <summary>
    /// IContainer インターフェイスの拡張関数実装用クラスです。
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// 指定されたコンポーネントの型がコンテナから取得します。
        /// </summary>
        /// <param name="container">コンポーネントを所持しているコンテナ</param>
        /// <returns>指定されたコンポーネント型のインスタンスがある場合はその参照を返します。見つけられなかった場合は null を返します。</returns>
        public static T GetComponent<T>(this IContainer container) where T : Component
        {
            // 指定された型のインスタンスを見つけるまでループ
            foreach (var component in container.Components)
            {
                // もし要求されたコンポーネントの型を扱えるなら
                if (component is T)
                {
                    // この参照を返す
                    return (T)component;
                }
            }


            // ループを抜けてきたということはコンポーネントを見つけられなかったということ
            return null;
        }
    }
}