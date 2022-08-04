using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Web;

namespace isRock.Template
{
    public class LineQnAWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "c1l4Jg4zOOUgaxyfG6t4nZI6+qhCWxwjBNjN0kYo6hgZ9fIyimoj8uLLS0Vq6bPotLBbp76875hzfCsLKnoHZfnJb7lfxys8+X8DJryr0lCUV+W5P8efNEEv2BDPdpEjQPwlpJ0930cNpOWFFMpC9AdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U2c68adac7cb0891c802d98ac78537eef";
        const string QnAEndpoint = "https://testalbeeqna.azurewebsites.net/qnamaker/knowledgebases/11306280-14af-4958-a63c-15758d708c84/generateAnswer";
        const string QnAKey = "6533ca72-8ece-4a20-bc58-d147c1a40c48";
        const string UnknowAnswer = "不好意思，您可以換個方式問嗎? 我不太明白您的意思...";

        [Route("api/TestQnA")]
        [HttpPost]
        public IActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        //建立 MsQnAMaker Client
                        var helper = new isRock.MsQnAMaker.Client(
                            new Uri(QnAEndpoint), QnAKey);
                        var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        var ret = (from c in QnAResponse.answers
                                   orderby c.score descending
                                   select c
                                ).Take(1);

                        var responseText = UnknowAnswer;
                        if (ret.FirstOrDefault().score > 0)
                            responseText = ret.FirstOrDefault().answer;
                        //回覆
                        this.ReplyMessage(LineEvent.replyToken, responseText);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}