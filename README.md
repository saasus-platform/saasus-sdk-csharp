---

# SDK 使用手順

このガイドでは、`saasus-sdk-csharp` を **.NET 8** および **.NET Framework 4.8** アプリケーションに導入する手順を説明します。

---

## .NET 8 アプリケーションで SDK を使用する手順

### 1. SDK プロジェクトをプロジェクト参照として追加

#### Visual Studio を使用する場合
1. アプリケーションプロジェクトを右クリックし、**[参照の追加]** を選択します。
2. **[プロジェクト]** タブから `saasus-sdk-csharp` プロジェクトを選択します。
3. **[OK]** をクリックしてプロジェクト参照を追加します。

#### コマンドラインを使用する場合
以下のコマンドを実行して、`saasus-sdk-csharp` をプロジェクト参照として追加します：

- **アプリケーションプロジェクトのディレクトリにいる場合**:
  ```bash
  dotnet add reference ../saasus-sdk-csharp/saasus-sdk-csharp.csproj
  ```

- **異なるディレクトリにいる場合**:
  ```bash
  dotnet add ./MyApp/MyApp.csproj reference ./saasus-sdk-csharp/saasus-sdk-csharp.csproj
  ```

---

### 2. プロジェクトのビルドと実行

1. **ビルド**  
   以下のコマンドを実行してプロジェクトをビルドします：
   ```bash
   dotnet build
   ```

2. **実行**  
   アプリケーションを実行して、`saasus-sdk-csharp` の機能が正しく動作することを確認します：
   ```bash
   dotnet run
   ```

---

### 3. 動作確認

アプリケーションコード内で `saasus-sdk-csharp` を参照し、期待する機能が正しく動作するか確認してください。

---

### **JSONライブラリについての注意点**

SDKではデフォルトで**Newtonsoft.Json**ライブラリを使用しています。そのため、アプリケーションのJSONライブラリが`System.Text.Json`など他のライブラリに設定されている場合、以下の点に注意が必要です。

#### **Newtonsoft.Jsonを使用する場合**
モデルをそのままレスポンスとして返却できます。

```csharp
var userInfo = await userInfoApi.GetUserInfoAsync(token);

// モデルをそのままレスポンスに返却
return Ok(userInfo);
```

#### **異なるJSONライブラリを使用する場合**
シリアライズ処理を明示的に行う必要があります。

```csharp
var userInfo = await userInfoApi.GetUserInfoAsync(token);

// JSON形式でレスポンスを返す（SDKのToJsonメソッドを利用）
var jsonResponse = userInfo.ToJson();
return Results.Text(jsonResponse, "application/json");

// Newtonsoft.Jsonを使用して明示的にシリアライズ
var jsonResponse = JsonConvert.SerializeObject(users.VarUsers);
return Results.Text(jsonResponse, "application/json");
```

JSONライブラリの設定に応じて、必要なシリアライズ処理を追加してください。

---

## .NET Framework 4.8（packages.config）アプリケーションで SDK を使用する手順

### **1. SDK プロジェクトのビルド**

1. SDK のソースコードがあるディレクトリに移動します：
   ```bash
   cd <SDKのソースコードディレクトリ>
   ```

2. 以下のコマンドを実行してプロジェクトをビルドします：
   ```bash
   dotnet build -c Release
   ```

3. ビルドが成功したことを確認してください。

---

### **2. NuGet パッケージの生成**

1. 以下のコマンドを実行して NuGet パッケージを生成します：
   ```bash
   dotnet pack -c Release
   ```

2. コマンドを実行すると、`bin/Release` フォルダに `.nupkg` ファイルが生成されます。
   例:
   ```
   bin/Release/saasus-sdk-csharp.1.0.0.nupkg
   ```

---

### **3. ローカルフィードの設定**

#### **3.1 ローカルフィード用ディレクトリを作成**
ローカルフィードとして利用するディレクトリを作成します：
```bash
mkdir C:\LocalNuGetFeed
```

#### **3.2 パッケージをコピー**
生成した `.nupkg` ファイルをローカルフィードディレクトリにコピーします：
```bash
cp bin/Release/saasus-sdk-csharp.1.0.0.nupkg C:\LocalNuGetFeed
```

#### **3.3 Visual Studio でローカルフィードを設定**
1. **Visual Studio** を開きます。
2. メニューから **[ツール]** → **[オプション]** を選択します。
3. **[NuGet パッケージ マネージャー]** → **[パッケージ ソース]** を選択します。
4. **[追加]** ボタンをクリックし、以下を設定します：
    - **名前**: LocalFeed（任意の名前）
    - **ソース**: `C:\LocalNuGetFeed`
5. **[OK]** をクリックして保存します。

---

### **4. アプリケーションに SDK をインストール**

1. **対象プロジェクトを開く**
   - Visual Studio で `.NET Framework` プロジェクト（例: `MyApp.sln`）を開きます。

2. **NuGet パッケージをインストール**
   1. ソリューションエクスプローラーでプロジェクトを右クリックし、**[NuGet パッケージの管理]** を選択します。
   2. 「パッケージソース」から、追加した `LocalFeed` を選択します。
   3. 一覧に表示された `saasus-sdk-csharp` を選択し、**[インストール]** をクリックします。

---

## 環境変数の設定

SDKを正しく動作させるには、以下の環境変数の設定が必要です。

| 環境変数名          | 説明                          |
|-----------------------|-------------------------------|
| `SAASUS_SAAS_ID`      | SaaSの識別子（SaaS ID）       |
| `SAASUS_API_KEY`      | APIアクセス用のAPIキー       |
| `SAASUS_SECRET_KEY`   | クライアントシークレットキー |

環境変数の設定方法は、実行環境やアプリケーションの要件に応じて適切に行ってください。
例えば、`appsettings.json`に定義した値を読み込んで環境変数に設定することもできます。

---

