#Bomber-GameDesign MapData文件说明

##简介
Mapdata使用纯文本文件，可使用Excel创建编辑，并保存为.csv文件。


##文件格式
Line 1:     x, y                        //_x,y分别为整个地图行数和列数_

Line 2:     char1, char2, ..., charY    //_一位一个字符，字符数量要和y对应_

.

.

.

Line x+1:   char1, char2, ..., charY    //_一位一个字符，字符数量要和y对应_


##MapComponent字符对照表
| Character     | MapComponent       |
| ------------- | ------------------ |
| #             | Wall Cube          |
| *             | Normal Cube        |
| -             | Only have a Plane  |
| 1             | Player 1           |
| 2             | Player 2           |


##MapData相关类
>`MapDataHelper`
>负责读取地图文件(TextAsset)，创建地图, 运行时可以调用 `MapDataHelper.getInstance`公共方法获取此对象
>
>PS：创建地图前请先设置脚本里的Perfab
>
>`MapUnityEditorHelper`
>在Unity的Inspector面板`MapDataHelper`类Component上添加创建删除地图按钮，方便开发人员调试


##示例
    5,5,,
    #,#,#,#,#
    #,1,-,-,#
    #,-,*,*,#
    #,2,-,-,#
    #,#,#,#,#