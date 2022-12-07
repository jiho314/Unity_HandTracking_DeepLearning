import os
import torch
import numpy as np
import torch.nn as nn
import torch.nn.functional as F
from tqdm import tqdm

from model import resnet34
from DataUtils.load_dataset import QD_Dataset, load_dataset

def train():
    net.train()
    loss_avg = 0.0
    correct = 0
    # info printed in terminal
    # info printed in terminal
    data_loader = tqdm(train_loader, desc='Training')
    # data_loader = train_loader  # info log in logtext
    for batch_idx, (data, target) in enumerate(data_loader):
        data, target = torch.autograd.Variable(data.cuda()), torch.autograd.Variable(target.cuda())

        data = data.view(-1, 1, 28, 28)
        data /= 255.0

        # forward
        output = net(data)

        # backward
        optimizer.zero_grad()
        loss = F.cross_entropy(output, target)
        loss.backward()
        optimizer.step()

        # accuracy
        pred = output.data.max(1)[1]
        correct += float(pred.eq(target.data).sum())

        # exponential moving average
        loss_avg = loss_avg*0.2+float(loss)*0.8

    # seq_train_loss.append(loss_avg)
    # seq_train_acc.append(correct/len(train_loader.dataset))
    print('\nTotal train accuarcy:', correct/len(train_loader.dataset)) # 전체 데이터 개수에서 맞게 예측한 비율
    print('Total train loss:', loss_avg)
    
    
def test():
    net.eval()
    loss_avg = 0.0
    correct = 0
    # info printed in terminal
    data_loader = tqdm(test_loader, desc='Testing')
    # data_loader = test_loader  # info log in logtext
    for batch_idx, (data, target) in enumerate(data_loader):
        data, target = torch.autograd.Variable(data.cuda()), torch.autograd.Variable(target.cuda())

        data = data.view(-1, 1, 28, 28)
        data /= 255.0

        # forward
        output = net(data)
        loss = F.cross_entropy(output, target)

        # accuracy
        pred = output.data.max(1)[1]
        correct += float(pred.eq(target.data).sum())

        # test loss average
        loss_avg += float(loss)

    # seq_test_loss.append(loss_avg/len(test_loader))
    # seq_test_acc.append(correct/len(test_loader.dataset))
    print('\nTotal test accuarcy:', correct/len(test_loader.dataset))
    print('Total test loss:', loss_avg/len(test_loader))
    

if __name__ == '__main__':
    os.chdir('C:/Users/USER/Desktop/project/unity_hand/Unity_HandTracking_DeepLearning/python')
    train_data = QD_Dataset(mtype="train", root='Dataset')
    train_loader = torch.utils.data.DataLoader(train_data, batch_size=256, shuffle=True)

    test_data = QD_Dataset(mtype="test", root='Dataset')
    test_loader = torch.utils.data.DataLoader(test_data, batch_size=64, shuffle=True)
    num_classes = train_data.get_number_classes()
    
    net = resnet34(num_classes)
    net.cuda()
    
    optimizer = torch.optim.SGD(net.parameters(), lr = 0.1, momentum=0.9, weight_decay=5e-4)
    
    epochs = 30
    
    # Main loop
    for epoch in range(epochs):
        print("*"*50)
        print("epoch "+str(epoch+1)+" is running...")
        train()
        print("")
        test()
        print("*"*50)
        
    torch.save(net.state_dict(), os.path.join('checkpoint', 'model.pytorch'))