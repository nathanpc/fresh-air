/* 
 * console.c
 * Provides a nice console for users to configure and control the system.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#include "console.h"
#include <string.h>
#include "dmx.h"
#include "uart.h"
#include "strutils.h"

// Private definitions.
#define CMD_MAX_LEN 10
#define ARG_MAX_LEN (UART_LINE_MAX_LEN - 2)

// Private methods.
bool ConsoleParseNextArgument(char *buf, const char *args, uint8_t *cursor);

// Commands.
void ConsoleCmdASO(const char *args);
void ConsoleCmdDSVA(const char *args);

void ConsoleCmdASO(const char *args) {
	char arg[ARG_MAX_LEN];
	uint8_t argCursor = 0;
	uint8_t argCount = 0;
    uint8_t outPort = 0;
	uint8_t outValue = 0;
	
	// Parse arguments.
	while (ConsoleParseNextArgument(arg, args, &argCursor)) {
		if (argCount == 0) {
			// Aux port number.
            if (!atoui(arg, &outPort)) {
                UARTSendLine("ERROR: Invalid AUX port argument");
                return;
            }
		} else if (argCount == 1) {
			// Aux port output state.
			if (!atoui(arg, &outValue)) {
				UARTSendLine("ERROR: Invalid AUX state value argument");
				return;
			}
		}
		
		argCount++;
	}
	
	// Check if we have the right number of arguments.
	if (argCount != 2) {
		char tmp[UINT8_MAX_DIGITS + 1];
		uitoa(tmp, argCount);
		
		UARTSendString("ERROR: Invalid number of arguments. Expected 2 got ");
		UARTSendLine(tmp);
		return;
	}
	
	// Set state of the Aux port output.
	switch (outPort) {
        case 1:
            if (outValue) {
                LATA |= AUX_1;
            } else {
                LATA &= ~AUX_1;
            }
            break;
        default:
            UARTSendLine("ERROR: Invalid AUX output port number");
            return;
    }

	ConsoleSendOK();
}

/**
 * Handles the DSVA (DMX Set Value for Address) command.
 * 
 * @param args Command arguments.
 */
void ConsoleCmdDSVA(const char *args) {
	char arg[ARG_MAX_LEN];
	uint8_t argCursor = 0;
	uint8_t argCount = 0;
	uint16_t dmxAddr = 0;
	uint8_t dmxValue = 0;
	
	// Parse arguments.
	while (ConsoleParseNextArgument(arg, args, &argCursor)) {
		if (argCount == 0) {
			// DMX address.
			if (!atoui16(arg, &dmxAddr)) {
				UARTSendLine("ERROR: Invalid DMX address argument");
				return;
			}
			
			if ((dmxAddr > 512) || (dmxAddr == 0)) {
				UARTSendLine("ERROR: Invalid DMX address number");
				return;
			}
		} else if (argCount == 1) {
			// DMX data.
			if (!atoui(arg, &dmxValue)) {
				UARTSendLine("ERROR: Invalid DMX value argument");
				return;
			}
		}
		
		argCount++;
	}
	
	// Check if we have the right number of arguments.
	if (argCount != 2) {
		char tmp[UINT8_MAX_DIGITS + 1];
		uitoa(tmp, argCount);
		
		UARTSendString("ERROR: Invalid number of arguments. Expected 2 got ");
		UARTSendLine(tmp);
		return;
	}
	
	// Set DMX value for the address.
	DMXSetValue(dmxAddr, dmxValue);
	ConsoleSendOK();
}

/**
 * Parses a command line.
 * 
 * @param line Line to be parsed.
 */
void ConsoleParseLine(const char *line) {
	char cmd[CMD_MAX_LEN];
	char args[ARG_MAX_LEN];
	uint8_t idxParse = 0;
	uint8_t idxTemp = 0;
	bool parsingArgs = false;
	char ch;
	
	// Initialize strings.
	cmd[0] = '\0';
	args[0] = '\0';
	
	// Parse the string until we get to the NULL terminator.
	do {
		ch = line[idxParse];
		
		// Check if we are parsing the command.
		if (!parsingArgs) {
			// Check if we have exceeded the size of the command buffer.
			if (idxTemp == CMD_MAX_LEN) {
				UARTSendLine("ERROR: Command too long\r\n");
				return;
			}

			// Check for the "command to arguments" separator.
			if ((ch == ' ') || (ch == '\0')) {
				cmd[idxTemp] = '\0';
				idxTemp = 0;
				parsingArgs = true;
			} else {
				// Append the character to the command buffer.
				cmd[idxTemp++] = ch;
			}
		} else {
			// Storing the arguments.
			args[idxTemp++] = ch;
		}
		
		idxParse++;
	} while (ch);
	
#ifdef DEBUG
	UARTSendString("Command: '");
	UARTSendString(cmd);
	UARTSendLine("'");
	UARTSendString("Arguments: '");
	UARTSendString(args);
	UARTSendLine("'");
#endif
	
	// Execute the commands.
	if (!strcmp(cmd, "DSVA")) {
		// DMX Set Value for Address.
		ConsoleCmdDSVA(args);
    } else if (!strcmp(cmd, "ASO")) {
        // Aux Set Output.
        ConsoleCmdASO(args);
	} else {
		// Invalid command.
		UARTSendString("ERROR: Invalid command '");
		UARTSendString(cmd);
		UARTSendLine("'");
	}
}

/**
 * Parses the next argument (space separated) in an arguments string.
 * 
 * @param  buf    Buffer to store the found argument.
 * @param  args   Original arguments buffer.
 * @param  cursor Parsing cursor (Should be initialized to 0 to get first argument).
 * @return        TRUE if a next argument was found.
 */
bool ConsoleParseNextArgument(char *buf, const char *args, uint8_t *cursor) {
	uint8_t idxTemp = 0;
	char ch = '\0';
	buf[0] = '\0';
	
	// Are we done here?
	if (args[*cursor] == '\0')
		return false;

	// Parse argument.
	do {
		ch = args[*cursor];

		// Check if we have exceeded the size of the argument buffer.
		if (*cursor == ARG_MAX_LEN) {
			UARTSendLine("ERROR: Single argument too long\r\n");
			return false;
		}

		// Check for a separator or terminator.
		if ((ch == ' ') || (ch == '\0')) {
			buf[idxTemp] = '\0';
			idxTemp = 0;
			if (ch != '\0')
				*cursor = *cursor + 1;
			
			return true;
		} else {
			// Append the character to the buffer.
			buf[idxTemp++] = ch;
		}
		
		// Advance cursor.
		*cursor = *cursor + 1;
	} while (ch);
	
	return false;
}

/**
 * Sends an OK response via UART.
 */
void ConsoleSendOK(void) {
	UARTSendLine("OK");
}
