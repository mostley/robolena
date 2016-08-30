from liblo import *
import sys
from time import sleep,time
from random import random
import serial
import math
sys.path.append("..")

BAUDRATE = 57600
PORT = '/dev/cu.usbmodem1411'

servos = serial.Serial()


class MyServer(ServerThread):
    def __init__(self):
        ServerThread.__init__(self, 8888)

    @make_method('/angles', 'ffffff')
    def foo_callback(self, path, args):
        #print args
        #print map(lambda x:math.degrees(x),args)
        anglesData.append(args)

def sendAngles(angles):
    anglestr = ""
    for i in range(len(angles)):
        #angRadians = math.degrees(angles[i])
        #print angRadians
        #angInt = int(angRadians*1024/300)
        anglestr += str(angles[i]) + " "

    anglestr = anglestr[:-1] + '\n'

    servos.write(anglestr)
    servos.flush()

    print "send angles '" + anglestr + "'"

def moveMotors():
    #print "writing on the motors..."
    angles = anglesData[0]

    del anglesData[0]

    sendAngles(angles)


def initMotors():
    global servos
    print "initializing the motor connection..."

    servos.baudrate = BAUDRATE
    servos.port = PORT
    servos.open()

    print servos

    sendAngles([0, 0, 0, 0, 0, 0])



def setup():
    global server, anglesData

    anglesData = []

    #starting oscServer
    try:
        server = MyServer()
    except ServerError, err:
        print "error" + str(err)
        sys.exit()

    print "Starting Server..."
    server.start()

    #initializing servos
    initMotors()


if __name__=="__main__":
    setup()
    timeAverage = 0
    count = 0
    try :
       while 1:
        lastTime = time()
        if(anglesData):
            moveMotors()
        if(time()-lastTime > 0.001):
            print "%f" % (time()-lastTime)
        timeAverage += (time()-lastTime)
        count +=1
        if count >= 100:
           # print "%f" % (timeAverage/100)
            timeAverage = 0
            count = 0

    except KeyboardInterrupt :
        print  "\nStoping OSCServer."
        server.stop()
        servos.close()
        sys.exit()