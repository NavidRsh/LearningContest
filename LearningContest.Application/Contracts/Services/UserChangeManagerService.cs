using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Contracts.Services
{
    public interface IUserChangeManagerService
    { }
    public class UserChangeManagerService:IUserChangeManagerService
    {
        private ConnectionMultiplexer _redis; 
        public UserChangeManagerService()
        {
            ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("192.168.242.56:32611");
            //InitUserChangeSubscriber();
        }

        //private void InitUserChangeSubscriber()
        //{
        //    var subscriber = _redis.GetSubscriber();
        //    subscriber.Subscribe(RedisCacheSubscriberUserChange, async (channel, message) =>
        //    {
        //        if (string.IsNullOrEmpty(message)) return;
        //        try
        //        {
        //            var eventMessage = JsonConvert.DeserializeObject<UserChangeMessage>(message);
        //            if (eventMessage == null) return;
        //            await _commonPlayerManager.RefreshUserCache(eventMessage);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.ApplicationError(ex);
        //        }
        //    });
        //}
        public void SubscribeToDo(string key)
        {
            var subscriber = _redis.GetSubscriber();
            subscriber.Subscribe(key, (channel, value) =>
            {
                //if ((string)channel == "__keyspace@0__:users" && (string)value == "sadd")
                //{
                //// var str = System.Text.Encoding.Default.GetString(channel);
                //// Do stuff if some item is added to a hypothethical "users" set in Redis
                //}
            });
        }
    }
}
