<div align="center">

# STALCRAFT:X Region Switcher 

Быстрая смена региона для STEAM клиента игры **STALCRAFT:X**  
Не слетает после обновлений игры.

[![Python](https://img.shields.io/badge/Python-3.8%2B-blue?logo=python&logoColor=white)](https://www.python.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Windows](https://img.shields.io/badge/OS-Windows%2010%2F11-important)

</div>

## Что делает программа

Создаёт / изменяет файл `sc_forced_realm`, который отвечает за приоритетный выбор региона. 

- `RU`     → RU 
- `GLOBAL` → EU/NA/ASIA 

После смены региона достаточно просто запустить игру через кнопку в программе или обычным способом.


## Как пользоваться

1. Скачай готовый `SRS.exe` из раздела **[Releases](https://github.com/helixleet/stalcraft-region-switcher/releases)**  
2. Запусти программу (антивирус может ругаться — это нормально для pyinstaller-сборок)  
3. Программа сама попытается найти папку STALCRAFT:X  
   • Не находит → нажми «Изменить» и сам укажи путь до папки STALCRAFT
4. Нажми «Сменить регион» → выбери Россия или EU/NA/ASIA. Чтоб регион определялся самой игрой: выбирать автоматически
5. Нажми «Запустить игру» (или запусти игру самостоятельно)

## Сборка из исходников

# 1. Клонируем репозиторий
```bash
git clone https://github.com/helixleet/stalcraft-region-switcher.git
cd stalcraft-region-switcher
```
# 2. Устанавливаем зависимости
```bash
pip install -r requirements.txt
```
# 3. Запуск
```bash
python change_region.py
```