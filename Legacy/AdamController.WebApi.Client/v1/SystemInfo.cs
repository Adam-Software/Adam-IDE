using AdamController.WebApi.Client.Common;
using AdamController.WebApi.Client.v1.ResponseModel;

namespace AdamController.WebApi.Client.v1
{
    #region Interface

    public interface ISystemInfo
    {
        public Task<ExtendedCommandExecuteResult> GetExtendedUptimeAndLoadAverage();
        public Task<ExtendedCommandExecuteResult> GetUptimeAndLoadAverage();
        public Task<ExtendedCommandExecuteResult> GetLoadAverage();
        public Task<ExtendedCommandExecuteResult> GetOsReleaseVersion();
        public Task<ExtendedCommandExecuteResult> GetDebianOsVersion();
        public Task<ExtendedCommandExecuteResult> GetArchitectureOsVersion();
        public Task<ExtendedCommandExecuteResult> GetKernelVersion();
        public Task<ExtendedCommandExecuteResult> GetGpuTemp();
        public Task<ExtendedCommandExecuteResult> GetCpuTemp();
        public Task<ExtendedCommandExecuteResult> GetNetworkInfo();
        public Task<ExtendedCommandExecuteResult> GetIpInfo();
        public Task<ExtendedCommandExecuteResult> GetWiFiSsids();
        public Task<ExtendedCommandExecuteResult> GetAdamServerVersion();
    }

    #endregion

    internal class SystemInfo : ISystemInfo
    {
        #region Const

        private const string cApiPath = "SystemInfo";

        #endregion

        #region Var 

        private readonly ApiClient mApiClient;

        #endregion

        #region ~

        internal SystemInfo(ApiClient apiClient) 
        {
            mApiClient = apiClient;
        }

        #endregion

        #region System average

        public Task<ExtendedCommandExecuteResult> GetExtendedUptimeAndLoadAverage()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetExtendedUptimeAndLoadAverage");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetUptimeAndLoadAverage()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetUptimeAndLoadAverage");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetLoadAverage()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetLoadAverage");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion

        #region OS version

        public Task<ExtendedCommandExecuteResult> GetOsReleaseVersion()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetOsReleaseVersion");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetDebianOsVersion()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetDebianOsVersion");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetArchitectureOsVersion()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetArchitectureOsVersion");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetKernelVersion()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetKernelVersion");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion

        #region CPU/GPU temperature

        public Task<ExtendedCommandExecuteResult> GetGpuTemp()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetGpuTemp");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetCpuTemp()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetCpuTemp");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion

        #region Network info

        public Task<ExtendedCommandExecuteResult> GetNetworkInfo()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetNetworkInfo");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetIpInfo()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetIpInfo");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        public Task<ExtendedCommandExecuteResult> GetWiFiSsids()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetWiFiSsids");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion

        #region AdamServer info

        public Task<ExtendedCommandExecuteResult> GetAdamServerVersion()
        {
            var responseMessage = mApiClient.Get($"{cApiPath}/GetAdamServerVersion");
            var result = responseMessage.ToExtendedCommandResultAsync();
            return result;
        }

        #endregion
    }

}
