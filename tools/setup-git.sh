#!/usr/bin/env bash
# Menyiapkan Git untuk proyek JeiLangkung di macOS. Jalankan sekali setelah clone, dari folder repositori:
#   bash tools/setup-git.sh
set -euo pipefail
root="$(cd "$(dirname "$0")/.." && pwd)"
version="$(sed -n 's/^m_EditorVersion: *//p' "$root/JeiLangkung/ProjectSettings/ProjectVersion.txt" | tr -d '\r')"

if ! git -C "$root" lfs install --local; then
  echo "Git LFS belum terpasang. Pasang dengan: brew install git-lfs, lalu jalankan skrip ini lagi." >&2
fi

merge="/Applications/Unity/Hub/Editor/$version/Unity.app/Contents/Tools/UnityYAMLMerge"
if [ -x "$merge" ]; then
  git -C "$root" config merge.unityyamlmerge.name "Unity SmartMerge"
  git -C "$root" config merge.unityyamlmerge.driver "\"$merge\" merge -h -p --force --fallback none %O %B %A %A"
  git -C "$root" config merge.unityyamlmerge.recursive binary
  git -C "$root" config mergetool.unityyamlmerge.cmd "\"$merge\" merge -p \"\$BASE\" \"\$REMOTE\" \"\$LOCAL\" \"\$MERGED\""
  git -C "$root" config mergetool.unityyamlmerge.trustExitCode false
  echo "Selesai: Git LFS dan UnityYAMLMerge Unity $version disetel untuk repositori ini."
else
  echo "Unity $version belum ada di lokasi standar Unity Hub ($merge). Pasang versi itu lewat Unity Hub, lalu jalankan skrip ini lagi." >&2
  echo "Tanpa UnityYAMLMerge, scene digabung sebagai teks biasa." >&2
fi
