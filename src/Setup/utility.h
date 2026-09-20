/*
*	[utility.h]
*
*	System Manager Setup Application
*
* Copyright © FireBlade 2026
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
*
*/

#pragma once
#include <windows.h>
#include <string>

HBITMAP GetStockIconBitmap(SHSTOCKICONID siid, UINT uFlags, int width, int height);
bool DirectoryExists(const std::wstring& path);
bool CreateDirectoryRecursive(const std::wstring& path);
std::wstring Utf8ToWide(const char* str);
bool IsDotNet452OrLaterInstalled();