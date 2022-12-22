# Unity_HandTracking_DeepLearning


- Updated with gitignore (~ 22.12.15)    
~~https://kin.naver.com/qna/detail.naver?d1id=1&dirId=10401&docId=393404499&qb=Q1JMRg==&enc=utf8&section=kin.qna&rank=4&search_sort=0&spq=0~~   


   
- Additionally updated without ignore(22.12.16)

- Additional update for unity_build (~22.12.22)

## About project
#### Realtime HandTrack
|1|2|
|:---:|:---:|
|<img src="https://user-images.githubusercontent.com/97828157/209090586-d22dedd1-8571-4fcf-96c6-ca18b8a26c27.gif" width="250"/>|<img src="https://user-images.githubusercontent.com/97828157/209091769-d1e3c0c2-1a89-4d73-be76-7232d6778991.gif" width="250"/>|
<!--
|![handtrack_demo1](https://user-images.githubusercontent.com/97828157/209090586-d22dedd1-8571-4fcf-96c6-ca18b8a26c27.gif)|![handtrack_demo2](https://user-images.githubusercontent.com/97828157/209091769-d1e3c0c2-1a89-4d73-be76-7232d6778991.gif)|
-->

#### Game Playing
|1|2|3|4|5|6|
|:---:|:---:|:---:|:---:|:---:|:---:|
|![1번](https://user-images.githubusercontent.com/61443621/208257091-b3247d4c-fa82-4f99-bde1-62e9159f401a.gif)|![2번](https://user-images.githubusercontent.com/61443621/208257064-46ec52fb-4962-4eda-af6b-b8d73d941f16.gif)|![3번](https://user-images.githubusercontent.com/61443621/208257074-f34738aa-5333-4f46-8070-55abe5f8d569.gif)|![4번](https://user-images.githubusercontent.com/61443621/208257081-60e200be-d45c-4ffd-86f3-b9182e37a38c.gif)|![5번](https://user-images.githubusercontent.com/61443621/208257085-b6eeb8fb-6b8e-4d36-afe7-07d03cbbd346.gif)|![6번](https://user-images.githubusercontent.com/61443621/208257088-ca8b695e-1c34-43eb-95b5-a85a6b05250b.gif)|
* This project is a  **`Unity3D Project`** <br>
* You can draw pictures using certain hand gestures. <br>
* A trained deep learning model will tell you what your drawing is.


## Unity
### How to run
1. First, ensure you have Git LFS.
   ```
   git lfs install  
   ```
2. Then, clone this repo.
   ```
   git clone https://github.com/jiho-00/Unity_HandTracking_DeepLearning   
   ```
3. All of project files are in unity/Assets. This folder includes all scripts and assets to run the project excluding python files.

### Unity demo scenes
|Scene name|Scene|Scene|
|:---:|:---:|:---:|
|Main|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255480-64118f70-3741-41d5-a52f-b0041259606e.png">|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255542-921e5797-7137-47ed-9618-b318d58c2d47.png">|
|Room|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208281368-fc0e4d13-ab7c-4c54-afdf-d1378c5b7298.png">|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208281331-b1af97fd-299b-4eac-b1e3-c756dfb3f7de.png">|
|Drawing|<img width="400" alt="image" src="https://github.com/jiho-00/Unity_HandTracking_DeepLearning/blob/main/screenshot/drawing.png">|<img width="400" alt="image" src="https://user-images.githubusercontent.com/97828157/208268699-f38d8963-13bc-442f-acaf-93cdef1bc66b.png">
<!--
"https://user-images.githubusercontent.com/61443621/208259048-88b252aa-33b9-4f39-9d8f-a219f4024df4.png"
"https://user-images.githubusercontent.com/61443621/208255662-214d6acc-f5c7-4802-b2a5-122d921e400e.png">|
-->

## Python
### Dataset
* [Google quickdraw dataset](https://quickdraw.withgoogle.com/data/)
### How to run(the built game is in the unity_build folder)
1. Preparing your datasets
    ```bash
    ├── data
      ├── txtfiles
      │   ├── 30categories.txt
    ```
2. Making train/test datasets
   ```
   python DataUtils.download_data.py
   ```
3. Main Training
   ```
   python main.py
   ```

4. Inference & Socket communication
   ```
   python InferenceOneImage.py
   ```
