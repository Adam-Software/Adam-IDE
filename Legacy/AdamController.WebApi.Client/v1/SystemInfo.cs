

using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    internal class SystemInfo
    {
        private const string cApiPath = "SystemInfo";

        #region System average

        internal static async Task<ExtendedCommandExecuteResult> GetExtendedUptimeAndLoadAverage()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetExtendedUptimeAndLoadAverage");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetUptimeAndLoadAverage()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetUptimeAndLoadAverage");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetLoadAverage()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetLoadAverage");
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region OS version

        internal static async Task<ExtendedCommandExecuteResult> GetOsReleaseVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetOsReleaseVersion");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetDebianOsVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetDebianOsVersion");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetArchitectureOsVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetArchitectureOsVersion");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetKernelVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetKernelVersion");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region CPU/GPU temperature

        internal static async Task<ExtendedCommandExecuteResult> GetGpuTemp()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetGpuTemp");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetCpuTemp()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetCpuTemp");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region Network info

        internal static async Task<ExtendedCommandExecuteResult> GetNetworkInfo()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetNetworkInfo");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetIpInfo()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetIpInfo");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        internal static async Task<ExtendedCommandExecuteResult> GetWiFiSsids()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetWiFiSsids");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion

        #region AdamServer info

        internal static async Task<ExtendedCommandExecuteResult> GetAdamServerVersion()
        {
            HttpResponseMessage? responseMessage = await ApiClient.Get($"{cApiPath}/GetAdamServerVersion");
            
            var result = await responseMessage.ToExtendedCommandResult();
            return result;
        }

        #endregion
    }

}
