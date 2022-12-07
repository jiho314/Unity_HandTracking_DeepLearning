import torchvision.models as models
import torch.nn as nn
import torch.nn.functional as F


def resnet34(numclasses, pretrained=False):
    model = models.resnet34(pretrained)
    conv1_out_channels = model.conv1.out_channels
    model.conv1 = nn.Conv2d(1, conv1_out_channels, kernel_size=3,
                            stride=1, padding=1, bias=False)
    model.maxpool = nn.MaxPool2d(kernel_size=2)
    fc_features = model.fc.in_features
    model.fc = nn.Linear(fc_features, numclasses)
    return model