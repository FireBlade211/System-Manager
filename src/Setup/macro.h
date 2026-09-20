/*
*	[macro.h]
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
#define Static_SetImage(hwnd, type, handle) (SNDMSG((hwnd), STM_SETIMAGE, (WPARAM)(type), (LPARAM)(handle)))

#define Progress_SetRange32(hwnd, nMin, nMax) (SNDMSG((hwnd), PBM_SETRANGE32, (WPARAM)(nMin), (LPARAM)(nMax)))
#define Progress_SetPos(hwnd, pos) \
    (SNDMSG((hwnd), PBM_SETPOS, (WPARAM)(pos), 0))

#define Progress_SetStep(hwnd, step) \
    (SNDMSG((hwnd), PBM_SETSTEP, (WPARAM)(step), 0))

#define Progress_StepIt(hwnd) \
    (SNDMSG((hwnd), PBM_STEPIT, 0, 0))

#define Progress_SetState(hwnd, state) \
    (SNDMSG((hwnd), PBM_SETSTATE, (WPARAM)(state), 0))

#define Progress_SetMarquee(hwnd, enable, interval) \
    (SNDMSG((hwnd), PBM_SETMARQUEE, (WPARAM)(enable), (LPARAM)(interval)))