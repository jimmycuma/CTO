#!/bin/bash
mkassetmeta() {
  local path="$1"
  local guid="$2"
  cat > "${path}.meta" <<META
fileFormatVersion: 2
guid: ${guid}
NativeFormatImporter:
  externalObjects: {}
  mainObjectFileID: 11400000
  userData:
  assetBundleName:
  assetBundleVariant:
META
}
