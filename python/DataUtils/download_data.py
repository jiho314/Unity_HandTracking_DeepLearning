import urllib.request
import os
import glob
import numpy as np


def load_classname(classtxt_root = "Data/txtfiles/30categories.txt"):
    f = open(classtxt_root,"r")
    # And for reading use
    classes = f.readlines()
    classes = [c.replace('\n','').replace(' ','_') for c in classes]
    return classes


def download(rawdata_root = "Data/"): 
  base = 'https://storage.googleapis.com/quickdraw_dataset/full/numpy_bitmap/'
  for c in classes:
    cls_url = c.replace('_', '%20')
    path = base+cls_url+'.npy'
    print(path)
    urllib.request.urlretrieve(path, rawdata_root +c+'.npy')


if __name__ == '__main__':
    os.chdir('C:/Users/USER/Desktop/project/unity_hand/Unity_HandTracking_DeepLearning/python')
    classes = load_classname()
    download() 
    