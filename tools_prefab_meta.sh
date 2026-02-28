#!/bin/bash
mkprefabmeta() {
  local path="$1"
  local guid="$2"
  cat > "${path}.meta" <<META
fileFormatVersion: 2
guid: ${guid}
PrefabImporter:
  externalObjects: {}
  userData:
  assetBundleName:
  assetBundleVariant:
META
}
