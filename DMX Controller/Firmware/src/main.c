/**
 * MintyDMX
 * A tiny DMX controller that fits neatly inside an Altoids tin.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#include "stdinc.h"
#include "uart.h"
#include "console.h"
#include "dmx.h"

// Private methods.
void EnableInterrupts(void);
void DisableInterrupts(void);
void InitializeIO(void);

/**
 * Main entry point.
 */
void main(void) {
	// Initialize everything.
	DisableInterrupts();
	InitializeIO();
	UARTInitializeTMR2Timeout();
	UARTInitialize();
	EnableInterrupts();
	
	UARTSwitchToComms();
	UARTSendLine("MintyDMX v0.1a");
	
	DMXInitialize(13);
	
	// Main application loop.
	while (true) {
		DMXSendPacket();
		__delay_ms(1);
		
		while (IsReceivingComms())
			__delay_ms(1);
	}
}

/**
 * Interrupt service routine.
 */
void __interrupt() ISR(void) {
	// UART receive interrupt.
	if (PIR1bits.RCIF)
		UARTHandleReceive();
	
	// Timer2 overflow interrupt.
	if (PIR1bits.TMR2IF) {
		UARTTimeoutHandler();
		PIR1bits.TMR2IF = 0;
	}
}

/**
 * Sets up all the I/O pins for the application.
 */
void InitializeIO(void) {
	// Setup digital outputs.
	TRISA = ~(AUX_1);
	TRISC = ~(DMX_TX + UART_TX + DMX_TX_EN);

	// Disable ALL analog inputs.
	ANSELA = 0;
	ANSELC = 0;

	// Setup pull-ups.
	WPUA = 0;
	WPUC = DMX_TX_EN;

	// Setup open-drains.
	ODCONA = 0;
	ODCONC = DMX_TX_EN;

	// Sets the default state of the outputs.
	LATA = 0;
	LATC |= DMX_TX + UART_TX + DMX_TX_EN;
}

/**
 * Enables global interrupts.
 * @remark You should set the interrupts of individual peripherals yourself.
 *         This function will only enable them globally.
 */
void EnableInterrupts(void) {
	INTCONbits.GIE = 1;
	INTCONbits.PEIE = 1;
}

/**
 * Disables global interrupts.
 * @remark You should set the interrupts of individual peripherals yourself.
 *         This function will only disable them globally.
 */
void DisableInterrupts(void) {
	INTCONbits.GIE = 0;
	INTCONbits.PEIE = 0;
}
