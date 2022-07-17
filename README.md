# SaveProjectTemplate

ProjectTemplateを作るコードです。


最低限の機能しかありません。

dependencieの設定とかリポジトリの設定はできません。

Assets以下のすべてのフォルダを保存します。


## 使い方

![image](https://user-images.githubusercontent.com/34824723/179381769-9acdf4b3-5cb3-4028-b07b-b407bf169c44.png)

名前とバージョンとUnityのバージョンを設定してCreateTemplateボタンを押します

プロジェクトフォルダ―直下にgentomecacheというフォルダが作られます。

その中にpackageフォルダがあるので、tarアーカイブしてgzip圧縮します。

![image](https://user-images.githubusercontent.com/34824723/179382020-5dfa2a02-9f2b-48b8-896f-b830acb5fd43.png)


圧縮したファイル名を 

Name-version.tgz

にします。

例：

Name　:　com.gentome

version　:　1.0.0

だとファイル名は

com.gentome-1.0.0.tgz

にする必要があります。

作成したtgzファイルを

C:\Program Files\Unity\Hub\Editor\2021.3.6f1\Editor\Data\Resources\PackageManager\ProjectTemplates

に置きます。パスはデフォルトのものでUnityのバージョンによって変わります。

UnityHubを再起動すればテンプレートが読み込めるはずです。

置いたあとはgentomecacheのフォルダを消す必要があります。

## 問題

圧縮まで自動でしたかったけど、文字化けしたりUnityHubで解凍できなかったりで断念した。



## ライセンス

圧縮ライブラリの
https://github.com/adamhathcock/sharpcompress
のプラグインを入れています

License Copyright (c) 2014  Adam Hathcock

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
