import os
import sys
from pathlib import Path
import vdf

from PyQt6.QtWidgets import (
    QApplication, QMainWindow, QWidget, QVBoxLayout, QHBoxLayout,
    QLabel, QPushButton, QFileDialog, QMessageBox, QMenu, QFrame
)
from PyQt6.QtCore import Qt, QTimer
from PyQt6.QtGui import QFont, QAction, QIcon


def resource_path(relative_path):
    try:
        base_path = sys._MEIPASS
    except AttributeError:
        base_path = os.path.dirname(os.path.abspath(__file__))
    return os.path.join(base_path, relative_path)


if sys.platform == "win32":
    try:
        import ctypes
        ctypes.windll.shell32.SetCurrentProcessExplicitAppUserModelID('stalcraft.region.switcher.1.0')
    except Exception:
        pass


APP_ID = "1818450"
FORCED_REALM_FILE = "sc_forced_realm"
REGIONS = {
    "RU": "Россия",
    "GLOBAL": "EU/NA/ASIA",
}


def find_game_folder():
    if sys.platform != "win32":
        return None

    import winreg

    possible_keys = [
        (winreg.HKEY_LOCAL_MACHINE, r"SOFTWARE\Valve\Steam"),
        (winreg.HKEY_LOCAL_MACHINE, r"SOFTWARE\Wow6432Node\Valve\Steam"),
        (winreg.HKEY_CURRENT_USER, r"Software\Valve\Steam"),
    ]

    steam_path = None
    for hive, key in possible_keys:
        try:
            with winreg.OpenKey(hive, key) as reg_key:
                steam_path, _ = winreg.QueryValueEx(reg_key, "InstallPath")
                if os.path.isdir(steam_path):
                    break
        except Exception:
            continue

    if not steam_path:
        return None

    steam_path = Path(steam_path)
    libfolders = steam_path / "steamapps" / "libraryfolders.vdf"

    if libfolders.is_file():
        try:
            data = vdf.loads(libfolders.read_text(encoding="utf-8", errors="replace"))
            for folder in data.get("libraryfolders", {}).values():
                if isinstance(folder, dict) and APP_ID in folder.get("apps", {}):
                    lib_path = Path(folder["path"])
                    manifest = lib_path / "steamapps" / f"appmanifest_{APP_ID}.acf"
                    if manifest.is_file():
                        mdata = vdf.loads(manifest.read_text(encoding="utf-8", errors="replace"))
                        installdir = mdata.get("AppState", {}).get("installdir")
                        if installdir:
                            candidate = lib_path / "steamapps" / "common" / installdir
                            if candidate.is_dir():
                                appid_file = candidate / "steam_appid.txt"
                                if appid_file.is_file() and appid_file.read_text().strip() == APP_ID:
                                    return candidate.resolve()
        except Exception:
            pass

    manifest = steam_path / "steamapps" / f"appmanifest_{APP_ID}.acf"
    if manifest.is_file():
        try:
            data = vdf.loads(manifest.read_text(encoding="utf-8", errors="replace"))
            installdir = data.get("AppState", {}).get("installdir")
            if installdir:
                candidate = steam_path / "steamapps" / "common" / installdir
                if candidate.is_dir():
                    appid_file = candidate / "steam_appid.txt"
                    if appid_file.is_file() and appid_file.read_text().strip() == APP_ID:
                        return candidate.resolve()
        except Exception:
            pass

    return None


def start_game():
    url = f"steam://rungameid/{APP_ID}"
    try:
        if sys.platform == "win32":
            os.startfile(url)
        elif sys.platform == "darwin":
            os.system(f'open "{url}"')
        else:
            os.system(f'xdg-open "{url}"')
    except Exception as e:
        QMessageBox.critical(None, "Ошибка", f"Не удалось запустить игру\n{e}")


class App(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("STALCRAFT:X Region Switcher")
        self.setFixedSize(560, 360)

        self.setStyleSheet("""
            QMainWindow {
                background: qlineargradient(x1:0, y1:0, x2:1, y2:1,
                    stop:0 #0f0f1a, stop:1 #1a0f1f);
                border: 2px solid #6a5acd;
                border-radius: 14px;
            }
            QMainWindow::title {
                background: #2a1f3a;
                color: #d0c0ff;
                padding: 6px;
            }
            QLabel {
                color: #e0d8ff;
            }
            QPushButton {
                background-color: #483d8b;
                border: 1px solid #6a5acd;
                border-radius: 10px;
                color: white;
                font: bold 13pt 'Segoe UI';
                padding: 10px;
            }
            QPushButton:hover {
                background-color: #5a4fa8;
                border: 1px solid #7b68ee;
            }
            QPushButton:pressed {
                background-color: #3a2f6b;
            }
            QFrame#folderFrame {
                background-color: #1a1529;
                border: 1px solid #5a4fa8;
                border-radius: 12px;
            }
            QLabel#signature {
                color: #6c5ce7;
                font: italic 10pt 'Segoe UI';
                background: transparent;
            }
        """)

        central = QWidget()
        self.setCentralWidget(central)
        layout = QVBoxLayout(central)
        layout.setContentsMargins(24, 24, 24, 16)
        layout.setSpacing(18)

        region_widget = QWidget()
        region_hbox = QHBoxLayout(region_widget)
        region_hbox.addStretch()

        lbl = QLabel("Текущий регион: ")
        lbl.setFont(QFont("Segoe UI", 16, QFont.Weight.Bold))
        lbl.setAlignment(Qt.AlignmentFlag.AlignRight | Qt.AlignmentFlag.AlignVCenter)
        region_hbox.addWidget(lbl)

        self.region_label = QLabel("")
        self.region_label.setFont(QFont("Segoe UI", 16, QFont.Weight.Bold))
        self.region_label.setAlignment(Qt.AlignmentFlag.AlignLeft | Qt.AlignmentFlag.AlignVCenter)
        region_hbox.addWidget(self.region_label)

        region_hbox.addStretch()
        layout.addWidget(region_widget)

        self.btn_container = QWidget()
        self.btn_layout = QVBoxLayout(self.btn_container)
        layout.addWidget(self.btn_container, alignment=Qt.AlignmentFlag.AlignCenter)

        folder_frame = QFrame()
        folder_frame.setObjectName("folderFrame")
        folder_layout = QVBoxLayout(folder_frame)
        folder_layout.setContentsMargins(20, 16, 20, 16)

        title = QLabel("Папка игры")
        title.setFont(QFont("Segoe UI", 14, QFont.Weight.Bold))
        title.setStyleSheet("color: #b0a8ff;")
        folder_layout.addWidget(title)

        folder_hbox = QHBoxLayout()
        self.folder_label = QLabel("...")
        self.folder_label.setFont(QFont("Segoe UI", 13))
        self.folder_label.setWordWrap(True)
        folder_hbox.addWidget(self.folder_label, stretch=1)

        btn_change = QPushButton("Изменить")
        btn_change.setFixedWidth(120)
        btn_change.clicked.connect(self._choose_folder_manually)
        folder_hbox.addWidget(btn_change)

        folder_layout.addLayout(folder_hbox)
        layout.addWidget(folder_frame)

        layout.addStretch()

        signature = QLabel("насрано с помощью нейросети")
        signature.setObjectName("signature")
        signature.setAlignment(Qt.AlignmentFlag.AlignCenter)
        layout.addWidget(signature)

        QTimer.singleShot(100, self._init_ui)

    def _init_ui(self):
        path = find_game_folder()
        if path:
            self.game_folder = path
            self._update_folder_label(path)
        else:
            self._update_folder_label("Папка не найдена", success=False)

        self._update_buttons()

    def _choose_folder_manually(self):
        path = QFileDialog.getExistingDirectory(self, "Выберите папку STALCRAFT:X")
        if not path:
            return

        p = Path(path)
        appid = p / "steam_appid.txt"

        if not appid.is_file():
            QMessageBox.warning(self, "Ошибка", "Отсутствует файл steam_appid.txt")
            return

        if appid.read_text(encoding="utf-8").strip() != APP_ID:
            QMessageBox.warning(self, "Ошибка", "Неверный AppID в steam_appid.txt")
            return

        self.game_folder = p.resolve()
        self._update_folder_label(self.game_folder)
        self._update_buttons()

    def _update_folder_label(self, path_or_text, success=True):
        if success and isinstance(path_or_text, Path):
            s = str(path_or_text)
            if len(s) > 70:
                parts = s.split(os.sep)
                if len(parts) >= 3:
                    s = "…\\" + "\\".join(parts[-3:])
                else:
                    s = "…\\" + os.path.basename(s)
            self.folder_label.setText(s)
            self.folder_label.setToolTip(str(path_or_text))
            self.folder_label.setStyleSheet("color: #e0d8ff;")
        else:
            self.folder_label.setText(str(path_or_text))
            self.folder_label.setStyleSheet("color: #ff9999;")

    def _update_buttons(self):
        while self.btn_layout.count():
            item = self.btn_layout.takeAt(0)
            if widget := item.widget():
                widget.deleteLater()

        if not hasattr(self, 'game_folder') or not self.game_folder:
            self.region_label.setText("")
            return

        forced = self.game_folder / FORCED_REALM_FILE
        current = self._get_current_region()

        display = current if current != "не задан" else "автоматически"
        self.region_label.setText(display)

        hbox = QHBoxLayout()
        hbox.setSpacing(32)

        btn_text = "Выбрать регион" if current == "не задан" else "Сменить регион"
        change_btn = QPushButton(btn_text)
        change_btn.setFixedSize(190, 54)

        menu = QMenu(self)
        auto = QAction("Выбирать автоматически", self)
        auto.setData("AUTO")
        auto.triggered.connect(self._set_region)
        menu.addAction(auto)

        menu.addSeparator()

        for code, name in [("RU", "Россия (RU)"), ("GLOBAL", "EU/NA/ASIA (GLOBAL)")]:
            act = QAction(name, self)
            act.setData(code)
            act.triggered.connect(self._set_region)
            menu.addAction(act)

        change_btn.setMenu(menu)
        hbox.addWidget(change_btn)

        launch = QPushButton("Запустить игру")
        launch.setFixedSize(190, 54)
        launch.clicked.connect(start_game)
        hbox.addWidget(launch)

        self.btn_layout.addLayout(hbox)

    def _set_region(self):
        action = self.sender()
        if not action:
            return

        value = action.data()
        forced = self.game_folder / FORCED_REALM_FILE

        try:
            if value == "AUTO":
                if forced.exists():
                    forced.unlink()
            else:
                forced.write_text(value, encoding="utf-8")
            self._update_buttons()
        except Exception as e:
            QMessageBox.warning(self, "Ошибка", f"Не удалось сохранить регион\n{e}")

    def _get_current_region(self):
        if not hasattr(self, 'game_folder') or not self.game_folder:
            return "не задан"

        forced = self.game_folder / FORCED_REALM_FILE
        if forced.is_file():
            try:
                val = forced.read_text(encoding="utf-8").strip().upper()
                return REGIONS.get(val, f"неизвестно ({val})")
            except Exception:
                return "ошибка чтения"

        return "не задан"


if __name__ == "__main__":
    app = QApplication(sys.argv)

    if getattr(sys, 'frozen', False):
        icon = resource_path("icon.ico")
        if os.path.exists(icon):
            app.setWindowIcon(QIcon(icon))

    window = App()
    window.show()
    sys.exit(app.exec())