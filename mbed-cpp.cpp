
#include "mbed.h"
#include "EthernetInterface.h"
#include<string>

// Network interface
EthernetInterface net;
DigitalOut kirmiziLED(LED3);
DigitalOut yesilLED(LED1);
// Socket demo
Thread thread;
string VeriSon="0";
void ledler();


void ledtest(){
    kirmiziLED = 1;
    ThisThread::sleep_for(500);
    yesilLED=1;
    ThisThread::sleep_for(500);
    kirmiziLED = 0;
    yesilLED=0;


}

int main()
{
    ledtest();

   net.connect();
   
   while(1){
    // Bring up the ethernet interface
    //printf("Ethernet socket example\n");
    //net.connect();

    // Show the network address
    SocketAddress a;
    net.get_ip_address(&a);
    //printf("IP address: %s\n", a.get_ip_address() ? a.get_ip_address() : "None");

    // Open a socket on the network interface, and create a TCP connection to mbed.org
    TCPSocket socket;
    socket.open(&net);

    net.gethostbyname("192.168.1.104", &a);
    a.set_port(97);
    socket.connect(a);
    // Send a simple http request
    char sbuffer[] = "mc";
    int scount = socket.send(sbuffer, sizeof sbuffer);
    //printf("sent %d [%.*s]\n", scount, strstr(sbuffer, "\r\n") - sbuffer, sbuffer);

    // Recieve a simple http response and print out the response line
    char rbuffer[64];
    int rcount = socket.recv(rbuffer, sizeof rbuffer);
    //printf("recv %d [%.*s]\n", rcount, strstr(rbuffer, "\r\n") - rbuffer, rbuffer);
    int digit1=rcount;
    if (digit1>0){
       //printf("%s\n",rbuffer);
       std::string str(rbuffer);
       printf("%s\n",rbuffer);
       VeriSon=rbuffer;
       if (VeriSon=="000.000.000.002"){
    yesilLED = 1;
    ThisThread::sleep_for(500);
    yesilLED = 0;


}
else if (VeriSon=="000.000.000.001"){
    kirmiziLED = 1;
    ThisThread::sleep_for(500);
    kirmiziLED = 0;
    
}
       

       thread.start(ledler);

    }


    // Close the socket to return its memory and bring down the network interface
    socket.close();

    // Bring down the ethernet interface
    //net.disconnect();
}
}

void ledler(){

 


}