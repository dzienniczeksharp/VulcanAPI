namespace VulcanAPI
{
    public abstract class BaseService
    {
        protected BaseService(VulcanAccount vulcanAccount)
        {
            this.Account = vulcanAccount;
        }

        public VulcanAccount Account { get; }
    }
}
