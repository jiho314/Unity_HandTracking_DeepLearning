# Unity_HandTracking_DeepLearning


- Updated with gitignore (~ 22.12.15)    
~~https://kin.naver.com/qna/detail.naver?d1id=1&dirId=10401&docId=393404499&qb=Q1JMRg==&enc=utf8&section=kin.qna&rank=4&search_sort=0&spq=0~~   


   
- Additionally updated without ignore(22.12.16)

- Additional update for unity_build, upgrading server performance (~22.12.22)

## About project
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
|Room|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255601-4e387590-e1f8-455c-94dc-29131ae1784c.png">|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208258782-7774ee85-dc86-4cd4-b67d-b72ba04dcd84.png">|
|Drawing|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208259048-88b252aa-33b9-4f39-9d8f-a219f4024df4.png">|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255662-214d6acc-f5c7-4802-b2a5-122d921e400e.png">|


## Python
### Dataset
* [Google quickdraw dataset](https://quickdraw.withgoogle.com/data/)
### How to run
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
