import serial
import time
import msvcrt
serialPort = serial.Serial(
    port="COM8", baudrate=230400, bytesize=8, timeout=2, stopbits=serial.STOPBITS_ONE
)
serialString = ""  # Used to hold data coming over UART

COM_data = []

foundData = False

filename = "L_tilt.txt"


while 1:
    # Wait until there is data waiting in the serial buffer
    if serialPort.in_waiting > 0:

        # Read data out of the buffer until a carraige return / new line is found
        serialString = serialPort.readline()

        # Print the contents of the serial data
        try:
            #print(serialString.decode("Ascii"))
            COM_data.append(serialString.decode("Ascii"))
            if(foundData == False):
                print("connection established")
                foundData = True
        except:
            pass

        if msvcrt.kbhit():
            print("done")
            break
    elif (not foundData):
        time.sleep(1)
        print("waiting for data")

print(COM_data)
with open(filename, 'w') as f:
    f.write("-")
with open(filename, 'a') as f:
    for record in COM_data:
        f.write(record)


