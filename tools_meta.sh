#!/bin/bash
mkmeta() {
  local path="$1"
  local guid="$2"
  cat > "${path}.meta" <<META
fileFormatVersion: 2
guid: ${guid}
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData:
  assetBundleName:
  assetBundleVariant:
META
}
