using AdamController.WebApi.Client.v1;

namespace AdamController.WebApi.Client
{
    public class BaseApi : IDisposable
    {
        private readonly ApiClient mApiClient;
        private readonly IAdamSdk mAdamSdk;
        private readonly IPythonCommand mPythonCommand;
        private readonly ISystemInfo mSystemInfo;

        public BaseApi(Uri uri, string login, string password)
        {
            mApiClient = new ApiClient(uri, login, password);

            mAdamSdk = new AdamSdk(mApiClient);
            mPythonCommand = new PythonCommand(mApiClient);
            mSystemInfo = new SystemInfo(mApiClient);
        }

        public IAdamSdk AdamSdk { get { return mAdamSdk; } }
        public ISystemInfo SystemInfo { get { return mSystemInfo; } }
        public IPythonCommand PythonCommand { get { return mPythonCommand; } }
        public void Dispose()
        {
            mApiClient.Dispose();
        }

    }
}
