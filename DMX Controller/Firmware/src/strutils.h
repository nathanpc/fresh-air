/* 
 * strutils.h
 * A collection of functions to deal with strings.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#ifndef STRUTILS_H
#define	STRUTILS_H

#include "stdinc.h"

// Definitions.
#define UINT8_MAX_DIGITS  3
#define UINT16_MAX_DIGITS 5

// Conversions.
uint8_t ui16toa(char *buf, const uint16_t n);
uint8_t uitoa(char *buf, const uint8_t n);
uint8_t atoui16(const char *str, uint16_t *n);
uint8_t atoui(const char *str, uint8_t *n);

#endif	/* STRUTILS_H */

