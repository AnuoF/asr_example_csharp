# AsrLibrary.dll 语音识别库集成说明 #
## •	说明 ##
本文档旨在简洁明了地说明AsrLibrary.dll语音识别库的集成和调用，方便开发人员集成到软件系统中。
## •	功能介绍 ##
AsrLibrary.dll语音识别库，封装了百度、讯飞、捷通华声的语音识别功能，和多个翻译引擎的接口，并提供统一的调用接口（语音识别和文本翻译），配置灵活，集成度高，发布方便。
AsrLibrary库集成了多种语言的识别能力，包括英语、普通话、粤语、四川话、维吾尔语、国语（台湾）、藏语、藏语安多、藏语康巴、哈萨克语、朝鲜语、彝语、蒙文、广东阳江话、状语、闽南语、上海话、东北话、河南话、天津话、山东话、贵州话、宁夏话、云南话、陕西话、甘肃话、武汉话、河北话、合肥话、长沙话、太原话。需要识别哪种语言，则更改配置文件AsrLibrary.config即可。

同时，AsrLibrary也集成了多个语种的文本翻译功能，包括：英语、维吾尔语、蒙文、藏语、哈萨克语、朝鲜语、彝语、壮语。如有需要，还可以进一步扩展集成其他语种的翻译。

## •	库接口说明 ##
AsrLibrary库对外提供IAsr和ITranslate接口。

IAsr语音识别接口：

    /// <summary>
    /// 语音识别接口。
    /// </summary>
    public interface IAsr : IDisposable
    {
        /// <summary>
        /// 语音识别
        /// </summary>
        /// <param name="audioData">小于 60s 的音频数据（总长度不超过32000），音频格式要求：pcm/16k/16位/单通道 。</param>
        /// <param name="languageType">音频语种</param>
        /// <param name="recogResult">识别成功返回识别结果，识别失败返回错误消息</param>
        /// <returns>识别成功或失败，true-成功；false-失败</returns>
        bool AudioRecog(byte[] audioData, LanguageType languageType, out string recogResult);

        /// <summary>
        /// 获取可识别的语种列表
        /// </summary>
        /// <returns>语种列表</returns>
        List<Language> GetLanguageList();

        /// <summary>
        /// 将配置文件中 text 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="text">text 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        LanguageType Text2LanguageType(string text);

        /// <summary>
        /// 将配置文件中 name 字段映射到 LanguageType 枚举值
        /// </summary>
        /// <param name="name">name 字段内容</param>
        /// <returns>LanguageType 枚举值</returns>
        LanguageType Name2LanguageType(string name);
    }

ITranslate翻译接口：

    /// <summary>
    /// 翻译接口：对外提供
    /// </summary>
    public interface ITranslate
    {
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="text">待翻译的内容</param>
        /// <param name="from">翻译源语种</param>
        /// <param name="result">成功返回翻译结果，失败返回错误消息</param>
        /// <param name="to">翻译目的语种，默认为中文</param>
        /// <returns>true-成功；false-失败</returns>
        bool Trans(string text, LanguageType from, out string result, LanguageType to = LanguageType.Mandarin);

        /// <summary>
        /// 获取支持的语种
        /// </summary>
        /// <returns>支持的语种列表</returns>
        List<Language> GetTransLanguages();
    }

## •	集成步骤 ##
### 1)	添加dll引用，并设置程序私有路径： ###
 
![](https://i.imgur.com/4A2tpBL.png)

添加dll引用

![](https://i.imgur.com/wP77mZb.png)
 
应用程序配置文件中添加私有路径
### 2)	拷贝库文件到指定的目录。将AsrSdk文件夹拷贝到根目录。 ###
 
拷贝SDK到指定目录
### 3)	配置AsrLibrary.config ###
在使用时首先需要配置SDK信息和需要识别的语种，找到配置文件AsrLibrary.config：
#### a)	正确配置SDK信息（这些信息是各开放平台注册之后提供的）： ####
 ![](https://i.imgur.com/v3Waw2w.png)

配置SDK信息

如上图，库集成了百度(baidu)、捷通华声(jths)和讯飞(ifly)，根据实际的应用场景来配置，如果没有用到的，可以不用配置。
#### b)	配置需要识别的语种，如下图所示： ####
 ![](https://i.imgur.com/Xb4Xpja.png)

配置识别的语种

例如，要用百度识别英语的语音，则配置engine=”baidu”，valid=”true”即可。
#### c)	如果需要调用翻译接口，则配置需要翻译的语种，如下图所示： ####
 
配置翻译的语种
例如，要用小牛翻译英语，则配置engine=”niu”，valid=”true”即可。
### 4)	接口调用 ###
经过上述几个步骤的准备工作之后，就可以编码并调试程序，调用方式看参见“Test”项目。拿语音识别接口为例：
#### a)	获取识别接口： ####
    // 1) 获取 Asr 语音识别功能接口
    _asr = AsrFun.GetAsr();

#### b)	调用接口方法： ####
    // 2) 获取可识别的语种列表
    _languageList = _asr.GetLanguageList();
    
    // 3）语音识别
    _asr.AudioRecog(data, type, out result);

#### c)	释放资源(当程序或模块退出时调用此方法，不需要多次调用)： ####
    // 4）语音识别
    _asr.Dispose()
## •	结束 ##
AsrLibrary基础库封装了多种SDK能力，虽经过自测，但不排除存在BUG，或者接口调用不太方便，如果有任何问题，请联系作者修改完善，谢谢！	