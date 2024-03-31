namespace AdamController.Services.FlayoutsRegionEventAwareServiceDependency
{
    public class FlyoutAction
    {
        private const string mOpening = "Opening";
        private const string mClosing = "Closing";

        private readonly string mThisAction;

        protected FlyoutAction(string action)
        {
            mThisAction = action;
        }

        public static FlyoutAction Opening
        {
            get { return new FlyoutAction(mOpening); }
        }

        public static FlyoutAction Closing
        {
            get { return new FlyoutAction(mClosing); }
        }

        public string Action
        {
            get { return mThisAction; }
        }

        public override bool Equals(object obj)
        {
            if (obj is not FlyoutAction other)
                return false;

            return this.Action == other.Action;
        }

        public override int GetHashCode()
        {
            return mThisAction.GetHashCode();
        }

        public override string ToString()
        {
            return mThisAction;
        }
    }
}
