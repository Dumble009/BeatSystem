

　変更場所を記載。

〈UI〉
・Needleのマテリアル（外見）を半透明に変更。
・UIにジャストタイムカウントを追加。
　タイミングよくテンポを刻むとジャストタイムカウントが1増える。


〈UIUpdater〉
・『フォルダの位置そのもの』を変更。循環参照を防ぐため。
　現在はScript/UIControll直下に存在。

（Controlのlは1個な気がする）


〈MusicPase.cs〉
・originalTempoの扱いを変更。
　Unityから操作できる値をoriginalBPMとして定義し、originalTempoはAwake()時にoriginalBPMから算出する。
・originalBPMを138に変更。
　BGM自体を変えた。みんなも好きな曲にしよう。
※BGMを入れるときはBGMのリバーブ版も用意すること。
・Thresholdを0.08→0.2に変更。
・delegate（≒イベントリスナー）のonJustTimingを実装。
　タイミングよくテンポを刻むと発動する。
・OnBeatにOnJustTiming()を追加。


〈AudioSourceController.cs〉
・Mathクラスの使用の為、Systemを名前空間に追加。
・ReberbedBGM、warning、justTimingをオーディオクリップ、オーディオソースに追加。
・OnTempoChange()の挙動を下のように変更。

　既定のテンポより早ければ　→　警告音を鳴らす
　既定のテンポより遅ければ　→　音量を落とし、リバーブ（くぐもった様な音になる効果）をかける

・FadeCoroutine()の挙動を変更。
　現在の実装では体験中にBGMの音量が変化するため、フェードアウト時にはフェードアウト開始時点の音量を参照する。


〈OSCQuaternionBeater〉
・テストの簡単化の為Up ThresholdとDown Thresholdを変更。
　これは好み。変更はUnity Editorから行う。


〈WebSocketEulerBeater〉
・delegate（≒イベントリスナー）のAngleChangeHandlerを追加。
　スマホの角度が変化したとき、WebSocketEulerBeaterが出した角度をある関数に一斉送信する。
　また、これへ関数を登録するpublic関数をWebSocketEulerBeaterに追加実装。

　マジでどうするのが正解か分からなかった。
　これでいいのか。そうであって下さい


〈NeedleController〉
・WebSocketEulerBeaterのAngleChangeHandlerを使って腕の動きを同期する形に改造。
　WebSocketに依存することになったため、アセンブリの設定にWebSocketを追加