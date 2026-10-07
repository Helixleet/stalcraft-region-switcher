<div align="center">

# STALZONE Region Switcher

Быстрая смена региона для STEAM клиента игры **STALZONE**  
Не слетает после обновлений игры.

**[English](README.en.md)** · Русский

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Windows](https://img.shields.io/badge/OS-Windows%2010%2F11-important)
![C#](https://img.shields.io/badge/C%23-5.0-239120?logo=csharp&logoColor=white)

</div>

## Что делает программа

Создаёт / изменяет файл `sc_forced_realm`, который отвечает за приоритетный выбор региона.

- `RU` → Россия
- `GLOBAL` → EU/NA/ASIA
- **Выбирать автоматически** → удаляет файл, регион выбирает сама игра

После смены региона достаточно просто запустить игру через кнопку в программе или обычным способом (через Steam).

## Как пользоваться

1. Скачай готовый `STALZONERegionSwitcher.exe` из раздела **[Releases](https://github.com/Helixleet/stalcraft-region-switcher/releases)**  
   **или** собери сам (см. ниже)
2. Запусти программу
3. Программа сама попытается найти папку игры по AppID `1818450`  
   • Не находит → нажми «Изменить» и укажи путь до папки игры вручную
4. Нажми «Сменить регион» / «Выбрать регион» → выбери Россия, EU/NA/ASIA или «Выбирать автоматически»
5. Нажми «Запустить игру» (или запусти игру самостоятельно через Steam)

## Сборка из исходников

### Самый простой способ (без Visual Studio)

Нужен только Windows 10/11 с .NET Framework 4.x (уже установлен).

```bat
git clone https://github.com/Helixleet/stalcraft-region-switcher.git
cd stalcraft-region-switcher
build.bat
```

Готовый exe: `bin\STALZONERegionSwitcher.exe`

### Через Visual Studio

1. Открой `STALZONERegionSwitcher.csproj` в Visual Studio 2019/2022
2. Конфигурация: **Release | Any CPU**
3. Build → Build Solution

> **Не используй** `dotnet build` — проект на .NET Framework 4.8, а не на современном .NET.

## Принцип работы

- Ищет Steam через реестр
- Читает `libraryfolders.vdf` и `appmanifest_1818450.acf`
- Находит папку игры и проверяет `steam_appid.txt`
- Запуск игры: `steam://rungameid/1818450`

## История

Ранее проект был на Python (PyQt6). Переписан на C# / .NET Framework 4.8, чтобы не требовать установки runtime и дополнительных зависимостей.

## Лицензия

[MIT](LICENSE)
