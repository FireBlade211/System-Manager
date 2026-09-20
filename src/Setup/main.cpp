/*
*	[main.cpp]
* 
*	System Manager Setup Application
* 
* Copyright © FireBlade 2026
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* 
*/

#include <windows.h>
#include <commctrl.h>
#include "resource.h"
#include <windowsx.h>
#include "utility.h"
#include "macro.h"
#include <shlobj.h>
#include <wchar.h>
#include <string>
#include <format>
#include <vector>
#include "libs/miniz.h"
#include <thread>
#include <shlwapi.h>

#define WM_PROGRESS (WM_APP + 1)
#define WM_COMPLETE (WM_APP + 2)
#define WM_SHOWMSG  (WM_APP + 3)

HFONT g_hTitleFont = NULL;
std::thread g_installThread;

struct {
	std::wstring installLocation = L"";
	bool createDesktopShortcut = false;
	std::atomic_bool failed = false;
	std::atomic_bool stopInstall = false;
	std::vector<std::wstring> installedFiles = {};
	std::atomic_bool installing = true;
} g_installInfo;

INT_PTR CALLBACK WelcomePageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK LicensePageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK InstallLocationPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK AdditionalOptionsPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK SummaryPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK InstallPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK FinishPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK ErrorPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam);

int WINAPI wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine, _In_ int nCmdShow)
{
	int cArgs = 0;
	LPWSTR* args = CommandLineToArgvW(lpCmdLine, &cArgs);

	bool uninstall = false;
	bool help = false;

	for (int i = 0; i < cArgs; i++)
	{
		LPWSTR a = args[i];

		if (_wcsicmp(a, L"/Uninstall") == 0)
			uninstall = true;
		else if (_wcsicmp(a, L"/Help") == 0)
			help = true;
	}

	LocalFree(args);

	if (help)
	{
		std::wstring helpText =
			//L"========================\r\n"
			//L"  System Manager Setup  \r\n"
			//L"========================\r\n"
			//L"\r\n"
			L"Options:\r\n"
			L"    /Uninstall     Uninstall System Manager.\r\n"
			L"    /Help            Show this help message.\r\n"
			L"\r\n";

		MessageBoxW(NULL, helpText.c_str(), L"Help", MB_OK);

		return 0;
	}
	else if (uninstall)
	{
		if (MessageBoxW(NULL, L"Are you sure you want to uninstall System Manager?", L"Uninstall", MB_ICONWARNING | MB_OKCANCEL)
			== IDOK)
		{
			WCHAR szExe[MAX_PATH] = L"";
			GetModuleFileNameW(NULL, szExe, MAX_PATH);

			WCHAR szDir[MAX_PATH] = L"";
			wcscpy_s(szDir, MAX_PATH, szExe);

			PathRemoveFileSpecW(szDir);

			WCHAR szWildcard[MAX_PATH] = L"";
			swprintf_s(szWildcard, MAX_PATH, L"%s\\*", szDir);

			std::vector<std::wstring> filesToDelete = {};
			std::vector<std::wstring> dirsToDelete = {};

			WIN32_FIND_DATA data = {};
			HANDLE hFind = FindFirstFileW(szWildcard, &data);

			if (hFind != INVALID_HANDLE_VALUE)
			{
				do
				{
					if (wcscmp(data.cFileName, L".") == 0 ||
						wcscmp(data.cFileName, L"..") == 0)
						continue;

					if (data.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
						dirsToDelete.push_back(std::wstring(szDir) + L"\\" + std::wstring(data.cFileName));
					else
						filesToDelete.push_back(std::wstring(szDir) + L"\\" + std::wstring(data.cFileName));

				} while (FindNextFileW(hFind, &data));

				FindClose(hFind);
			}

			// This will only find one recursive level deep, but that's fine because that's as
			// far as it should go anyway
			for (std::wstring& dir : dirsToDelete)
			{
				swprintf_s(szWildcard, MAX_PATH, L"%s\\*", dir.c_str());

				hFind = FindFirstFileW(szWildcard, &data);

				if (hFind != INVALID_HANDLE_VALUE)
				{
					do
					{
						if (wcscmp(data.cFileName, L".") == 0 ||
							wcscmp(data.cFileName, L"..") == 0)
							continue;

						if (!(data.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY))
						{
							filesToDelete.push_back(
								dir + L"\\" + data.cFileName
							);
						}

					} while (FindNextFileW(hFind, &data));

					FindClose(hFind);
				}
			}

			BOOL bRestart = FALSE;

			auto rebootDelete = [&](const std::wstring& path) {
				bRestart = TRUE;
				MoveFileExW(path.c_str(), NULL, MOVEFILE_DELAY_UNTIL_REBOOT);
			};

			for (std::wstring& file : filesToDelete)
				if (!DeleteFileW(file.c_str()))
					rebootDelete(file);

			for (std::wstring& dir : dirsToDelete)
				if (!RemoveDirectoryW(dir.c_str()))
					rebootDelete(dir);

			LPCWSTR subKey =
				L"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\System Manager";

			RegDeleteTreeW(HKEY_LOCAL_MACHINE, subKey);
			RegDeleteKeyW(HKEY_LOCAL_MACHINE, subKey);

			if (bRestart)
			{
				// We would use RestartDialog(Ex), but since Windows Vista it ignores pszPrompt and just gives a generic
				// message of "Reboot to apply these settings" which would not work for an uninstaller, so we make it ourselves

				TASKDIALOGCONFIG tdc = {};
				tdc.cbSize = sizeof(tdc);
				tdc.pszWindowTitle = L"Uninstall";
				tdc.pszMainInstruction = L"A reboot is required";
				tdc.pszContent = L"In order to finish uninstallation of System Manager, you need to reboot your system. Do you want to reboot now?";

				TASKDIALOG_BUTTON buttons[2] = {};

				buttons[0].nButtonID = 1001;
				buttons[0].pszButtonText = L"Restart now";

				buttons[1].nButtonID = 1002;
				buttons[1].pszButtonText = L"Restart later";

				tdc.pButtons = buttons;
				tdc.cButtons = 2;

				tdc.pszMainIcon = TD_WARNING_ICON;

				int iButton = 0;
				HRESULT hr = TaskDialogIndirect(&tdc, &iButton, NULL, NULL);

				if (SUCCEEDED(hr))
				{
					if (iButton == 1001)
					{
						HANDLE hToken = NULL;

						if (OpenProcessToken(
							GetCurrentProcess(),
							TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,
							&hToken))
						{
							TOKEN_PRIVILEGES tp = {};
							tp.PrivilegeCount = 1;

							if (LookupPrivilegeValueW(
								NULL,
								SE_SHUTDOWN_NAME,
								&tp.Privileges[0].Luid))
							{
								tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

								AdjustTokenPrivileges(
									hToken,
									FALSE,
									&tp,
									sizeof(tp),
									NULL,
									NULL
								);
							}

							CloseHandle(hToken);
						}

						ExitWindowsEx(
							EWX_REBOOT,
							SHTDN_REASON_MAJOR_APPLICATION |
							SHTDN_REASON_MINOR_INSTALLATION |
							SHTDN_REASON_FLAG_PLANNED
						);
					}
				}
			}
		}

		return 0;
	}

	INITCOMMONCONTROLSEX iccex = {};
	iccex.dwSize = sizeof(iccex);
	iccex.dwICC = ICC_WIN95_CLASSES | ICC_STANDARD_CLASSES;

	InitCommonControlsEx(&iccex);

	LOGFONTW lf = {};
	lf.lfWeight = FW_BOLD;
	lf.lfHeight = -MulDiv(12, 96, 72);
	lf.lfWidth  = 0;
	
	wcscpy_s(lf.lfFaceName, L"Verdana");

	g_hTitleFont = CreateFontIndirectW(&lf);

	PROPSHEETPAGEW ppsp[8] = {};
	
	ppsp[0].pszTemplate = MAKEINTRESOURCEW(IDD_WELCOME);
	ppsp[0].dwFlags		= PSP_HIDEHEADER;
	ppsp[0].pfnDlgProc = WelcomePageProc;

	ppsp[1].pszTemplate = MAKEINTRESOURCEW(IDD_LICENSE);
	ppsp[1].dwFlags = PSP_USEHEADERTITLE | PSP_USEHEADERSUBTITLE;
	ppsp[1].pszHeaderTitle = L"License Agreement";
	ppsp[1].pszHeaderSubTitle = L"Please agree to the following license agreement";
	ppsp[1].pfnDlgProc = LicensePageProc;

	ppsp[2].pszTemplate = MAKEINTRESOURCEW(IDD_LOCATION);
	ppsp[2].dwFlags = PSP_USEHEADERTITLE | PSP_USEHEADERSUBTITLE;
	ppsp[2].pszHeaderTitle = L"Install Location";
	ppsp[2].pszHeaderSubTitle = L"Choose where to install System Manager";
	ppsp[2].pfnDlgProc = InstallLocationPageProc;
	
	ppsp[3].pszTemplate = MAKEINTRESOURCEW(IDD_OPTIONS);
	ppsp[3].dwFlags = PSP_USEHEADERTITLE | PSP_USEHEADERSUBTITLE;
	ppsp[3].pszHeaderTitle = L"Additional Options";
	ppsp[3].pszHeaderSubTitle = L"Choose additional options before installation";
	ppsp[3].pfnDlgProc = AdditionalOptionsPageProc;

	ppsp[4].pszTemplate = MAKEINTRESOURCEW(IDD_SUMMARY);
	ppsp[4].dwFlags = PSP_USEHEADERTITLE | PSP_USEHEADERSUBTITLE;
	ppsp[4].pszHeaderTitle = L"Summary";
	ppsp[4].pszHeaderSubTitle = L"Verify your changes before installation. To edit your choices, click Back.";
	ppsp[4].pfnDlgProc = SummaryPageProc;

	ppsp[5].pszTemplate = MAKEINTRESOURCEW(IDD_INSTALL);
	ppsp[5].dwFlags = PSP_USEHEADERTITLE | PSP_USEHEADERSUBTITLE;
	ppsp[5].pszHeaderTitle = L"Installing System Manager";
	ppsp[5].pszHeaderSubTitle = L"Please wait while System Manager is being installed on your computer...";
	ppsp[5].pfnDlgProc = InstallPageProc;

	ppsp[6].pszTemplate = MAKEINTRESOURCEW(IDD_FINISH);
	ppsp[6].dwFlags		= PSP_HIDEHEADER;
	ppsp[6].pfnDlgProc  = FinishPageProc;

	ppsp[7].pszTemplate = MAKEINTRESOURCEW(IDD_ERROR);
	ppsp[7].dwFlags = PSP_HIDEHEADER;
	ppsp[7].pfnDlgProc = ErrorPageProc;

	for (int i = 0; i < ARRAYSIZE(ppsp); i++)
	{
		ppsp[i].dwSize = sizeof(PROPSHEETPAGEW);
		ppsp[i].hInstance = hInstance;
	}

	PROPSHEETHEADERW psh = {};
	psh.dwSize = sizeof(psh);
	psh.dwFlags = PSH_WIZARD97 | PSH_MODELESS | PSH_PROPSHEETPAGE | PSH_WATERMARK;
	psh.pszCaption = L"System Manager Setup";
	psh.ppsp = ppsp;
	psh.nPages = ARRAYSIZE(ppsp);
	psh.pszbmWatermark = MAKEINTRESOURCEW(IDB_WATERMARK);
	psh.hInstance = hInstance;

	HWND hwndPropSheet = (HWND)PropertySheetW(&psh);

	MSG msg = {};
	while (GetMessageW(&msg, NULL, 0, 0))
	{
		if (!PropSheet_IsDialogMessage(hwndPropSheet, &msg))
		{
			if (PropSheet_GetCurrentPageHwnd(hwndPropSheet) == NULL)
				PostQuitMessage(0);

			TranslateMessage(&msg);
			DispatchMessageW(&msg);
		}
	}

	DeleteObject(g_hTitleFont);

	return 0;
}

INT_PTR CALLBACK WelcomePageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
		SetWindowFont(GetDlgItem(hDlg, IDC_STATIC_TITLE), g_hTitleFont, TRUE);
		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_NEXT);
			break;
		}

		break;
	}
	}

	return 0;
}

INT_PTR CALLBACK LicensePageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
		CheckDlgButton(hDlg, IDC_RADIO_NOAGREE, BST_CHECKED);

		SetDlgItemTextW(hDlg, IDC_EDIT_LICENSE,
			L"MIT License\r\n\r\n"

			L"Copyright (c) 2026 FireBlade\r\n\r\n"

			L"Permission is hereby granted, free of charge, to any person obtaining a copy "
			L"of this software and associated documentation files (the \"Software\"), to deal "
			L"in the Software without restriction, including without limitation the rights "
			L"to use, copy, modify, merge, publish, distribute, sublicense, and/or sell "
			L"copies of the Software, and to permit persons to whom the Software is "
			L"furnished to do so, subject to the following conditions:\r\n\r\n"

			L"The above copyright notice and this permission notice shall be included in all "
			L"copies or substantial portions of the Software.\r\n\r\n"

			L"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR "
			L"IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, "
			L"FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE "
			L"AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER "
			L"LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, "
			L"OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE "
			L"SOFTWARE.");

		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			if (IsDlgButtonChecked(hDlg, IDC_RADIO_AGREE))
				PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK | PSWIZB_NEXT);
			else
				PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK);
			
			break;
		}

		break;
	}
	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
		case IDC_RADIO_AGREE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK | PSWIZB_NEXT);
			break;
		case IDC_RADIO_NOAGREE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK);
			break;
		}

		break;
	}

	return 0;
}

INT_PTR CALLBACK InstallLocationPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
	{
		SetWindowFont(GetDlgItem(hDlg, IDC_STATIC_TITLE), g_hTitleFont, TRUE);

		PWSTR pszPath;
		HRESULT hr = SHGetKnownFolderPath(FOLDERID_ProgramFiles, KF_FLAG_DEFAULT, NULL, &pszPath);

		if (SUCCEEDED(hr))
		{
			LPCWSTR extra = L"\\FireBlade\\System Manager";
			size_t len = wcslen(pszPath) + 1 + wcslen(extra);

			LPWSTR szNewPath = new WCHAR[len];
			swprintf_s(szNewPath, len, L"%s%s", pszPath, extra);

			SetDlgItemTextW(hDlg, IDC_EDIT_INSTALLLOCATION, szNewPath);
			delete[] szNewPath;

			CoTaskMemFree(pszPath);
		}

		break;
	}
	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
		case IDC_BUTTON_BROWSE:
		{
			HRESULT hr = CoInitializeEx(0, COINIT_APARTMENTTHREADED);
			if (SUCCEEDED(hr))
			{
				BROWSEINFOW bi = {};
				bi.hwndOwner = hDlg;
				bi.pidlRoot = NULL;
				bi.lpszTitle = L"Select a folder and click Select.";
				bi.ulFlags = BIF_RETURNONLYFSDIRS | BIF_USENEWUI;
				bi.lpfn = [](HWND hwnd, UINT m, LPARAM lP, LPARAM lpData) -> int CALLBACK {
					switch (m)
					{
					case BFFM_INITIALIZED:
						SendMessageW(hwnd, BFFM_SETOKTEXT, NULL, (LPARAM)L"Select");
						break;
					}

				return 0;
				};
				
				LPITEMIDLIST pidl = SHBrowseForFolderW(&bi);
				if (pidl)
				{
					WCHAR szPath[MAX_PATH] = L"";
					SHGetPathFromIDListW(pidl, szPath);

					SetDlgItemTextW(hDlg, IDC_EDIT_INSTALLLOCATION, szPath);
					CoTaskMemFree(pidl);
				}

				CoUninitialize();
			}

			break;
		}
		}

		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK | PSWIZB_NEXT);
			break;
		case PSN_WIZNEXT:
		{
			g_installInfo.installLocation.resize(GetWindowTextLengthW(GetDlgItem(hDlg, IDC_EDIT_INSTALLLOCATION)));
			
			GetDlgItemTextW(hDlg, IDC_EDIT_INSTALLLOCATION, g_installInfo.installLocation.data(), g_installInfo.installLocation.size() + 1);

			break;
		}
		}

		break;
	}
	}

	return 0;
}

INT_PTR CALLBACK AdditionalOptionsPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
		CheckDlgButton(hDlg, IDC_CHECK_SHORTCUT, BST_CHECKED);
		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK | PSWIZB_NEXT);
			break;
		case PSN_KILLACTIVE:
			g_installInfo.createDesktopShortcut = IsDlgButtonChecked(hDlg, IDC_CHECK_SHORTCUT) == BST_CHECKED;
			break;
		}

		break;
	}
	}

	return 0;
}

INT_PTR CALLBACK SummaryPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
		{
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_BACK | PSWIZB_NEXT);
			PropSheet_SetNextText(GetParent(hDlg), L"&Install");

			// Generate the summary.
			std::wstring summary = L"Installation summary\r\n";

			auto append = [&](std::wstring_view str) {
				summary += str;
				summary += L"\r\n";
			};

			append(std::format(L"    Install to: {}", g_installInfo.installLocation));

			if (g_installInfo.createDesktopShortcut)
			{
				append(L"    Additional options:");
				append(L"        Create a desktop shortcut");
			}

			SetDlgItemTextW(hDlg, IDC_EDIT_SUMMARY, summary.c_str());

			break;
		}
		}

		break;
	}
	}

	return 0;
}

ITaskbarList3* g_pTaskbarList = NULL;

void PostInstallMessage(HWND hDlg, std::wstring message)
{
	auto* pMessage = new std::wstring(std::move(message));

	if (!PostMessageW(
		hDlg,
		WM_SHOWMSG,
		0,
		reinterpret_cast<LPARAM>(pMessage)))
	{
		delete pMessage;
	}
}

INT_PTR CALLBACK InstallPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
	{
		HRESULT hr = CoInitializeEx(0, COINIT_APARTMENTTHREADED);
		if (SUCCEEDED(hr))
		{
			hr = CoCreateInstance(CLSID_TaskbarList, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&g_pTaskbarList));

			if (SUCCEEDED(hr))
			{
				g_pTaskbarList->HrInit();

				HINSTANCE hInstance = GetModuleHandleW(nullptr);

				g_installThread = std::thread([hDlg, hInstance]() {
					HRSRC hRes = FindResourceW(
						hInstance,
						MAKEINTRESOURCEW(IDR_PACKAGE),
						RT_RCDATA
					);

					if (hRes)
					{
						HGLOBAL hData = LoadResource(hInstance, hRes);

						if (hData)
						{
							DWORD size = SizeofResource(hInstance, hRes);
							void* data = LockResource(hData);

							CreateDirectoryRecursive(g_installInfo.installLocation);

							mz_zip_archive zip_archive;
							memset(&zip_archive, 0, sizeof(zip_archive));

							// Initialize the reader with memory buffer.
							if (!mz_zip_reader_init_mem(&zip_archive, data, size, 0)) {
								PostInstallMessage(hDlg, L"Failed to initialize the ZIP reader from memory.");
								//pList->SetProgressState(GetParent(hDlg), TBPF_ERROR);
								PostMessageW(hDlg, WM_PROGRESS, (WPARAM)1, (LPARAM)0);

								g_installInfo.failed = true;
								g_installInfo.installing = false;

								PostMessageW(hDlg, WM_COMPLETE, 1, 0);
								return;
							}

							g_installInfo.installing = true;

							ULONGLONG ullTotalSize = 0;

							// Iterate through all files in the archive.
							int num_files = mz_zip_reader_get_num_files(&zip_archive);
							int totalProgress = num_files;

							if (!IsDotNet452OrLaterInstalled())
								totalProgress += 2;

							PostMessageW(hDlg, WM_PROGRESS, 2, MAKELPARAM(0, totalProgress));

							for (int i = 0; i < num_files; i++) {
								if (g_installInfo.stopInstall)
								{
									// Roll back everything installed.
									for (const std::wstring& path : g_installInfo.installedFiles)
										DeleteFileW(path.c_str());

									RemoveDirectoryW(g_installInfo.installLocation.c_str());

									mz_zip_reader_end(&zip_archive);

									g_installInfo.installing = false;
									PostMessageW(hDlg, WM_COMPLETE, 2, 0);
									
									return;
								}

								mz_zip_archive_file_stat file_stat;
								if (!mz_zip_reader_file_stat(&zip_archive, i, &file_stat)) {
									continue;
								}

								std::wstring filename = Utf8ToWide(file_stat.m_filename);
								PostInstallMessage(hDlg, std::format(L"Extracting: {} ({} bytes)", filename, (UINT)file_stat.m_uncomp_size));

								//pList->SetProgressState(GetParent(hDlg), TBPF_NORMAL);
								PostMessageW(hDlg, WM_PROGRESS, (WPARAM)0, MAKELPARAM(i + 1, totalProgress));
								//pList->SetProgressValue(GetParent(hDlg), i + 1, num_files);
								//Progress_SetPos(GetDlgItem(hDlg, IDC_PROGRESS_INSTALL), i + 1);

								if (file_stat.m_is_directory)
								{
									CreateDirectoryRecursive(
										g_installInfo.installLocation + L"\\" + filename
									);

									continue;
								}

								// Allocate memory for the uncompressed file.
								void* p_extracted = malloc(file_stat.m_uncomp_size);
								if (!p_extracted)
								{
									PostInstallMessage(hDlg, L"Failed to allocate memory for extracted file.");
									g_installInfo.failed = true;
									g_installInfo.installing = false;

									PostMessageW(hDlg, WM_COMPLETE, 1, 0);
									mz_zip_reader_end(&zip_archive);
									return;
								}

								// Extract the file to memory.
								if (mz_zip_reader_extract_to_mem(&zip_archive, i, p_extracted, file_stat.m_uncomp_size, 0)) {
									std::wstring path =
										g_installInfo.installLocation + L"\\" +
										filename;

									size_t separator = path.find_last_of(L"\\/");

									if (separator != std::wstring::npos)
									{
										CreateDirectoryRecursive(path.substr(0, separator));
									}

									HANDLE hFile = CreateFileW(path.c_str(), GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_ALWAYS, 0, NULL);
									if (hFile != INVALID_HANDLE_VALUE)
									{
										if (file_stat.m_uncomp_size > MAXDWORD)
										{
											PostInstallMessage(hDlg, L"File is too large to extract.");
											g_installInfo.failed = true;
											g_installInfo.installing = false;

											//onFinish();
											PostMessageW(hDlg, WM_COMPLETE, 1, 0);
											free(p_extracted);
											mz_zip_reader_end(&zip_archive);
											return;
										}

										DWORD dwWritten = 0;

										if (!WriteFile(hFile, p_extracted, file_stat.m_uncomp_size, &dwWritten, NULL) ||
											dwWritten != file_stat.m_uncomp_size)
										{
											g_installInfo.failed = true;
											g_installInfo.installing = false;

											PostInstallMessage(hDlg, std::format(L"Failed to write file: {}", filename));
											//onFinish();
											PostMessageW(hDlg, WM_COMPLETE, 1, 0);
											free(p_extracted);
											CloseHandle(hFile);

											mz_zip_reader_end(&zip_archive);
											return;
										}

										ullTotalSize += file_stat.m_uncomp_size;

										g_installInfo.installedFiles.push_back(path);
										CloseHandle(hFile);
									}
									else
									{
										PostInstallMessage(hDlg, std::format(
											L"Failed to create file: {}",
											filename
										));

										g_installInfo.failed = true;
										g_installInfo.installing = false;

										//onFinish();
										PostMessageW(hDlg, WM_COMPLETE, 1, 0);
										free(p_extracted);
										mz_zip_reader_end(&zip_archive);
										return;
									}
								}
								else {
									PostInstallMessage(hDlg, std::format(L"Failed to extract file {}", filename));
									free(p_extracted);

									g_installInfo.installing = false;
									g_installInfo.failed = true;

									//onFinish();
									PostMessageW(hDlg, WM_COMPLETE, 1, 0);

									mz_zip_reader_end(&zip_archive);
									return;
								}

								free(p_extracted);
							}

							if (!IsDotNet452OrLaterInstalled() && !g_installInfo.failed)
							{
								WCHAR szTemp[MAX_PATH] = L"";
								
								GetTempPathW(MAX_PATH, szTemp);
								PathCombineW(szTemp, szTemp, L"netfrmwrk452-setup.exe");

								PostMessageW(hDlg, WM_PROGRESS, (WPARAM)0, MAKELPARAM(totalProgress - 1, totalProgress));
								PostInstallMessage(hDlg, L".NET Framework runtime not found. Installing .NET Framework 4.5.2 runtime...");

								// Note: If any of this fails, we ignore it since it will prompt the user to download the runtime
								// when they run the app if it's not installed anyway
								HRSRC hResSetup = FindResourceW(
									hInstance,
									MAKEINTRESOURCEW(IDR_NETFRMWRK_452_SETUP),
									RT_RCDATA
								);

								if (hResSetup)
								{
									HGLOBAL hDataSetup = LoadResource(hInstance, hResSetup);

									if (hDataSetup)
									{
										DWORD size = SizeofResource(hInstance, hResSetup);
										void* data = LockResource(hDataSetup);

										HANDLE hFile = CreateFileW(szTemp, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, 0, NULL);
										if (hFile != INVALID_HANDLE_VALUE)
										{
											DWORD dwWritten = 0;
											WriteFile(hFile, data, size, &dwWritten, NULL);

											CloseHandle(hFile);
										}

										SHELLEXECUTEINFOW sei = {};
										sei.cbSize = sizeof(sei);
										sei.fMask = SEE_MASK_NOCLOSEPROCESS;
										sei.hwnd = hDlg;
										sei.lpFile = szTemp;
										sei.lpParameters = L"/q";

										if (ShellExecuteExW(&sei))
										{
											if (sei.hProcess != INVALID_HANDLE_VALUE)
											{
												DWORD dwWaitResult = WaitForSingleObject(sei.hProcess, INFINITE);

												if (dwWaitResult == WAIT_OBJECT_0)
												{
													PostMessageW(hDlg, WM_PROGRESS, (WPARAM)0, MAKELPARAM(totalProgress, totalProgress));
													PostInstallMessage(hDlg, L"Successfully installed .NET Framework 4.5.2 runtime.");
												}

												CloseHandle(sei.hProcess);
											}
										}
									}
								}
								else
								{
									PostInstallMessage(hDlg, L"Could not find the .NET Framework 4.5.2 runtime.");
								}

								// this will fail if the file doesn't exist but we don't care
								DeleteFileW(szTemp);
							}

							// Clean up.
							mz_zip_reader_end(&zip_archive);
							PostMessageW(hDlg, WM_COMPLETE, 0, 0);
							g_installInfo.installing = false;

							HKEY hKey = nullptr;

							LPCWSTR subKey =
								L"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\System Manager";

							if (RegCreateKeyExW(HKEY_LOCAL_MACHINE, subKey, 0, nullptr, 0, KEY_WRITE, nullptr, &hKey, nullptr) == ERROR_SUCCESS)
							{
								RegSetValueExW(
									hKey,
									L"DisplayName",
									0,
									REG_SZ,
									reinterpret_cast<const BYTE*>(L"System Manager"),
									sizeof(L"System Manager")
								);

								RegSetValueExW(
									hKey,
									L"Publisher",
									0,
									REG_SZ,
									reinterpret_cast<const BYTE*>(L"FireBlade"),
									sizeof(L"FireBlade")
								);

								RegSetValueExW(
									hKey,
									L"DisplayVersion",
									0,
									REG_SZ,
									reinterpret_cast<const BYTE*>(L"1.2"),
									sizeof(L"1.2")
								);

								WCHAR exePath[MAX_PATH];
								GetModuleFileNameW(nullptr, exePath, ARRAYSIZE(exePath));

								CopyFileW(exePath, (g_installInfo.installLocation +
									L"\\SystemManagerSetup.exe").c_str(), FALSE);

								std::wstring uninstallString =
									L"\"" + g_installInfo.installLocation +
									L"\\SystemManagerSetup.exe\" /Uninstall";

								RegSetValueExW(
									hKey,
									L"UninstallString",
									0,
									REG_SZ,
									reinterpret_cast<const BYTE*>(uninstallString.c_str()),
									static_cast<DWORD>((uninstallString.size() + 1) * sizeof(wchar_t))
								);

								std::wstring displayIcon =
									g_installInfo.installLocation + L"\\System Manager.exe";

								RegSetValueExW(
									hKey,
									L"DisplayIcon",
									0,
									REG_SZ,
									reinterpret_cast<const BYTE*>(displayIcon.c_str()),
									static_cast<DWORD>((displayIcon.size() + 1) * sizeof(wchar_t))
								);

								DWORD estimatedSize = ullTotalSize / 1024;

								RegSetValueExW(
									hKey,
									L"EstimatedSize",
									0,
									REG_DWORD,
									reinterpret_cast<const BYTE*>(&estimatedSize),
									sizeof(estimatedSize)
								);

								DWORD dwNo = 1;

								RegSetValueExW(
									hKey,
									L"NoModify",
									0,
									REG_DWORD,
									reinterpret_cast<BYTE*>(&dwNo),
									sizeof(dwNo)
								);

								RegSetValueExW(
									hKey,
									L"NoRepair",
									0,
									REG_DWORD,
									reinterpret_cast<BYTE*>(&dwNo),
									sizeof(dwNo)
								);

								RegCloseKey(hKey);
							}

							PostInstallMessage(hDlg, L"Installation complete.");
						}
						else
						{
							PostInstallMessage(hDlg, L"Failed to load the embedded installation package.");
							g_installInfo.failed = true;
							PostMessageW(hDlg, WM_PROGRESS, 1, 0);
							PostMessageW(hDlg, WM_COMPLETE, 1, 0);
						}
					}
					else
					{
						PostInstallMessage(hDlg, L"Failed to find the embedded installation package.");
						g_installInfo.failed = true;
						PostMessageW(hDlg, WM_PROGRESS, 1, 0);
						PostMessageW(hDlg, WM_COMPLETE, 1, 0);
					}
				});
			}
		}

		break;
	}
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL);
			PropSheet_SetNextText(GetParent(hDlg), NULL);

			break;
		case PSN_WIZNEXT:
			g_pTaskbarList->SetProgressState(hDlg, TBPF_NOPROGRESS);

			g_pTaskbarList->Release();
			CoUninitialize();

			if (g_installInfo.failed)
			{
				SetWindowLongPtrW(hDlg, DWLP_MSGRESULT, IDD_ERROR);
				return TRUE;
			}

			break;
		case PSN_QUERYCANCEL:
		{
			if (g_installInfo.failed)
			{
				PropSheet_PressButton(GetParent(hDlg), PSBTN_NEXT);
				SetWindowLongPtrW(hDlg, DWLP_MSGRESULT, TRUE);
				return TRUE;
			}

			if (g_installInfo.installing)
			{
				if (MessageBoxW(hDlg, L"Do you want to cancel the installation?", L"Cancel", MB_ICONWARNING | MB_OKCANCEL) == IDOK)
				{
					g_installInfo.stopInstall = true;

					if (g_installThread.joinable())
						g_installThread.join();

					// close the wizard
					return FALSE;
				}
				else
				{
					SetWindowLongPtrW(hDlg, DWLP_MSGRESULT, TRUE);
					return TRUE;
				}
			}

			break;
		}
		}

		break;
	}
	case WM_PROGRESS:
	{
		if (!g_pTaskbarList) return 0;

		if (wParam == 0)
		{
			g_pTaskbarList->SetProgressState(hDlg, TBPF_NORMAL);
			g_pTaskbarList->SetProgressValue(hDlg, LOWORD(lParam), HIWORD(lParam));

			Progress_SetPos(GetDlgItem(hDlg, IDC_PROGRESS_INSTALL), LOWORD(lParam));
		}
		else if (wParam == 2)
		{
			Progress_SetRange32(
				GetDlgItem(hDlg, IDC_PROGRESS_INSTALL),
				LOWORD(lParam),
				HIWORD(lParam)
			);
		}
		else
		{
			Progress_SetState(GetDlgItem(hDlg, IDC_PROGRESS_INSTALL), PBST_ERROR);
			g_pTaskbarList->SetProgressState(hDlg, TBPF_ERROR);
		}

		break;
	case WM_COMPLETE:
	{
		if (g_installThread.joinable())
			g_installThread.join();

		if (!g_installInfo.failed && g_installInfo.createDesktopShortcut)
		{
			LPWSTR pszDesktop;
			HRESULT hr = SHGetKnownFolderPath(FOLDERID_Desktop, KF_FLAG_DEFAULT, NULL, &pszDesktop);

			if (SUCCEEDED(hr))
			{
				IShellLinkW* pShellLink;
				hr = CoCreateInstance(CLSID_ShellLink, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pShellLink));

				if (SUCCEEDED(hr))
				{
					std::wstring path = (g_installInfo.installLocation + L"\\System Manager.exe");

					pShellLink->SetPath(path.c_str());
					pShellLink->SetIconLocation(path.c_str(), 0);
					pShellLink->SetWorkingDirectory(g_installInfo.installLocation.c_str());

					IPersistFile* pFile;
					hr = pShellLink->QueryInterface(IID_PPV_ARGS(&pFile));

					if (SUCCEEDED(hr))
					{
						std::wstring shortcutPath = std::format(L"{}\\System Manager.lnk", pszDesktop);

						hr = pFile->Save(shortcutPath.c_str(), TRUE);
						pFile->Release();
					}

					pShellLink->Release();
				}

				CoTaskMemFree(pszDesktop);
			}
		}

		if (wParam != 2)
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_NEXT | PSWIZB_CANCEL);

		break;
	}
	case WM_SHOWMSG:
	{
		std::wstring* str = (std::wstring*)lParam;

		ListBox_InsertString(GetDlgItem(hDlg, IDC_LIST_DETAILS), 0, str->c_str());

		delete str;

		break;
	}
	}
	}

	return 0;
}

INT_PTR CALLBACK FinishPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
		SetWindowFont(GetDlgItem(hDlg, IDC_STATIC_TITLE), g_hTitleFont, TRUE);
		CheckDlgButton(hDlg, IDC_CHECK_LAUNCH, BST_CHECKED);
		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_FINISH);
			break;
		case PSN_WIZFINISH:
		{
			if (IsDlgButtonChecked(hDlg, IDC_CHECK_LAUNCH) == BST_CHECKED)
			{
				ShellExecuteW(hDlg, NULL, L"System Manager.exe", NULL, g_installInfo.installLocation.c_str(), SW_SHOW);
			}
			break;
		}
		}

		break;
	}
	}

	return 0;
}

INT_PTR CALLBACK ErrorPageProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
	case WM_INITDIALOG:
		SetWindowFont(GetDlgItem(hDlg, IDC_STATIC_TITLE), g_hTitleFont, TRUE);
		break;
	case WM_NOTIFY:
	{
		LPNMHDR nmhdr = (LPNMHDR)lParam;
		switch (nmhdr->code)
		{
		case PSN_SETACTIVE:
			PropSheet_SetWizButtons(GetParent(hDlg), PSWIZB_CANCEL | PSWIZB_FINISH);
			break;
		}

		break;
	}
	}

	return 0;
}