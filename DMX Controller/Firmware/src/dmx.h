/* 
 * dmx.h
 * The actual DMX512 controller.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#ifndef DMX_H
#define	DMX_H

#include "stdinc.h"

// Definitions.
#define DMX_MAX_ADDR 512
#define DMX_ARR_ADDR(n) (n + 1)
#define DMX_ADDR_ARR(n) (n - 1)

// Initialization.
void DMXInitialize(const uint16_t lastAddress);

// Universe control.
void DMXSetValue(const uint16_t addr, const uint8_t value);

// DMX operation.
bool DMXIsSendingPacket(void);
void DMXSendPacket(void);

#endif	/* DMX_H */

