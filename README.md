# Unity_HandTracking_DeepLearning


- Updated with gitignore (~ 22.12.15)    
~~https://kin.naver.com/qna/detail.naver?d1id=1&dirId=10401&docId=393404499&qb=Q1JMRg==&enc=utf8&section=kin.qna&rank=4&search_sort=0&spq=0~~   


   
- Additionally updated without ignore(22.12.16)

# About project
* This project is a  **`Unity3D Project`** <br>
* You can draw pictures using certain hand gestures. <br>
* A trained deep learning model will tell you what your drawing is.


# Unity
## How to run
1. First, ensure you have Git LFS
   ```
git lfs install
      
    ```
2. Then, clone this repo
   ```
git clone https://github.com/jiho-00/Unity_HandTracking_DeepLearning
      
    ```
3. All of project files are in unity/Assets. This folder includes all scripts and assets to run the project excluding python files.

## Unity demo scenes
|Scene name|Scene|
|:---:|:---:|
|Main|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255480-64118f70-3741-41d5-a52f-b0041259606e.png">| 
|Main|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255542-921e5797-7137-47ed-9618-b318d58c2d47.png">|
|Room|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255601-4e387590-e1f8-455c-94dc-29131ae1784c.png">|
|Drawing|<img width="400" alt="image" src="https://user-images.githubusercontent.com/61443621/208255662-214d6acc-f5c7-4802-b2a5-122d921e400e.png">|


# Python
## Dataset

## How to run
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
