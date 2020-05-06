using MQTT.common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT
{
    class Program
    {
        static void Main(string[] args)
        {
            //此处填写购买得到的 MQTT 接入点域名
            String brokerUrl = ConfigurationManager.AppSettings["Mqtt_BrokerUrl"];
            //此处填写阿里云帐号 AccessKey
            String accessKey = ConfigurationManager.AppSettings["Mqtt_AccessKey"];
            //此处填写阿里云帐号 SecretKey
            String secretKey = ConfigurationManager.AppSettings["Mqtt_SecretKey"];
            //此处填写在 MQ 控制台创建的 Topic，作为 MQTT 的一级 Topic
            String parentTopic = ConfigurationManager.AppSettings["Mqtt_ParentTopic"];
            //此处填写客户端 ClientId，需要保证全局唯一，其中前缀部分即 GroupId 需要先在 MQ 控制台创建
            String clientId = ConfigurationManager.AppSettings["Mqtt_GroupId"] + "@@@" + MainHelper.GetRandomString(16, true, true, true, false, "crm");
            MqttClient client = new MqttClient(brokerUrl);
            client.MqttMsgPublishReceived += client_recvMsg;
            client.MqttMsgPublished += client_publishSuccess;
            client.ConnectionClosed += client_connectLose;
            String userName = accessKey;
            //计算签名
            String passWord = MainHelper.HMACSHA1(secretKey, clientId.Split('@')[0]);
            client.Connect(clientId, userName, passWord, true, 60);
            //订阅 Topic，支持多个 Topic，以及多级 Topic
            //string[] subTopicArray = { parentTopic + "/subDemo1", parentTopic + "/subDemo2/level3" };
            string[] subTopicArray = { parentTopic + "/School/", parentTopic + "/School/Sub1" };
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };
            client.Subscribe(subTopicArray, qosLevels);
            Console.WriteLine(DateTime.Now.ToString() + " 开始接收消息队列信息！");
            LogHelper.WriteProgramLog(DateTime.Now.ToString() + " 开始接收消息队列信息！");
            //client.Publish(parentTopic + "/School/", Encoding.UTF8.GetBytes("接收成功！"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            ////发送 P2P 消息，二级 topic 必须是 p2p,三级 topic 是接收客户端的 clientId
            //client.Publish(parentTopic + "/p2p/" + clientId, Encoding.UTF8.GetBytes("hello mqtt"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            //System.Threading.Thread.Sleep(50000);
            //client.Disconnect();
        }
        static void client_recvMsg(object sender, MqttMsgPublishEventArgs e)
        {
            // access data bytes throug e.Message
            Console.WriteLine(DateTime.Now.ToString() + " Recv Msg : Topic is " + e.Topic);
            LogHelper.WriteProgramLog(DateTime.Now.ToString() + " Recv Msg : Topic is " + e.Topic + " ,Body is " + Encoding.UTF8.GetString(e.Message));
            //解析获取到的信息
            try
            {
                string message = Encoding.UTF8.GetString(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString() + " 更新发生错误，请查看错误日志！");
                LogHelper.WriteProgramLog(DateTime.Now.ToString() + " 更新发生错误：" + ex.Message);
            }
            
        }
        static void client_publishSuccess(object sender, MqttMsgPublishedEventArgs e)
        {
            // access data bytes throug e.Message
            Console.WriteLine(DateTime.Now.ToString() + " Publish Msg Success");
            LogHelper.WriteProgramLog(DateTime.Now.ToString() + " Publish Msg Success");
        }
        static void client_connectLose(object sender, EventArgs e)
        {
            // access data bytes throug e.Message
            Console.WriteLine(DateTime.Now.ToString() + " Connect Lost,Try Reconnect");
            LogHelper.WriteProgramLog(DateTime.Now.ToString() + " Connect Lost,Try Reconnect");
        }
    }
}
