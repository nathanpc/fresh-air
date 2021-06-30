/* 
 * uart.c
 * Deals with all of the UART schenenigans.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#include "uart.h"
#include "strutils.h"
#include "console.h"

// Private definitions.
#define BAUD_9600  832
#define BAUD_19K2  416
#define BAUD_57K6  138
#define BAUD_115K  68
#define BAUD_250K  32
#define BAUD_DMX   BAUD_250K
#define BAUD_COMMS BAUD_19K2
#define TIMEOUT_COUNT 10

// Private variables.
char lineBuffer[UART_LINE_MAX_LEN + 1];
uint8_t idxBuffer = 0;
bool rcvComms = false;
uint32_t timeoutCount = 0;

// Private methods.
void UARTTimeoutTimerStatus(bool on);
void UARTEnableRXPPS(void);
void UARTEnableDMXTXPPS(void);
void UARTDisableDMXTXPPS(void);

/**
 * Sets up the UART peripheral for DMX communication.
 */
void UARTInitialize(void) {
	// Setup PPS for TX and RX.
	UARTEnableDMXTXPPS();
	UARTEnableRXPPS();
	
	// Configure the baud rate generator.
	TX1STAbits.BRGH    = 1;          // High baud rate mode.
	BAUD1CONbits.BRG16 = 1;          // 16-bit baud rate generator.
	SP1BRG             = BAUD_COMMS; // Baud rate for general communication.
	
	// Setup the serial parameters.
	TX1STAbits.SYNC = 0;  // Asynchronous mode.
	RC1STAbits.SPEN = 1;  // Configures the TX pin.
	TX1STAbits.TX9  = 1;  // 9th bit used to emulate 2-bit STOP.
	RC1STAbits.RX9  = 0;  // No 9th bit for receiving.
	
	// Setup interrupts.
	PIE1bits.TXIE   = 0;
	PIE1bits.RCIE   = 1;
	
	// Enable the receiver and transmitter.
	RC1STAbits.CREN = 1;
	TX1STAbits.TXEN = 1;
}

/**
 * Sets up Timer2 to be used for the communication request timeout.
 */
void UARTInitializeTMR2Timeout(void) {
	// Setup the timer for its slowest setting of 128us @ 32MHz.
	T2CONbits.T2CKPS  = 0b11;    // 1:64 Prescaler.
	T2CONbits.T2OUTPS = 0b1111;  // 1:16 Postscaler.
	PR2               = 0xFF;    // Maximum period.
	
	// Enable the interrupt and make sure the timer is OFF.
	PIE1bits.TMR2IE  = 1;
	UARTTimeoutTimerStatus(false);
}

/**
 * Handles the timeout timer overflow.
 */
void UARTTimeoutHandler(void) {
	// Check if we have timed out.
	if (timeoutCount > TIMEOUT_COUNT) {
		// Reset everything.
		UARTTimeoutTimerStatus(false);
		UARTSendLine("ERROR: UART command request timeout");
		idxBuffer = 0;
		rcvComms = false;
		UARTSwitchToDMX();
		
		return;
	}
	
	// Increment the counter.
	timeoutCount++;
}

/**
 * Handles the UART receive interrupt.
 */
void UARTHandleReceive(void) {
	// Detect framing errors.
	if (RC1STAbits.FERR) {
		rcvComms = true;
		
		// Reset the error flag.
		RC1STAbits.SPEN = 0;
		RC1STAbits.SPEN = 1;
		
		// Alert the user that it's clear to send a command.
		UARTSwitchToComms();
		idxBuffer = 0;
		UARTSendChar('>');
		
		// Start the timeout timer.
		UARTTimeoutTimerStatus(true);
		
		return;
	}
	
	// Detect overrun errors.
	if (RC1STAbits.OERR) {
		// Alert the user and switch back to DMX mode.
		UARTSwitchToComms();
		UARTSendLine("ERROR: UART overrun condition");
		UARTSwitchToDMX();
		
		// Reset the error flag.
		RC1STAbits.SPEN = 0;
		RC1STAbits.SPEN = 1;
		
		idxBuffer = 0;
		return;
	}
	
	// Check if we have hit the line limit.
	if (idxBuffer == UART_LINE_MAX_LEN) {
		UARTSwitchToComms();
		UARTSendLine("ERROR: Line buffer size exceeded");
		UARTSwitchToDMX();
		
		idxBuffer = 0;
		return;
	}
	
	// Process received character.
	char c = RC1REG;
	if ((idxBuffer == 0) && (c > 'z'))  // We always get garbage after switching.
		return;
	lineBuffer[idxBuffer++] = c;
	
	
	// Looks like we've reached the end!
	if (c == '\n') {
		// Terminate the line buffer.
		lineBuffer[idxBuffer - 2] = '\0';
		idxBuffer = 0;
		rcvComms = false;
		UARTTimeoutTimerStatus(false);

		// Switch to general communication mode and parse the command line.
		UARTSwitchToComms();
#ifdef DEBUG
		UARTSendLine(lineBuffer);
#endif
		ConsoleParseLine(lineBuffer);

		// Switch back to DMX mode.
		UARTSwitchToDMX();
	}
}

/**
 * Disables the UART RX interrupt.
 */
void UARTDisableRXInterrupt(void) {
	PIE1bits.RCIE = 0;
}

/**
 * Enables the UART RX interrupt.
 */
void UARTEnableRXInterrupt(void) {
	PIE1bits.RCIE = 1;
}

/**
 * Sends a character via UART.
 * 
 * @param c Character to be sent.
 */
void UARTSendChar(const char c) {
	// Wait for the TX buffer to be ready for more data.
	while (!TX1STAbits.TRMT)
		;
	
	// Send character.
	TX1REG = c;
}

/**
 * Sends an 8-bit value via UART and simulates a 2-bit STOP using the 9th bit.
 * 
 * @param data Value to be sent.
 */
void UARTSendValueWith2Stop(const uint8_t data) {
	// Wait for the TX buffer to be ready for more data.
	while (!TX1STAbits.TRMT)
		;
	
	// Send data and simulate 2-bit STOP with 9th bit.
	TX1STAbits.TX9D = 1;
	TX1REG = data;
}

/**
 * Sends a whole string via UART.
 * 
 * @param str String to be sent.
 */
void UARTSendString(const char *str) {
	const char *tmp = str;
	
	while (*tmp) {
		// Wait for the TX buffer to be ready for more data.
		while (!TX1STAbits.TRMT)
			;
		
		// Send data.
		TX1REG = *tmp++;
	}
}

/**
 * Sends a whole string with a CRLF at the end via UART.
 * 
 * @param str String to be sent.
 */
void UARTSendLine(const char *str) {
	const char *tmp = str;
	
	// Send string.
	while (*tmp) {
		// Wait for the TX buffer to be ready for more data.
		while (!TX1STAbits.TRMT)
			;
		
		// Send data.
		TX1REG = *tmp++;
	}
	
	// Send CRLF.
	while (!TX1STAbits.TRMT)
		;
	TX1REG = '\r';
	while (!TX1STAbits.TRMT)
		;
	TX1REG = '\n';
}

/**
 * Sends a number as a string via UART.
 * 
 * @param str String to be sent.
 */
void UARTSendNumber(const uint16_t n) {
	// Convert number into string.
	char str[UINT16_MAX_DIGITS + 1];
	if (!ui16toa(str, n)) {
		UARTSendLine("ERROR: Couldn't convert number into string");
		return;
	}
	
	char *tmp = str;
	while (*tmp) {
		// Wait for the TX buffer to be ready for more data.
		while (!TX1STAbits.TRMT)
			;
		
		// Send data.
		TX1REG = *tmp++;
	}
}

/**
 * Sends a DMX 22-bit BREAK.
 */
void UARTSendDMXBreak(void) {
	// Wait for the TX buffer to be empty.
	while (!TX1STAbits.TRMT)
		;

	// Disable the transmitter.
	TX1STAbits.TXEN = 0;
	UARTDisableDMXTXPPS();
	
	// Send the BREAK and MAB sequence.
	LATC   |= DMX_TX + DMX_TX_EN;
	TRISC  &= ~(DMX_TX);
	LATC   &= ~DMX_TX;
	__delay_us(100);
	LATC   |= DMX_TX;
	__delay_us(10);
	
	// Enable the transmitter.
	UARTEnableDMXTXPPS();
	TX1STAbits.TXEN = 1;
}

/**
 * Switch UART to DMX mode.
 */
void UARTSwitchToDMX(void) {
	// Wait for the TX buffer to be empty.
	while (!TX1STAbits.TRMT)
		;
	
	// Unlock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 0;
	
	// Switch TX output to RC0.
	RC2PPS = 0;
	RC0PPS = 0b10100;
	
	// Lock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 1;
	
	// Setup transmitter for DMX.
	TX1STAbits.TX9  = 1;         // 9th bit used to emulate 2-bit STOP.
	SP1BRG          = BAUD_DMX;  // Baud rate for DMX.
	
	// UART PPS screws with the TRIS register, so go back please.
	TRISC &= ~(UART_TX);
	
	// Enable the RS-485 transceiver.
	LATC |= UART_TX + DMX_TX_EN;
}

/**
 * Switches UART to regular communications mode.
 */
void UARTSwitchToComms(void) {
	// Wait for the TX buffer to be empty.
	while (!TX1STAbits.TRMT)
		;
	
	// Unlock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 0;
	
	// Switch TX output to RC2.
	RC0PPS = 0;
	RC2PPS = 0b10100;
	
	// Lock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 1;
	
	// Setup transmitter for general communications.
	TX1STAbits.TX9  = 0;          // 1-bit STOP.
	SP1BRG          = BAUD_COMMS; // Standard baud rate for communication.

	// Disable the RS-485 transceiver.
	LATC &= ~DMX_TX_EN;
	
	// UART PPS screws with the TRIS register, so go back please.
	TRISC &= ~(DMX_TX);
	LATC  |= DMX_TX;
}

/**
 * Switches UART to configuration mode.
 */
void UARTSwitchToConfig(void) {
	// Switch ports.
	UARTSwitchToComms();
	
	// Change the baud rate for general communications (9600 baud).
	SP1BRG = BAUD_9600;
}

/**
 * Enables the PPS TX output.
 */
void UARTEnableDMXTXPPS(void) {
	UARTSwitchToDMX();
}

/**
 * Disables the PPS TX output.
 */
void UARTDisableDMXTXPPS(void) {
	UARTSwitchToComms();
}

/**
 * Enables the PPS RX input.
 */
void UARTEnableRXPPS(void) {
	// Unlock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 0;
	
	// Set RX input to RC3.
	RXPPS  = 0b10011;
	
	// Lock PPS.
	PPSLOCK               = 0x55;
	PPSLOCK               = 0xAA;
	PPSLOCKbits.PPSLOCKED = 1;
}

/**
 * Sets the status of the timeout timer.
 * 
 * @param on Should we turn ON the timer?
 * @remark This function also resets the timeout counter.
 */
void UARTTimeoutTimerStatus(bool on) {
	timeoutCount = 0;
	T2CONbits.TMR2ON = on;
}

/**
 * Checks if we are currently receiving commands from the computer.
 * 
 * @return TRUE if we are waiting for the computer to send data.
 */
bool IsReceivingComms(void) {
	return rcvComms;//idxBuffer > 0;
}
