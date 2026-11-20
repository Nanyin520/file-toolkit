# File Toolkit · 文件工具箱

A tiny **local-only** Windows utility with two tools in one window:

1. **Corrupt** — produce a corrupted copy of any file (same size, same extension), leaving the original untouched.
2. **Timestamps** — set a file's creation / modified / accessed time to any value you type.

Nothing leaves your machine. No install, no network, no config files, no admin rights.

一个纯本地运行的 Windows 小工具，一个窗口里两件事：

1. **文件破坏**：生成一个与原件同大小、同后缀的损坏副本，原文件不动。
2. **修改时间**：把文件的创建 / 修改 / 访问时间改成你指定的任意时间。

不联网、不安装、不要管理员权限。

![Corrupt page](docs/screenshots/zh-corrupt.png)
![Timestamps page](docs/screenshots/zh-timestamps.png)

English UI:

![Corrupt page (EN)](docs/screenshots/en-corrupt.png)
![Timestamps page (EN)](docs/screenshots/en-timestamps.png)

---

## Features · 功能

| | |
|---|---|
| Corrupted copy | Streams cryptographically random bytes (`RNGCryptoServiceProvider`) into the output file, matching the original byte-for-byte in length. Progress bar with live percentage. |
| Output folder | Optional. Pick any folder, or leave empty to save next to the original. Collisions never overwrite: `name_broken.ext`, `name_broken1.ext`, … |
| Timestamps | Six-segment time editor (year / month / day / hour / minute / second). Click a segment and type, use `↑ ↓` to step, `← →` to move between segments, wheel to adjust. Tick which of the three timestamps to apply. |
| Bilingual UI | Chinese and English, switchable with the button in the top-right corner. The choice is remembered. Defaults to your system language. |
| Drag & drop | Drop a file anywhere on the window. |
| Look | Fully custom-drawn controls (rounded cards, segmented tabs, toggles, capsule progress bar) — no stock WinForms widgets. |

- 损坏副本按 64 KB 分块写入随机字节，进程内实时回报进度。
- 输出目录可选；重名自动追加序号，绝不覆盖。
- 时间编辑器支持点选分段、键盘输入自动进位、↑↓ 增减、滚轮微调。
- 中英双语，右上角一键切换并记住选择；首次启动跟随系统语言。

---

## Requirements · 运行要求

- **Windows 7 / 8 / 10 / 11** (32-bit or 64-bit — the binary is AnyCPU)
- **.NET Framework 4.x** — already present on Windows 8 and later. Windows 7 needs 4.0+ installed; if missing, Windows will prompt you with a download link.
- No other dependency. No DLL, no installer, no registry writes.

The only thing the app writes outside its own folder is the language preference, stored at
`%APPDATA%\FileBreaker\settings.txt`.

---

## Usage · 使用

1. Double-click `FileBreaker.exe`.
2. **Corrupt**: click *Browse* (or drag a file in), optionally choose an output folder, then *Start*.
3. **Timestamps**: pick a file, adjust the target time, tick the fields you want, then *Apply*.
4. Switch language any time with the `EN` / `中文` button at the top right.

---

## Build from source · 从源码编译

Single file, no project file, no third-party library — plain `csc` is enough:

```bat
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" ^
  /nologo /target:winexe /optimize+ /utf8output ^
  /win32icon:AppIcon.ico ^
  /out:FileBreaker.exe FileBreaker.cs
```

`AppIcon.ico` contains six PNG-compressed sizes (16 → 256 px). Version info
(product name, company, file version, copyright) is declared with `[assembly: …]`
attributes at the top of `FileBreaker.cs` and gets compiled into the exe automatically.

---

## Notes · 说明

- **Antivirus false positives** are possible. The program's whole job is writing a file
full of random bytes, which heuristic engines sometimes flag. Whitelist the exe if needed.
- The corrupted copy is *deliberately* unusable — it is intended for sanitising damaged
samples, testing error handling of file parsers, or producing junk fixtures.
- Setting timestamps requires write access to the target file's folder.

---

## Project layout · 目录

```
FileBreaker.cs            all source: theme, custom controls, UI, logic, version resources
AppIcon.ico               application icon (6 embedded sizes)
docs/appicon-1024.png     source artwork for the icon
docs/screenshots/         UI screenshots (zh / en)
```

---

## License · 授权

[**PolyForm Noncommercial License 1.0.0**](LICENSE) — see the `LICENSE` file for the full text.

- **Free** for personal use, study and learning, hobby projects, and any other non-commercial purpose.
- **Commercial use** (using it in a business, bundling it with a paid product, or using it as part of a service you sell) requires **prior permission** — please open an Issue or contact [@Nanyin520](https://github.com/Nanyin520) first. I'm usually happy to say yes.
- Want a different arrangement (MIT, commercial license, paid integration)? Just ask — I'm flexible.

中文说明：

- **个人使用、学习研究、非商业目的**：免费，直接下载用，不用打招呼。
- **商业用途**（在公司里用、打包进收费产品、作为你对外提供的服务的一部分）：请**先联系我**，通过本仓库 Issues 留言或找 [@Nanyin520](https://github.com/Nanyin520)。一般来说我很好说话。
- 想换更宽松的许可（比如 MIT）或者要商业授权，也可以直接谈。

---

*By Nanyin*
