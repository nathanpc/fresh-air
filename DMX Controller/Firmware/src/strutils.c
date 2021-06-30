/* 
 * strutils.c
 * A collection of functions to deal with strings.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#include "strutils.h"
#include <string.h>

// Private methods.
uint16_t ui16pow(const uint16_t _base, const uint8_t exp);

/**
 * Converts a 16-bit unsigned integer number into a string.
 * 
 * @param  buf String to hold the number.
 * @param  n   Number to be converted.
 * @return     Number of characters in the string.
 * 
 * @remark #buf must be able to hold 5+NUL characters.
 */
uint8_t ui16toa(char *buf, const uint16_t n) {
	char tmp[UINT16_MAX_DIGITS + 1];
	uint8_t len = 0;
	uint16_t x = n;
	tmp[UINT16_MAX_DIGITS] = '\0';
	
	// Populate the string backwards.
	do {
		// Convert digit into character.
		tmp[UINT16_MAX_DIGITS - len - 1] = '0' + (x % 10);
		
		// Advance to the next number.
		x /= 10;
		len++;
	} while (x != 0);
	
	// Copy our temporary string into the passed one.
	strcpy(buf, tmp + UINT16_MAX_DIGITS - len);

	return len;
}

/**
 * Converts a 8-bit unsigned integer number into a string.
 * 
 * @param  buf String to hold the number.
 * @param  n   Number to be converted.
 * @return     Number of characters in the string.
 * 
 * @remark #buf must be able to hold 3+NUL characters.
 */
uint8_t uitoa(char *buf, const uint8_t n) {
	char tmp[UINT8_MAX_DIGITS + 1];
	uint8_t len = 0;
	uint16_t x = n;
	tmp[UINT8_MAX_DIGITS] = '\0';
	
	// Populate the string backwards.
	do {
		// Convert digit into character.
		tmp[UINT8_MAX_DIGITS - len - 1] = '0' + (x % 10);
		
		// Advance to the next number.
		x /= 10;
		len++;
	} while (x != 0);
	
	// Copy our temporary string into the passed one.
	strcpy(buf, tmp + UINT8_MAX_DIGITS - len);

	return len;
}

/**
 * Converts a string into a 16-bit unsigned integer.
 * 
 * @param  str String to be converted.
 * @param  n   Pointer to the resulting number variable.
 * @return     Number of characters converted. 0 if an error occurred.
 */
uint8_t atoui16(const char *str, uint16_t *n) {
	uint16_t num = 0;
	int8_t idx = (int8_t)strlen(str) - 1;
	uint8_t len = 0;
	char ch;
	
	// Go through the string backwards.
	while (idx >= 0) {
		ch = str[idx];
		
		// Check if character is a digit.
		if ((ch < '0') && (ch > '9'))
			return 0;
		
		// Add multiplied character to the accumulator.
		num += (ch - '0') * ui16pow(10, len);

		// Go to the next character.
		idx--;
		len++;
	}
	
	// Set the number pointer and return the numbers parsed.
	*n = num;
	return len;
}

/**
 * Converts a string into a 8-bit unsigned integer.
 * 
 * @param  str String to be converted.
 * @param  n   Pointer to the resulting number variable.
 * @return     Number of characters converted. 0 if an error occurred.
 */
uint8_t atoui(const char *str, uint8_t *n) {
	uint16_t num = (uint16_t)n;
	uint8_t len = atoui16(str, &num);
	
	*n = (uint8_t)num;
	
	return len;
}

/**
 * Calculates the power of a 16-bit unsigned integer.
 * 
 * @param  _base Base number.
 * @param  exp   Exponent number.
 * @return       Power of the two numbers.
 * 
 * @remark This function doesn't check for overflows.
 */
uint16_t ui16pow(const uint16_t _base, const uint8_t exp) {
	uint16_t n = 1;
	for (uint8_t i = 0; i < exp; i++)
		n *= _base;
	
	return n;
}