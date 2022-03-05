using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace LearningContest.Application.Extension
{
    public interface ILogger
    {
        void Fatal(string message);

        void Error(string message);

        void Warn(string message);

        void Info(string message);

        void Debug(string message);

        void Trace(string message);


        void Fatal(string message, Exception ex);

        void Error(string message, Exception ex);

        void Warn(string message, object value);

        void Info(string message, object value);

        void Debug(string message, object value);

        void Trace(string message, object value);



        void Fatal(string message, Exception ex = null, [CallerMemberName] string methodName = "");

        void Error(string message, Exception ex = null, [CallerMemberName] string methodName = "");

        void Warn(string message, object value = null, [CallerMemberName] string methodName = "");

        void Info(string message, object value = null, [CallerMemberName] string methodName = "");

        void Debug(string message, object value = null, [CallerMemberName] string methodName = "");

        void Trace(string message, object value = null, [CallerMemberName] string methodName = "");



        void Fatal(string followUpKey, string message, Exception ex = null, [CallerMemberName] string methodName = "");

        void Error(string followUpKey, string message, Exception ex = null, [CallerMemberName] string methodName = "");
        void Warn(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "");
        void Info(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "");
        void Debug(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "");
        void Trace(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "");

    }

    public static class LogConstants
    {
        public const string Shop = "#shop";
    }

    public class Serilogger : ILogger
    {

        //private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        public Serilogger()
        {

        }

        public void Fatal(string message)
        {
            Serilog.Log.Fatal(message);
        }

        public void Error(string message)
        {
            //#if RELEASE
            //   _logger.Error(message);
            Serilog.Log.Error(message);
            //#endif
        }

        public void Warn(string message)
        {
            var flag = LogConstants.Shop;
            //#if RELEASE
            //   _logger.Warn(message);
            Serilog.Log.Warning("{Flag}-{Message}", flag, message);
            //#endif
        }

        public void Info(string message)
        {
            var flag = LogConstants.Shop;

            //#if RELEASE
            //   _logger.Info(message);
            Serilog.Log.Information("{Flag}-{Message}", flag, message);
            //#endif
        }

        public void Debug(string message)
        {
            var flag = LogConstants.Shop;

            //#if RELEASE
            //_logger.Debug(message);
            Serilog.Log.Debug("{Flag}-{Message}", flag, message);
            //#endif
        }

        public void Trace(string message)
        {
            var flag = LogConstants.Shop;

            //#if RELEASE
            //_logger.Trace(message);
            Serilog.Log.Verbose("{Flag}-{Message}", flag, message);
            //#endif
        }



        public void Fatal(string message, Exception ex)
        {
            //Serilog.Log.Fatal(ex, $"{message} - {ex.GetFullMessage()}");
            var flag = LogConstants.Shop;

            var valueJson = ex.GetExceptionDetails();

            Serilog.Log.Fatal(ex, "{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }

        public void Error(string message, Exception ex)
        {
            //Serilog.Log.Error(ex, $"{message} - {ex.GetFullMessage()}");
            var flag = LogConstants.Shop;

            var valueJson = ex.GetExceptionDetails();

            Serilog.Log.Error(ex, "{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }

        //public void Error(string message, Exception ex, string body)
        //{
        //    //Serilog.Log.Error(ex, $"{message} - {ex.GetFullMessage()}");
        //    var flag = LogConstants.HnrLogFlag;

        //    var valueJson = ex.GetFullMessage();

        //    Serilog.Log.Error(ex, "{Flag}-{Message}-{@ValueJson}", flag, message+"===|==="+body, valueJson);
        //}


        public void Warn(string message, object value)
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Warning("{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }

        public void Info(string message, object value)
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Information("{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }

        public void Debug(string message, object value)
        {
            //Debug(GetMessage(message, value));
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Debug("{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }

        public void Trace(string message, object value)
        {
            //Trace(GetMessage(message, value));
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Verbose("{Flag}-{Message}-{@ValueJson}", flag, message, valueJson);
        }



        public void Fatal(string message, Exception ex = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = ex?.GetExceptionDetails();

            if (ex == null)
            {
                Serilog.Log.Fatal("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
            }
            else
            {
                Serilog.Log.Fatal(ex, "{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
            }

        }

        public void Error(string message, Exception ex = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = ex?.GetExceptionDetails();

            if (ex == null)
            {
                Serilog.Log.Error("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
            }
            else
            {
                Serilog.Log.Error(ex, "{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
            }
        }

        public void Warn(string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Warning("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
        }

        public void Info(string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Information("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
        }

        public void Debug(string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Debug("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
        }

        public void Trace(string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Verbose("{Flag}-{Message}-{@ValueJson}-{@MethodName}", flag, message, valueJson, methodName);
        }


        public void Fatal(string followUpKey, string message, Exception ex = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = ex?.GetExceptionDetails();

            if (ex == null)
            {
                Serilog.Log.Fatal("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
            }
            else
            {
                Serilog.Log.Fatal(ex, "{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
            }

        }

        public void Error(string followUpKey, string message, Exception ex = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = ex?.GetExceptionDetails();

            if (ex == null)
            {
                Serilog.Log.Error("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
            }
            else
            {
                Serilog.Log.Error(ex, "{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
            }
        }

        public void Warn(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Warning("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
        }

        public void Info(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Information("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
        }

        public void Debug(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Debug("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
        }

        public void Trace(string followUpKey, string message, object value = null, [CallerMemberName] string methodName = "")
        {
            var flag = LogConstants.Shop;

            var valueJson = value == null ? string.Empty : JsonConvert.SerializeObject(value, jsonSerializerSettings);

            Serilog.Log.Verbose("{Flag}-{Message}-{@ValueJson}-{@MethodName}-{@FollowUpKey}", flag, message, valueJson, methodName, followUpKey);
        }


        //private string GetMessage(string message, object value)
        //{
        //    return $"{LogConstants.HnrLogFlag}-{message}-{JsonConvert.SerializeObject(value, jsonSerializerSettings)}";
        //}

    }


}
