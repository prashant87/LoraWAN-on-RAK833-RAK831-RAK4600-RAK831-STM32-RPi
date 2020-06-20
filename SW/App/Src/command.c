#include <command.h>
#include <string.h>
#include "cmdline.h"
#include "main.h"
#include "param.h"
#include "usart.h"

tCmdLineEntry g_psCmdTable[] = { { "help", Cmd_help,
		" : Display list of commands" }, { "set", Cmd_set,
		" : Set the specific param in the next arg" }, { "get", Cmd_get,
		" : Get the specific param in the next arg" },

{ 0, 0, 0 } };

const char * ErrorCode[4] = { "CMDLINE_BAD_CMD", "CMDLINE_TOO_MANY_ARGS",
		"CMDLINE_TOO_FEW_ARGS", "CMDLINE_INVALID_ARG" };

uint8_t commandBuffer[50];
uint8_t commandSendBuffer[50];
uint32_t commandBufferIndex = 0;
uint32_t gotCommandFlag = 0;
uint8_t isEcho = 1;

/*Brief: must be mapped to Rx Interrupt!
 *
 * */
void command_recv_callback_irq(UART_HandleTypeDef *huart) {
	uint8_t receivedChar;
	uint32_t ui32Status;
	char *EnterCMD = "\r\n>";
	receivedChar = (uint8_t) ((*huart).Instance->DR & (uint8_t) 0x00FF);

	if (isEcho == 1)
		HAL_UART_Transmit(huart, &receivedChar, 1, 100);

//	__HAL_UART_ENABLE_IT(&huart1, UART_IT_RXNE);
	if (receivedChar != 13) {
		if ((receivedChar == 8) || (receivedChar == 127)) {
			if (commandBufferIndex > 0)
				commandBufferIndex--;
		} else {
			commandBuffer[commandBufferIndex] = receivedChar;
			commandBufferIndex++;
		}
	} else {
		if (commandBufferIndex != 0) {
			commandBuffer[commandBufferIndex] = 0;
			commandBufferIndex = 0;
			gotCommandFlag = 1;
		}
		if (isEcho == 1)
			HAL_UART_Transmit(huart, EnterCMD, 3, 100);
		PRINTF_EN = 0;
	}

}
int Cmd_set(int argc, char *argv[]) {
	char merg[4];
	if ((strcmp(*(argv + 1), "mb") == 0)) {

		if ((strcmp(*(argv + 2), "mode") == 0)) {
			u_mem_set(NODE_MB_MODE_ADR, atoi(*(argv + 3)));
		} else if ((strcmp(*(argv + 2), "baud") == 0)) {
			u_mem_set(NODE_MB_BAUD_ADR, atoi(*(argv + 3)));
		} else if ((strcmp(*(argv + 2), "db") == 0)) {
			u_mem_set(NODE_MB_DATABITS_ADR, atoi(*(argv + 3)));
		} else if ((strcmp(*(argv + 2), "pari") == 0)) {
			u_mem_set(NODE_MB_PARTITY_ADR, atoi(*(argv + 3)));
		} else if ((strcmp(*(argv + 2), "id") == 0)) {
			u_mem_set(NODE_MB_ID_ADR, atoi(*(argv + 3)));
		}
	} else if ((strcmp(*(argv + 1), "lr") == 0)) {
		if ((strcmp(*(argv + 2), "mode") == 0)) {

			u_mem_set(NODE_MB_MODE_ADR, atoi(*(argv + 3)));

		}
		if ((strcmp(*(argv + 2), "deveui") == 0)) {
			for (uint8_t idx = 0; idx < 8; idx++) {
				memcpy(merg, *(argv + 3) + 2 * idx, 2);
				u_mem_set(NODE_LRWAN_DEVEUI_ADR + idx, (uint8_t) strtol(merg, NULL, 16));
				printf("%d \r\n",  (uint8_t) strtol(merg, NULL, 16));
			}

		}
		if ((strcmp(*(argv + 2), "appeui") == 0)) {
			for (uint8_t idx = 0; idx < 16; idx++) {
				u_mem_set(NODE_LRWAN_APPEUI_ADR + idx, atoi(*(argv + 3)));
			}

		}
		if ((strcmp(*(argv + 2), "appkey") == 0)) {
			for (uint8_t idx = 0; idx < 16; idx++) {
				u_mem_set(NODE_LRWAN_APPKEY_ADR + idx, atoi(*(argv + 3)));
			}

		}
	} else if ((strcmp(*(argv + 1), "id") == 0)) {
		u_mem_set(NODE_ID_ADR, atoi(*(argv + 3)));

	} else if ((strcmp(*(argv + 1), "cf") == 0)) {
		u_mem_set(NODE_HAVE_PARAM_ADR, atoi(*(argv + 3)));
	}
	/* Reponse -----------------------------------------------------*/
	char *reponse = "OK\r\n";
	HAL_UART_Transmit(&huart2, reponse, 4, 1000);

}
int Cmd_get(int argc, char *argv[]) {

	memset(commandSendBuffer, 0, 50);
	strcat(commandSendBuffer, *(argv + 0));
	strcat(commandSendBuffer, " ");
	strcat(commandSendBuffer, *(argv + 1));
	strcat(commandSendBuffer, " ");
	strcat(commandSendBuffer, *(argv + 2));
	strcat(commandSendBuffer, " ");
	if ((strcmp(*(argv + 1), "mb") == 0)) {

		if ((strcmp(*(argv + 2), "mode") == 0)) {
			strcat(commandSendBuffer,
					itoa_user(u_mem_get(NODE_MB_MODE_ADR), 16));
		} else if ((strcmp(*(argv + 2), "baud") == 0)) {
			strcat(commandSendBuffer,
					itoa_user(u_mem_get(NODE_MB_BAUD_ADR), 16));
		} else if ((strcmp(*(argv + 2), "db") == 0)) {
			strcat(commandSendBuffer,
					itoa_user(u_mem_get(NODE_MB_DATABITS_ADR), 16));
		} else if ((strcmp(*(argv + 2), "pari") == 0)) {
			strcat(commandSendBuffer,
					itoa_user(u_mem_get(NODE_MB_PARTITY_ADR), 16));
		} else if ((strcmp(*(argv + 2), "id") == 0)) {
			strcat(commandSendBuffer, itoa_user(u_mem_get(NODE_MB_ID_ADR), 16));
		}
	} else if ((strcmp(*(argv + 1), "lr") == 0)) {
		if ((strcmp(*(argv + 2), "mode") == 0)) {
			strcat(commandSendBuffer,
					itoa_user(u_mem_get(NODE_LRWAN_MODE_ADR), 16));

		}
		if ((strcmp(*(argv + 2), "deveui") == 0)) {
			for (uint8_t idx = 0; idx < 8; idx++) {
				strcat(commandSendBuffer,
						itoa_user(u_mem_get(NODE_LRWAN_DEVEUI_ADR + idx), 16));

				printf("%c %c \r\n", itoa_user(u_mem_get(NODE_LRWAN_DEVEUI_ADR + idx), 16),(itoa_user(u_mem_get(NODE_LRWAN_DEVEUI_ADR + idx), 16)+1));
			}

		}
		if ((strcmp(*(argv + 2), "appeui") == 0)) {
			for (uint8_t idx = 0; idx < 16; idx++) {
				strcat(commandSendBuffer,
						itoa_user(u_mem_get(NODE_LRWAN_APPEUI_ADR + idx), 16));
			}

		}
		if ((strcmp(*(argv + 2), "appkey") == 0)) {
			for (uint8_t idx = 0; idx < 16; idx++) {
				strcat(commandSendBuffer,
						itoa_user(u_mem_get(NODE_LRWAN_APPKEY_ADR + idx), 16));
			}

		}
	} else if ((strcmp(*(argv + 1), "id") == 0)) {
		strcat(commandSendBuffer, itoa_user(u_mem_get(NODE_ID_ADR), 16));

	} else if ((strcmp(*(argv + 1), "cf") == 0)) {
		strcat(commandSendBuffer,
				itoa_user(u_mem_get(NODE_HAVE_PARAM_ADR), 16));
	}
	strcat(commandSendBuffer, "\r\n");
	/* Reponse -----------------------------------------------------*/
	HAL_UART_Transmit(&huart2, commandSendBuffer, strlen(commandSendBuffer),
			1000);

}
int Cmd_help(int argc, char *argv[]) {
	tCmdLineEntry *pEntry;
//      HAL_UART_Transmit(&huart1, "\nAvailable commands\r\n", 1, 100);
// Print some header text.
	printf("\nAvailable commands\r\n");
	printf("------------------\r\n");

// Point at the beginning of the command table.
	pEntry = &g_psCmdTable[0];

// Enter a loop to read each entry from the command table.  The
// end of the table has been reached when the command name is NULL.
	while (pEntry->pcCmd) {
// Print the command name and the brief description.
		printf("%s%s\n\r", pEntry->pcCmd, pEntry->pcHelp);

// Advance to the next entry in the table.
		pEntry++;

	}
	printf("------------------\r\n");
// Return success.
	return (0);
}
///*
//command: SETRGBLED	number RED	GREEN	BLUE	(number = 0-1)
//					SETRGBLED	0 FF	00	1A
//
//*/
//int
//setRGBLED(int argc, char *argv[])
//{
//	if (argc < 5) return 1;
//    	return(update_color(CHARTOBIN(*argv[1]) ,CHAR2TOBIN(argv[2]),CHAR2TOBIN(argv[3]),CHAR2TOBIN(argv[4])));
//
//}
//
///*
//command: SETLIGHT	channel level	(level from 00-FF, channel = 0-F)
//					SETLIGHT	F 80
//
//*/
//
//int
//setLight(int argc, char *argv[])
//{
//	if (argc < 3) return 1;
//  return (setADCChannel(CHARTOBIN(*argv[1]) ,CHAR2TOBIN(argv[2])));
//
//}
//
//int
//setAllLight(int argc, char *argv[])
//{
//	if (argc < 2) return 1;
//   broadcastADCOutput(CHAR2TOBIN(argv[1]));
//
//	return 0;
//}
//
//
///*
//command: CONTROLRELAY	channel state	(state is ON-OFF, channel = 0-3)
//					CONTROLRELAY	0 ON
//*/
//int
//controlRelay(int argc, char *argv[])
//{
//	unsigned char number;
//	if (argc != 3) return 1;
//	number = CHARTOBIN(*argv[1]);
//	if (number > 3) return 1;
// if(my_strcmp( "ON", argv[2]) == 0)
//            {
//                onRelay(CHARTOBIN(*argv[1]));
//							return 0;
//            }
//	else if (my_strcmp( "OFF", argv[2]) == 0)
//            {
//                offRelay(CHARTOBIN(*argv[1]));
//							return 0;
//            }
//						return 1;	//error
//}

/*
 * Brief: Handler for Command line. Must be mapped to one thread!
 *
 *
 * */
void vCmdTask(const void *arg) {
	char *Badcommand = "Bad Command\r\n>";
	int ret;
	while (1) {

		if (gotCommandFlag) {
			gotCommandFlag = 0;
			ret = CmdLineProcess(commandBuffer);
			if (ret == CMDLINE_BAD_CMD)
				HAL_UART_Transmit(&huart2, Badcommand, strlen(Badcommand), 100);
			//					UARTprintf();
		}
		osDelay(100);
	}

}