/* 
 * console.h
 * Provides a nice console for users to configure and control the system.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#ifndef CONSOLE_H
#define	CONSOLE_H

#include "stdinc.h"

// Parsing.
void ConsoleParseLine(const char *line);

// Responses.
void ConsoleSendOK(void);

#endif	/* CONSOLE_H */

