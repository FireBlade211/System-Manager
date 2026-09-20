/*
*	[utility.cpp]
*
*	System Manager Setup Application
*
* Copyright © FireBlade 2026
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
*
*/

#include "utility.h"

HBITMAP GetStockIconBitmap(SHSTOCKICONID siid, UINT uFlags, int width, int height)
{
	SHSTOCKICONINFO sii = {};
	sii.cbSize = sizeof(sii);

	HRESULT hr = SHGetStockIconInfo(siid, uFlags, &sii);
	if (FAILED(hr)) return 0;

	HICON hIcon = sii.hIcon;
	ICONINFO ii = {};

	if (!GetIconInfo(hIcon, &ii))
		return 0;

	BITMAP bm = {};
	if (!GetObjectW(ii.hbmColor, sizeof(bm), &bm))
	{
		DeleteObject(ii.hbmColor);
		DeleteObject(ii.hbmMask);
		return 0;
	}

	if (width == -1) width = bm.bmWidth;
	if (height == -1) height = bm.bmHeight;

	BITMAPINFO bi = {};
	bi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bi.bmiHeader.biWidth = width;
	bi.bmiHeader.biHeight = -height;
	bi.bmiHeader.biPlanes = 1;
	bi.bmiHeader.biBitCount = 32;
	bi.bmiHeader.biCompression = BI_RGB;

	void* bits = nullptr;
	HDC hdc = GetDC(nullptr);

	HBITMAP hBitmap = CreateDIBSection(
		hdc,
		&bi,
		DIB_RGB_COLORS,
		&bits,
		nullptr,
		0
	);

	if (hBitmap)
	{
		HDC memDC = CreateCompatibleDC(hdc);

		if (memDC)
		{
			HGDIOBJ oldBitmap = SelectObject(memDC, hBitmap);

			// Clear the bits to transparent.
			ZeroMemory(bits, static_cast<size_t>(width) * height * 4);


			DrawIconEx(
				memDC,
				0, 0,
				hIcon,
				width,
				height,
				0,
				nullptr,
				DI_NORMAL
			);

			SelectObject(memDC, oldBitmap);
			DeleteDC(memDC);
		}
		else
		{
			DeleteObject(hBitmap);
			hBitmap = nullptr;
		}
	}

	ReleaseDC(nullptr, hdc);

	DeleteObject(ii.hbmColor);
	DeleteObject(ii.hbmMask);

	return hBitmap;
}

bool DirectoryExists(const std::wstring& path)
{
	DWORD attributes = GetFileAttributesW(path.c_str());

	return attributes != INVALID_FILE_ATTRIBUTES &&
		(attributes & FILE_ATTRIBUTE_DIRECTORY);
}

bool CreateDirectoryRecursive(const std::wstring& path)
{
	if (DirectoryExists(path))
		return true;

	size_t separator = path.find_last_of(L"\\/");

	if (separator != std::wstring::npos &&
		!(separator == 2 && path[1] == L':'))
	{
		std::wstring parent = path.substr(0, separator);

		if (!CreateDirectoryRecursive(parent))
			return false;
	}

	if (CreateDirectoryW(path.c_str(), nullptr))
		return true;

	return GetLastError() == ERROR_ALREADY_EXISTS &&
		DirectoryExists(path);
}

std::wstring Utf8ToWide(const char* str)
{
	if (!str)
		return {};

	int length = MultiByteToWideChar(
		CP_UTF8,
		MB_ERR_INVALID_CHARS,
		str,
		-1,
		nullptr,
		0
	);

	if (length <= 0)
		return {};

	std::wstring result(length, L'\0');

	if (!MultiByteToWideChar(
		CP_UTF8,
		MB_ERR_INVALID_CHARS,
		str,
		-1,
		result.data(),
		length))
	{
		return {};
	}

	result.resize(length - 1);
	return result;
}

bool IsDotNet452OrLaterInstalled()
{
	HKEY hKey = nullptr;

	if (RegOpenKeyExW(
		HKEY_LOCAL_MACHINE,
		L"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full",
		0,
		KEY_READ,
		&hKey) != ERROR_SUCCESS)
	{
		return false;
	}

	DWORD release = 0;
	DWORD size = sizeof(release);

	LONG result = RegQueryValueExW(
		hKey,
		L"Release",
		nullptr,
		nullptr,
		reinterpret_cast<LPBYTE>(&release),
		&size);

	RegCloseKey(hKey);

	return result == ERROR_SUCCESS && release >= 379893;
}