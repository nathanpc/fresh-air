/* 
 * dmx.c
 * The actual DMX512 controller.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#include "dmx.h"
#include "uart.h"

// Private variables.
uint8_t universe[DMX_MAX_ADDR];
uint16_t lastArrAddr = DMX_ADDR_ARR(DMX_MAX_ADDR);
bool dmxSending = false;
uint8_t sLATC = 0;

/**
 * Initializes the DMX controller.
 * 
 * @param lastAddress Number of the last address to transmit.
 */
void DMXInitialize(const uint16_t lastAddress) {
	lastArrAddr = DMX_ADDR_ARR(lastAddress);
	
	// Clear the universe buffer just to be sure.
	for (uint16_t i = 0; i < DMX_MAX_ADDR; i++)
		universe[i] = 0;
	
	// Switch communications to DMX.
	UARTSwitchToDMX();
}

/**
 * Sets the value for a specific DMX address.
 * 
 * @param addr  DMX address (1-512).
 * @param value Value to write to the specified DMX address.
 */
void DMXSetValue(const uint16_t addr, const uint8_t value) {
	universe[DMX_ADDR_ARR(addr)] = value;
}

/**
 * Sends the universe buffer as a DMX packet.
 */
void DMXSendPacket(void) {
	// Start the transmission.
	UARTDisableRXInterrupt();
	dmxSending = true;
	UARTSendDMXBreak();  // Send BREAK.
	LATC |= DMX_TX_EN;   // Ensure the RS485 transceiver is enabled.
	
	// Send null start code.
	UARTSendValueWith2Stop(0);
	
	// Send data values for each address.
	for (uint16_t i = 0; i <= lastArrAddr; i++) {
		// Send data.
		UARTSendValueWith2Stop(universe[i]);
	}
	
	// End the transmission.
	dmxSending = false;
	UARTEnableRXInterrupt();
}

/**
 * Checks if we are in the middle of sending a DMX packet.
 * 
 * @return TRUE if we are currently sending a DMX packet.
 */
bool DMXIsSendingPacket(void) {
	return dmxSending;
}
