import os
import cv2
import sys
import torch.nn as nn
import torch.nn.functional as F

from tqdm import tqdm
from torchvision import transforms

# dataset
#from DataUtils.load_dataset import QD_Dataset, load_dataset
from DataUtils.download_data import load_classname
# model
from model import resnet34

import torch
import numpy as np

# for commu
import matplotlib.pyplot as plt
import socket
import base64
import io
from PIL import Image
import time


def loadCnnModel():
    #load cnn model
    device = torch.device('cpu')
    net = resnet34(30)
    PATH = "checkpoint/model.pytorch"
    net.load_state_dict(torch.load(PATH, map_location=device))
    return net


def PredictSingleImage(net, data):
    net.eval()
    loss_avg = 0.0
    correct = 0

    tensorData = torch.from_numpy(data).type(torch.FloatTensor)
    tensorData = tensorData.view(-1, 1, 28, 28)
    tensorData /= 255.0

    # forward
    output = net(tensorData)
    print(output)

    #predict
    _,output_index = torch.max(output, 1)
    print(output_index)

    result = int(output_index[0])

    return result

if __name__ == '__main__':
    os.chdir('C:/Users/USER/Desktop/project/unity_hand/Unity_HandTracking_DeepLearning/python') 
    # Load model
    net = loadCnnModel()
    # Load classname
    classname = load_classname()
    
    #TCP Server (수신)
    
    host = '127.0.0.1' #호스트 주소 127.0.0.1
    port = 9999 #포트 번호 9999

    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) #IPv4프로토콜, TCP소켓 사용하여 소켓 생성
    server_socket.bind((host, port))
    server_socket.listen(1)
    print('waiting')
    connectionSocket, addr = server_socket.accept()
    print('connected') #연결 성공시 'connected' 출력
    len_data = connectionSocket.recv(1024) #수신받을 data의 길이 수신받음
    data = connectionSocket.recv(int(len_data.decode("utf-8"))) #image data 수신받음
    data_base64 = data.decode("utf-8") #data decoding
    print("image received")
    server_socket.close() #소켓 닫기

    #data_base64 : base64 format의 png파일
    imgdata = base64.b64decode(data_base64)
    dataBytesIO = io.BytesIO(imgdata)
    image = Image.open(dataBytesIO) #image : png format의 img파일
    image.save("mydraw/sample.png",'png')
    # 이미지 불러오기
    singleImagePATH = "mydraw/sample.png"
    gray = cv2.imread(singleImagePATH, cv2.IMREAD_GRAYSCALE)
    #gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    cropImage = gray[120:800, 250:930]
    resizeImage = cv2.resize(cropImage, (28, 28))
    # convert numpy
    transform = np.array(resizeImage)
    # 색 반전
    data = np.where(transform < 25, 255, np.where(transform > 25, 0, transform))
    
    # predict
    result = PredictSingleImage(net, data)
    print(classname[result])
    
    # sending result
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  
    # 지정한 host, port 이용하여 서버에 접속
    sock.connect((host, port))
    
    # while True:
    # time.sleep(0.5) #sleep 0.5 sec
    ans = classname[result]
    sock.sendall(ans.encode("UTF-8")) # Converting string to Byte, and sending it to C#
    print("Answer sent!")
    # time.sleep(0.5)
    sock.close() #소켓 닫기
    
