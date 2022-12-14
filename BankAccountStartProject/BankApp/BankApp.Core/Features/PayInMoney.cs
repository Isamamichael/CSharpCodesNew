using BankApp.Core.DataAccess;
using BankApp.Core.Services;

namespace BankApp.Core.Features
{
    public class PayInMoney
    {
        private IAccountRepository _accountRepository;
        private INotificationService _notificationService;

        public PayInMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(int intoAccountId, decimal amount)
        {
            var into = _accountRepository.GetAccountById(intoAccountId);

            if (amount > 0)
            {
                into.PayIn(amount);
            }
            else if(amount < 0)
            {
                throw new InvalidOperationException("The amount you entered is negative");
            }
            else if(amount == 0)
            {
                throw new Exception("You cannot pay in an amount of value zero");
            }
            
            else
            {
                throw new Exception();
            }
           

            if (into.FraudulentActivityDectected())
            {
                _notificationService.NotifyFraudlentActivity(into);
            }
            _accountRepository.Update(into);
           
        }
    }
}
