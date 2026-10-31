# Menyiapkan Git untuk proyek JeiLangkung di Windows. Jalankan sekali setelah clone, dari folder repositori:
#   powershell -ExecutionPolicy Bypass -File tools\setup-git.ps1
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$versionLine = Select-String -Path (Join-Path $root 'JeiLangkung\ProjectSettings\ProjectVersion.txt') -Pattern '^m_EditorVersion:'
$version = $versionLine.Line.Split(':')[1].Trim()

git -C $root lfs install --local
if ($LASTEXITCODE -ne 0) { Write-Warning 'Git LFS tidak tersedia. Pasang Git for Windows terbaru (sudah termasuk LFS), lalu jalankan skrip ini lagi.' }

function Set-GitConfig([string]$name, [string]$value) {
    # Argumen disusun sendiri: PowerShell 5 memotong tanda kutip di dalam nilai bila dikirim langsung ke git.
    $escaped = $value.Replace('"', '\"')
    $process = Start-Process git -ArgumentList "-C `"$root`" config $name `"$escaped`"" -NoNewWindow -Wait -PassThru
    if ($process.ExitCode -ne 0) { throw "git config $name gagal." }
}

$merge = "C:\Program Files\Unity\Hub\Editor\$version\Editor\Data\Tools\UnityYAMLMerge.exe"
if (Test-Path -LiteralPath $merge) {
    $path = $merge -replace '\\', '/'
    Set-GitConfig 'merge.unityyamlmerge.name' 'Unity SmartMerge'
    Set-GitConfig 'merge.unityyamlmerge.driver' "`"$path`" merge -h -p --force --fallback none %O %B %A %A"
    Set-GitConfig 'merge.unityyamlmerge.recursive' 'binary'
    Set-GitConfig 'mergetool.unityyamlmerge.cmd' "`"$path`" merge -p `"`$BASE`" `"`$REMOTE`" `"`$LOCAL`" `"`$MERGED`""
    Set-GitConfig 'mergetool.unityyamlmerge.trustExitCode' 'false'
    Write-Host "Selesai: Git LFS dan UnityYAMLMerge Unity $version disetel untuk repositori ini."
}
else {
    Write-Warning "Unity $version belum ada di lokasi standar Unity Hub ($merge). Pasang versi itu lewat Unity Hub, lalu jalankan skrip ini lagi. Tanpa UnityYAMLMerge, scene digabung sebagai teks biasa."
}
