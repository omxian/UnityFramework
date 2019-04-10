##如何使用
1. 转一张最简单的表需要的元素
	1. 一个标签页为常规表内容
	2. 另一个标签页名称为`@Types`为定义信息,`TableName: "表名" Package: "table"`
2. 运行bat:   `tabtoy.exe --mode=v2 --lua_out=Test.lua --luaenumintvalue=true --combinename=Test --lan=zh_cn Test.xlsx `


##其他
1. 表提供的各种功能见Sample.xlsx
2. 提供的导出参数见Sample.bat
3. 提供一个方法定义全局的`@Types`,建一张表，只有`@Types`信息，bat导出表时在`XXX.xlsx`前附加全局表。
4. 更多内容请看文档 `https://github.com/davyxu/tabtoy/blob/master/doc/Manual_V2.md`