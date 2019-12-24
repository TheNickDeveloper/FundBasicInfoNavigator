# FundBasicInfoNavigator
WPF project by applyig bond API for indicating the fund real time basic information.

## Design
1. MVVM design pattern by using Caliburn.Micro framework.
2. Xunit apply for unit test.
3. Muti-thread apply while catching bond info.

## Application UI

![image](https://github.com/TheNickDeveloper/FundBasicInfoNavigator/blob/master/image/ApplicationUI.png)

## How to Use
### Select Input Option
1. Manual Search
...Input fund code in the search box. Tf there are more then two funds, seperate them by putting comma mark(e.g. 1,2,3,...).
2. Import CSV file
...Press Browse button, and select the source file as intput file.
    
2. Select Export Option
  -2.1 Display Only
    Display the result on the application UI only.
  -2.2 Export as CSV
    Export result as csv file under definded export folder.
  -2.3 Export as Excel
    Same as above but save as excel file.
  
3. Search / Search & Export
  Press search button, the result will indicate/export in the followinig grild and defined folder :)
