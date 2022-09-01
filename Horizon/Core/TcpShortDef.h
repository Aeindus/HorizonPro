#pragma once

#define recvHeader(header) client.recvPacketHeader(header,__LINE__)
#define recvData(buffer,len) client.recvData(buffer,len,__LINE__)

//Send Header Packet type,size,arguments
#define sendHeader(type,size,args) client.sendPacketHeader(type,size,args,__LINE__)
#define sendData(buffer,len) client.sendData(buffer,len,__LINE__)
#define sendError(description) client.sendError(description,__LINE__)
#define sendNotification(description) client.sendNotification(description,__LINE__)

