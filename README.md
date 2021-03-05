# BaiduTranslateAPP
- 使用百度翻译API官方提供的部分代码，进行了部分的修改  
- 百度翻译API官网：https://fanyi-api.baidu.com/  
- 通用翻译文档：https://fanyi-api.baidu.com/doc/21  
- 使用前请在Form1.cs文件下TranslateAPI类下的GetTranslateAsync方法中填写自己的APP ID和秘钥  
- 在翻译前对原始字符串中的换行符进行了替换，替换成了空格，可以部分程度上解决由于复制（特别是从pdf文件中复制）产生的影响翻译的换行符
- 使用了Newtonsoft.Json包来解析json，通过NuGet获取安装的
