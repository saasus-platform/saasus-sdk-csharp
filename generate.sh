#!/bin/bash

# 生成するモジュール名の配列
MODULES=("auth" "pricing" "billing" "awsmarketplace" "integration" "apilog" "communication" )

# sdkに含まれる生成したプログラムを削除
SDK_SRC_DIR="generated"
BASE_TEMP_DIR="temp"  # ホスト上のパス
DOCKER_BASE_TEMP_DIR="/local/${BASE_TEMP_DIR}"  # Docker内でのパス

# 既存のdocsディレクトリを削除
if [ -d "docs" ]; then
    rm -rf "docs"
    echo "Deleted existing docs directory"
fi
mkdir -p "docs"
echo "Created new docs directory"

# SDKの既存プログラム削除
for module in "${MODULES[@]}"
do
    target_dir="${SDK_SRC_DIR}/${module}api"
    if [ -d "${target_dir}" ]; then
        # Expansion ディレクトリを除外して削除
        find "${target_dir}" -mindepth 1 -maxdepth 1 ! -name "Expansion" -exec rm -rf {} +
        echo "Deleted contents of ${target_dir}, excluding Expansion"
    fi
done

# 各モジュールごとに処理
for module in "${MODULES[@]}"
do
    if [ "${module}" = "integration" ]; then
        target="/local/${module}.yml"
    else
        target="/local/${module}api.yml"
    fi

    # モジュールごとの一時出力ディレクトリ
    DOCKER_MODULE_TEMP_DIR="${DOCKER_BASE_TEMP_DIR}/${module}"

    # モジュールごとの追加プロパティを定義
    additional_properties="useOneOfDiscriminatorLookup=true,packageName=${module}api,targetFramework=netstandard2.0"
    
    case "${module}" in
        "auth")
            additional_properties+=",isAuthModule=true"
            ;;
        "pricing")
            additional_properties+=",isPricingModule=true"
            ;;
        "billing")
            additional_properties+=",isBillingModule=true"
            ;;
        "awsmarketplace")
            additional_properties+=",isAwsMarketplaceModule=true"
            ;;
        "integration")
            additional_properties+=",isIntegrationModule=true"
            ;;
        "apilog")
            additional_properties+=",isApiLogModule=true"
            ;;
        "communication")
            additional_properties+=",isCommunicationModule=true"
            ;;
        *)
            echo "Unknown module: ${module}"
            exit 1
            ;;
    esac
    docker run --rm -v ${PWD}:/local openapitools/openapi-generator-cli:v7.0.0 generate \
      -i ${target} \
      -g csharp \
      -o ${DOCKER_MODULE_TEMP_DIR} \
      --additional-properties ${additional_properties} \
      --template-dir /local/templates/openapi-generator/csharp-v7-0-0/

    # コンフリクトを避けるため TenantAttributeApi,UserAttributeApi に using ステートメントを追加
    if [ "${module}" = "auth" ]; then
        files_to_modify=("${BASE_TEMP_DIR}/${module}/src/${module}api/Api/TenantAttributeApi.cs" "${BASE_TEMP_DIR}/${module}/src/${module}api/Api/UserAttributeApi.cs")
        for file in "${files_to_modify[@]}"; do
            if [ -f "$file" ]; then
                # "using" ステートメントの最後に追記
                awk '
                    /^using / { print; last_using=1; next }
                    last_using && !inserted { print "using Attribute = authapi.Model.Attribute;"; inserted=1 }
                    { print }
                ' "$file" > "${file}.tmp" && mv "${file}.tmp" "$file"
                echo "Modified $file: Added 'using Attribute = authapi.Model.Attribute;'"
            fi
        done
    fi

done

    # .csprojファイルを削除
    find temp/ -name "*.csproj" -delete

for module in "${MODULES[@]}"
do
      MODULE_TEMP_DIR="${BASE_TEMP_DIR}/${module}"
    # プログラムをコピー
    if [ -d "${MODULE_TEMP_DIR}/src/${module}api" ]; then
        mkdir -p "${SDK_SRC_DIR}"
        cp -pr "${MODULE_TEMP_DIR}/src/${module}api" "${SDK_SRC_DIR}/"
        echo "Copied program files for ${module} to ${SDK_SRC_DIR}"
    else
        echo "No program files found for ${module}"
    fi

    # ドキュメントをコピー
    if [ -d "${MODULE_TEMP_DIR}/docs" ]; then
        mkdir -p "docs/${module}api"
        cp -pr "${MODULE_TEMP_DIR}/docs/." "docs/${module}api/docs/"
    else
        echo "No docs found for ${module}"
    fi
    # MODULE_TEMP_DIR直下のREADME.mdをコピー
    if [ -f "${MODULE_TEMP_DIR}/README.md" ]; then
        cp "${MODULE_TEMP_DIR}/README.md" "docs/${module}api/"
        echo "Copied README.md for ${module} to docs/${module}api/"
    else
        echo "No README.md found for ${module}"
    fi
    echo "copy ${module} success"
done

# 一時ディレクトリを削除
rm -rf "${BASE_TEMP_DIR}"
echo "delete generated success"