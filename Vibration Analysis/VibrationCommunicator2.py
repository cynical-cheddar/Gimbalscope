import serial
import time
import msvcrt
serialPort = serial.Serial(
    port="COM11", baudrate=230400, bytesize=8, timeout=2, stopbits=serial.STOPBITS_ONE
)
serialString = ""  # Used to hold data coming over UART

COM_data = []

foundData = False

fileNameRangeBottom = 78

fileNameRangeTop = 100

fileNameRangeIncrement = 2
i = fileNameRangeBottom

filenames = []

while (i < fileNameRangeTop +fileNameRangeIncrement ):
    name = str(i) + ".txt"
    filenames.append(name)
    i += fileNameRangeIncrement
print(filenames)
fileNameIndex = 0


while 1:
    # Wait until there is data waiting in the serial buffer
    if serialPort.in_waiting > 0:

        # Read data out of the buffer until a carraige return / new line is found
        serialString = serialPort.readline()

        # Print the contents of the serial data
        try:
            #print(serialString.decode("Ascii"))
            #COM_data.append(serialString.decode("Ascii"))
            if(foundData == False):
                print("connection established")
                print("press any key to start data collection")
                foundData = True
        except:
            pass

        if msvcrt.kbhit():
            print("begin")
            break
    elif (not foundData):
        time.sleep(1)
        print("waiting for data")

time.sleep(1)
print("hit enter to start data entry")
input()
print("press K to pause data collection")
while (fileNameIndex < fileNameRangeTop + fileNameRangeIncrement):
    if serialPort.in_waiting > 0:

        # Read data out of the buffer until a carraige return / new line is found
        serialString = serialPort.readline()
        try:
            COM_data.append(serialString.decode("Ascii"))
        except:
            pass
    # advance to next
    if msvcrt.kbhit():
        print("saving to file " + filenames[fileNameIndex])
        filename = filenames[fileNameIndex]
        print(COM_data)
        with open(filename, 'w') as f:
            f.write("-")
        with open(filename, 'a') as f:
            for record in COM_data:
                f.write(record)
        fileNameIndex += 1
        COM_data.clear()
        print("increase the saturation to " +  str(filename) + " and hit ENTER")
        input()
        print("Wait 5 seconds")
        print("press K to pause data collection")





