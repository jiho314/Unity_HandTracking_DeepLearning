import cv2
import mediapipe as mp
from cvzone.HandTrackingModule import HandDetector
import socket

# Parameters
width, height = 1280, 720

# Webcam
cap = cv2.VideoCapture(0)

cap.set(3, width)
cap.set(4, height)

# Hand Detector
detector = HandDetector(maxHands=2, detectionCon=0.8)

# Communication
sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 53456)

# make dataset
def make_dataset(lmlist, handType, dataset):
    for lm in lmlist:
        dataset.extend([lm[0], height-lm[1], lm[2]])
    if handType == "Right":
        dataset.insert(0,1)
    else:
        dataset.insert(0,0)

# Start
while True:
    # Get the frame from the webcam
    success, img = cap.read()
    # Hands
    hands, img = detector.findHands(img)

    data = []
    data1 = []
    data2 = []

    if hands:
        if len(hands)==1:
            #Hand 1
            #Get the first hand detected
            hand= hands[0]
            # Get the landmark list
            lmlist = hand["lmList"]
            # Handtype Left or Right
            handType = hand["type"]

            make_dataset(lmlist, handType, data)
            print(data)
            sock.sendto(str.encode(str(data)),serverAddressPort)


        else: # when len(hands) == 2
            hand1, hand2 = hands[0], hands[1]
            lmlist1, lmlist2 = hand1["lmList"], hand2["lmList"]
            handType1, handType2 = hand1["type"], hand2["type"]

            make_dataset(lmlist1, handType1, data1)
            make_dataset(lmlist2, handType2, data2)
            print(data1+data2)
            sock.sendto(str.encode(str(data1+data2)),serverAddressPort)

    cv2.imshow("Image", img)

