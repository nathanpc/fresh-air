/* 
 * uart.h
 * Deals with all of the UART schenenigans.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#ifndef UART_H
#define	UART_H

#include "stdinc.h"

// Definitions.
#define UART_LINE_MAX_LEN 50

// Interrupts.
void UARTTimeoutHandler(void);
void UARTDisableRXInterrupt(void);
void UARTEnableRXInterrupt(void);

// Initialization.
void UARTInitializeTMR2Timeout(void);
void UARTInitialize(void);
void UARTSwitchToDMX(void);
void UARTSwitchToComms(void);
void UARTSwitchToConfig(void);

// Transmission.
void UARTSendDMXBreak(void);
void UARTSendChar(const char c);
void UARTSendValueWith2Stop(const uint8_t data);
void UARTSendString(const char *str);
void UARTSendNumber(const uint16_t n);
void UARTSendLine(const char *str);

// Receiving.
bool IsReceivingComms(void);
void UARTHandleReceive(void);

#endif	/* UART_H */
