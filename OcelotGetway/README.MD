#OcelotGetway   API网关
#APIA    API Client
#APIB    API Client

#依赖包
在OcelotGetway
增加Ocelot,Ocelot.Provider.Consul,Ocelot.Cache.CacheManager，Ocelot.Provider.Polly

#使用Consul作为服务发现
1、下载Consul
2、在系统Path中设置
3、Win+R  cmd 中，运行 consul agent -dev，启动Consul 发现服务；
4、在浏览器中打开  http://localhost:8500/ui/dc1/services，看到UI后台界面
5、至此Consul安装完成
6、在Consul目录中配置文件
7、在Cmd中运行例子，如：consul agent -dev -config-dir=D:/consul_1.5.2_windows_amd64/consul.d
,其中D:/consul_1.5.2_windows_amd64/consul.d是本机的配置文件路径
（文档来源： https://www.jianshu.com/p/05a1bf2545a0）
8、在浏览器中，http://localhost:8500/v1/catalog/service/count，可以看到注册的两个Count服务的信息