/* 
 * pins.h
 * Pin definitions.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

#ifndef PINS_H
#define	PINS_H

//               16F18325
//             +----------+
// ICSP Data  -|RA0    RC0|- DMX Transmitter
// ICSP Clock -|RA1    RC1|- DMX Receiver
// Aux 1      -|RA2    RC2|- UART Transmitter
// Reset      -|RA3    RC3|- UART Receiver
// Aux 2      -|RA4    RC4|- DMX TX Enable
// Aux 3      -|RA5    RC5|- Aux 4
//             +----------+


// Pin definitions.
#define AUX_1     (1 << 2)  // RA2
#define AUX_2     (1 << 4)  // RA4
#define AUX_3     (1 << 5)  // RA5
#define DMX_TX    1         // RC0
#define DMX_RX    (1 << 1)  // RC1
#define UART_TX   (1 << 2)  // RC2
#define UART_RX   (1 << 3)  // RC3
#define DMX_TX_EN (1 << 4)  // RC4
#define AUX_4     (1 << 5)  // RC5

#endif	/* PINS_H */
