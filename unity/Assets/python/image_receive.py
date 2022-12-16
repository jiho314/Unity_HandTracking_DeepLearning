import socket
import base64
import io
from PIL import Image
import matplotlib.pyplot as plt

#TCP Server (수신)
HOST = '127.0.0.1' #호스트 주소 127.0.0.1
PORT = 9999 #포트 번호 9999

server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) #IPv4프로토콜, TCP소켓 사용하여 소켓 생성
server_socket.bind((HOST, PORT))
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
#print(type(image))
print(image)
plt.imshow(image)
plt.show()