UI生成工具文档：

0. 需要更新项目ProjectSettings目录，以及对应代码

1.用法：
	1.1 制作Prefab
	1.2 在Prefab Tag上选择需要生成的UI文件类型（UI_CSharp,UI_Lua）
	1.3 在需要获取的组件上面标记Tag（目前可用类型Texture,Sprite,Lable,GameObject,Toggle,Button,Transform）
	1.4 在Prefab的文档位置上右键选择Generate UI Base View
	
2.配置
	2.1 保存路径可到对应builder的初始化函数SetSavePath处修改
	2.2 日后如需使用自定义的组件，可以新建一个类继承ComponentInfo,然后在初始化函数的SetComponentInfo设置

3.注意：
	3.1 逻辑代码请不要直接写在生成的文件上，最好另外创建一个类继承此类
	3.2 当框架确定后会继续扩展模板

